using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using S5_L5_Progetto_Settimanale_AndreaGuarnieri.Models;

var builder = WebApplication.CreateBuilder(args);

// Aggiungi i servizi al contenitore
builder.Services.AddControllersWithViews();

// Aggiungi i servizi personalizzati
builder.Services.AddScoped<IAnagrafica, AnagraficaService>();
builder.Services.AddScoped<ITipoViolazione, TipoViolazioneService>();
builder.Services.AddScoped<IVerbale, VerbaleService>();
builder.Services.AddScoped<IUtente, UtenteService>();

// Configurazione autenticazione e autorizzazione
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15); // Durata del cookie di autenticazione
        options.SlidingExpiration = true; // Rinnova il cookie se l'utente è attivo
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User", "Admin"));
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
