using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BANA.Data;
using BANA.IA;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ArchiveContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ArchiveContext") ?? throw new InvalidOperationException("Connection string 'ArchiveContext' not found.")));
builder.Services.AddDbContext<RondeContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RondeContext") ?? throw new InvalidOperationException("Connection string 'RondeContext' not found.")));
builder.Services.AddDbContext<FinalFicheContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FinalFicheContext") ?? throw new InvalidOperationException("Connection string 'FinalFicheContext' not found.")));
builder.Services.AddDbContext<Fiche5Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Fiche5Context") ?? throw new InvalidOperationException("Connection string 'Fiche5Context' not found.")));
builder.Services.AddDbContext<Fiche4Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Fiche4Context") ?? throw new InvalidOperationException("Connection string 'Fiche4Context' not found.")));
builder.Services.AddDbContext<Fiche3Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Fiche3Context") ?? throw new InvalidOperationException("Connection string 'Fiche3Context' not found.")));
builder.Services.AddDbContext<Fiche2Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Fiche2Context") ?? throw new InvalidOperationException("Connection string 'Fiche2Context' not found.")));
builder.Services.AddDbContext<Fiche1Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Fiche1Context") ?? throw new InvalidOperationException("Connection string 'Fiche1Context' not found.")));
builder.Services.AddDbContext<VaccinationContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("VaccinationContext") ?? throw new InvalidOperationException("Connection string 'VaccinationContext' not found.")));
builder.Services.AddDbContext<FicheAssuranceContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FicheAssuranceContext") ?? throw new InvalidOperationException("Connection string 'FicheAssuranceContext' not found.")));
builder.Services.AddDbContext<LNMEContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LNMEContext") ?? throw new InvalidOperationException("Connection string 'LNMEContext' not found.")));
builder.Services.AddDbContext<DmiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DmiContext") ?? throw new InvalidOperationException("Connection string 'DmiContext' not found.")));
builder.Services.AddDbContext<AppointmentContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AppointmentContext") ?? throw new InvalidOperationException("Connection string 'AppointmentContext' not found.")));
builder.Services.AddDbContext<HospiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("HospiContext") ?? throw new InvalidOperationException("Connection string 'HospiContext' not found.")));
builder.Services.AddDbContext<CimContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CimContext") ?? throw new InvalidOperationException("Connection string 'CimContext' not found.")));
builder.Services.AddDbContext<ParametreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ParametreContext") ?? throw new InvalidOperationException("Connection string 'ParametreContext' not found.")));
builder.Services.AddDbContext<FicheContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FicheContext") ?? throw new InvalidOperationException("Connection string 'FicheContext' not found.")));

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
 //   serverOptions.ListenAnyIP(4148); // ou 443 pour HTTPS
//});

builder.Services.AddDbContext<HonoraireContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("HonoraireContext") ?? throw new InvalidOperationException("Connection string 'HonoraireContext' not found.")));

builder.Services.AddDbContext<ResultatContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ResultatContext") ?? throw new InvalidOperationException("Connection string 'ResultatContext' not found.")));

builder.Services.AddDbContext<TransactionStockContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TransactionStockContext") ?? throw new InvalidOperationException("Connection string 'TransactionStockContext' not found.")));

builder.Services.AddDbContext<ExamenContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ExamenContext") ?? throw new InvalidOperationException("Connection string 'ExamenContext' not found.")));

builder.Services.AddDbContext<AssuranceContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AssuranceContext") ?? throw new InvalidOperationException("Connection string 'AssuranceContext' not found.")));

builder.Services.AddDbContext<PaidContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PaidContext") ?? throw new InvalidOperationException("Connection string 'PaidContext' not found.")));

builder.Services.AddDbContext<ChambreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ChambreContext") ?? throw new InvalidOperationException("Connection string 'ChambreContext' not found.")));

builder.Services.AddDbContext<DoctorContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DoctorContext") ?? throw new InvalidOperationException("Connection string 'DoctorContext' not found.")));

builder.Services.AddDbContext<StaffContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("StaffContext") ?? throw new InvalidOperationException("Connection string 'StaffContext' not found.")));

builder.Services.AddDbContext<ContactContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContactContext") ?? throw new InvalidOperationException("Connection string 'ContactContext' not found.")));

builder.Services.AddDbContext<DepartementContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DepartementContext") ?? throw new InvalidOperationException("Connection string 'DepartementContext' not found.")));

builder.Services.AddDbContext<ServiceContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ServiceContext") ?? throw new InvalidOperationException("Connection string 'ServiceContext' not found.")));


builder.Services.AddDbContext<FactureContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FactureContext") ?? throw new InvalidOperationException("Connection string 'FactureContext' not found.")));

builder.Services.AddDbContext<PatientContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PatientContext") ?? throw new InvalidOperationException("Connection string 'PatientContext' not found.")));

builder.Services.AddDbContext<TacheContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TacheContext") ?? throw new InvalidOperationException("Connection string 'TacheContext' not found.")));

builder.Services.AddDbContext<EmplacementContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EmplacementContext") ?? throw new InvalidOperationException("Connection string 'EmplacementContext' not found.")));

builder.Services.AddDbContext<StockContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("StockContext") ?? throw new InvalidOperationException("Connection string 'StockContext' not found.")));


builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UserContext") ?? throw new InvalidOperationException("Connection string 'UserContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Services AI
builder.Services.AddHttpClient();
builder.Services.AddScoped<IAIService, GeminiService>();

//Authentification
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CookieAuthHm";
    options.DefaultChallengeScheme = "CookieAuthHm";
})
.AddCookie("CookieAuthDmi", config =>
{
    config.Cookie.Name = "AuthBANAdmi.Cookie";
    config.LoginPath = "/User/Login2";
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromHours(1);

})
.AddCookie("CookieAuthHm", config =>
{
    config.Cookie.Name = "AuthBANA.Cookie";
    config.LoginPath = "/User/Login";
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromHours(1);

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Stock/Page404";
        await next();
    }
});
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Departement}/{action=Index}/{id?}");

app.Run();
