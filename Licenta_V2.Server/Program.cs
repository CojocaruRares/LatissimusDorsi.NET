using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseStrings"));
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TrainerService>();
builder.Services.AddSingleton<FirebaseAuthService>();
builder.Services.AddSingleton<WorkoutService>();
builder.Services.AddSingleton<PdfService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<TrainingSessionService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Public"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => {
    options
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
    }
);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");
    
app.Run();
