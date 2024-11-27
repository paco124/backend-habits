using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PartsService.Controllers;
using PartsService.Models;
using System.Net;
using System.Text.Json;

namespace PartsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Habits : BaseController
    {
        private readonly FirestoreDb _firestoreDb;

        public Habits(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }

        // GET: api/Habits
        [HttpGet]
        public async Task<ActionResult> GetHabits()
        {
            CollectionReference habitsRef = _firestoreDb.Collection("Habits");
            QuerySnapshot snapshot = await habitsRef.GetSnapshotAsync();

            var habits = new List<object>();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                habits.Add(document.ToDictionary());
            }

            return Ok(habits);
        }

        // GET: api/Habits/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetHabit(int id)
        {
            CollectionReference habitsRef = _firestoreDb.Collection("Habits");
            Query query = habitsRef.WhereEqualTo("id", id);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
                return NotFound(new { message = "Hábito no encontrado." });

            return Ok(snapshot.Documents[0].ToDictionary());
        }

        // POST: api/Habits
        [HttpPost]
        public async Task<ActionResult> CreateHabit([FromBody] HabitModelCreate newHabit)
        {
            CollectionReference habitsRef = _firestoreDb.Collection("Habits");

            // Obtener el último ID
            Query query = habitsRef.OrderByDescending("id").Limit(1);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            int lastId = snapshot.Documents.Count > 0
                ? snapshot.Documents[0].GetValue<int>("id")
                : 0;

            var habitData = new
            {
                id = lastId + 1,
                name = newHabit.name,
                description = newHabit.description
            };

            DocumentReference docRef = await habitsRef.AddAsync(habitData);

            return Created(docRef.Id, habitData);
        }

        // PUT: api/Habits/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHabit(int id, [FromBody] HabitModel updatedHabit)
        {
            CollectionReference habitsRef = _firestoreDb.Collection("Habits");
            Query query = habitsRef.WhereEqualTo("id", id);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
                return NotFound(new { message = "Hábito no encontrado." });

            DocumentReference docRef = snapshot.Documents[0].Reference;
            await docRef.SetAsync(new
            {
                id = id,
                name = updatedHabit.name,
                description = updatedHabit.description
            }, SetOptions.Overwrite);

            return Ok(new { message = "Hábito actualizado correctamente." });
        }

        // DELETE: api/Habits/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHabit(int id)
        {
            CollectionReference habitsRef = _firestoreDb.Collection("Habits");
            Query query = habitsRef.WhereEqualTo("id", id);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
                return NotFound(new { message = "Hábito no encontrado." });

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                await document.Reference.DeleteAsync();
            }

            return Ok(new { message = "Hábito eliminado correctamente." });
        }

        public class HabitModel 
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class HabitModelCreate
        {
            public string name { get; set; }
            public string description { get; set; }
        }

    }
}
