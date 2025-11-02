using Microsoft.EntityFrameworkCore;
using Sprint1.Domain.Repositories;
using Sprint1.Infrastructure.Data;
using Sprint1.Infrastructure.Data.UseCase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleDb"))
);

// Register repositories and use cases
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioUseCase, UsuarioUseCase>();
builder.Services.AddScoped<TestConnectionUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Rotas personalizadas
app.MapControllerRoute(
    name: "usuario-details",
    pattern: "usuarios/detalhes/{id:int}",
    defaults: new { controller = "Usuario", action = "Details" });

app.MapControllerRoute(
    name: "usuario-create",
    pattern: "usuarios/criar",
    defaults: new { controller = "Usuario", action = "Create" });

app.MapControllerRoute(
    name: "usuario-edit-email",
    pattern: "usuarios/{id:int}/alterar-email",
    defaults: new { controller = "Usuario", action = "EditEmail" });

app.MapControllerRoute(
    name: "usuario-edit-senha",
    pattern: "usuarios/{id:int}/alterar-senha",
    defaults: new { controller = "Usuario", action = "EditSenha" });

app.MapControllerRoute(
    name: "usuario-test-connection",
    pattern: "usuarios/testar-conexao",
    defaults: new { controller = "Usuario", action = "TestConnection" });

// Rota padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
