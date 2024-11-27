using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using PartsServer; // Asegúrate de importar el namespace de FirestoreHelper
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Inicializa Firestore usando el FirestoreHelper
FirestoreHelper.SetEnviromentVariable();

// Agregar FirestoreDb como un servicio singleton
builder.Services.AddSingleton(FirestoreHelper.Database);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
