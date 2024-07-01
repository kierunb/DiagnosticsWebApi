using DiagnosticsWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddTransient<UserProfileDataGeneratorService>();
builder.Services.AddSingleton<IUserProfileCache, BadUserProfileCache>();
//builder.Services.AddSingleton<IUserProfileCache, BetterUserProfileCache>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
