using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;

var builder = WebApplication.CreateBuilder(args);

// Aggiungi i servizi al contenitore
builder.Services.AddControllersWithViews();

// Aggiungi i servizi personalizzati
builder.Services.AddScoped<IAnagrafica, AnagraficaService>();
builder.Services.AddScoped<ITipoViolazione, TipoViolazioneService>();
builder.Services.AddScoped<IVerbale, VerbaleService>();

var app = builder.Build();

// Configura la pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
