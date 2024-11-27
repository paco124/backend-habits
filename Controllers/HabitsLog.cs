using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using PartsService.Controllers;

namespace PartsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitsLog : BaseController
    {
        public class HabitLog
        {
            public int id { get; set; }
            public int habit_id { get; set; }
            public DateTime date { get; set; }
            public bool status { get; set; }
        }

        public class HabitLogCreate
        {
            public int habit_id { get; set; }
            public DateTime date { get; set; }
            public bool status { get; set; }
        }

        private readonly FirestoreDb _firestoreDb;

        public HabitsLog(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }

        // Create
        [HttpPost("log")]
        public async Task<ActionResult> AddHabitLog([FromBody] HabitLogCreate habitLog)
        {
            CollectionReference logsRef = _firestoreDb.Collection("Habits_Log");

            // Obtener el último id existente
            Query query = logsRef.OrderByDescending("id").Limit(1);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            int lastId = snapshot.Documents.Count > 0
                ? snapshot.Documents.First().GetValue<int>("id")
                : 0;

            // Asignar un nuevo id
            int newId = lastId + 1;

            // Crear el log con el nuevo id
            await logsRef.AddAsync(new
            {
                id = newId,
                habit_id = habitLog.habit_id,       
                date = habitLog.date,
                status = habitLog.status
            });

            return Ok(new { message = "Log registrado correctamente.", newId });
        }


        // Read
        [HttpGet("logs")]
        public async Task<ActionResult> GetHabitLogs()
        {
            CollectionReference logsRef = _firestoreDb.Collection("Habits_Log");
            QuerySnapshot snapshot = await logsRef.GetSnapshotAsync();

            var logs = snapshot.Documents.Select(doc => new HabitLog
            {
                id = doc.ContainsField("id") ? doc.GetValue<int>("id") : 0,
                date = doc.ContainsField("date") ? doc.GetValue<Timestamp>("date").ToDateTime() : default,
                habit_id = doc.ContainsField("habit_id") ? doc.GetValue<int>("habit_id") : 0,
                status = doc.ContainsField("status") ? doc.GetValue<bool>("status") : false,
            }).ToList();

            return Ok(logs);
        }

        // Update
        [HttpPut("log/{id}")]
        public async Task<ActionResult> UpdateHabitLog(int id, [FromBody] HabitLog updatedLog)
        {
            CollectionReference logsRef = _firestoreDb.Collection("Habits_Log");
            Query query = logsRef.WhereEqualTo("id", id);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
            {
                return NotFound(new { message = "Log no encontrado." });
            }

            foreach (var doc in snapshot.Documents)
            {
                await doc.Reference.UpdateAsync(new Dictionary<string, object>
                {
                    { "habit_id", updatedLog.habit_id },
                    { "date", updatedLog.date },
                    { "status", updatedLog.status }
                });
            }

            return Ok(new { message = "Log actualizado correctamente." });
        }

        // Delete
        [HttpDelete("log/{id}")]
        public async Task<ActionResult> DeleteHabitLog(int id)
        {
            CollectionReference logsRef = _firestoreDb.Collection("Habits_Log");
            Query query = logsRef.WhereEqualTo("id", id);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
            {
                return NotFound(new { message = "Log no encontrado." });
            }

            foreach (var doc in snapshot.Documents)
            {
                await doc.Reference.DeleteAsync();
            }

            return Ok(new { message = "Log eliminado correctamente." });
        }

        // Get by ID
        [HttpGet("log/{id}")]
        public async Task<ActionResult> GetHabitLogById(int id)
        {
            CollectionReference logsRef = _firestoreDb.Collection("Habits_Log");
            Query query = logsRef.WhereEqualTo("id", id);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
            {
                return NotFound(new { message = "Log no encontrado." });
            }

            var habitLog = snapshot.Documents.Select(doc => new HabitLog
            {
                id = doc.ContainsField("id") ? doc.GetValue<int>("id") : 0,
                date = doc.ContainsField("date") ? doc.GetValue<Timestamp>("date").ToDateTime() : default,
                habit_id = doc.ContainsField("habit_id") ? doc.GetValue<int>("habit_id") : 0,
                status = doc.ContainsField("status") ? doc.GetValue<bool>("status") : false,
            }).FirstOrDefault();

            return Ok(habitLog);
        }

    }
}
