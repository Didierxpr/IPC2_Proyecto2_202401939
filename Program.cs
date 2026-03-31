using IPC2_Proyecto2_202401939.Services;

var builder = WebApplication.CreateBuilder(args);

// Registra Razor Pages para construir la interfaz web.
builder.Services.AddRazorPages();

// Registra el estado global en memoria para la aplicacion.
builder.Services.AddSingleton<AppStateService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.Run();
