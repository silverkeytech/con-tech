using ConTech.Core.Features.Project;
using ConTech.Data.DatabaseSpecific;
using ConTech.Web;
using ConTech.Web.Extensions;
using Microsoft.Extensions.Configuration;
using OrchardCore.Localization;
using SD.LLBLGen.Pro.DQE.SqlServer;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SK.Framework.Email;
using System.Data.SqlClient;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddRazorPages();

builder.Services.RegisterApplicationServices(builder.Configuration, builder.Environment);

builder.Services.AddPortableObjectLocalization(options => options.ResourcesPath = "Localization");
builder.Services.AddSingleton<ILocalizationFileLocationProvider, MultiplePoFilesLocationProvider>();

ConfigureLLBLGen(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); 
app.MapRazorPages();

app.Run();

static void ConfigureLLBLGen(IServiceCollection services, IConfiguration config)
{
    var co = config.GetConnectionString("SqlServer");
    RuntimeConfiguration.AddConnectionString("ConnectionString.SQL Server (SqlClient)", config.GetConnectionString("SqlServer"));
    RuntimeConfiguration.ConfigureDQE<SQLServerDQEConfiguration>(c =>
    {
        c.AddDbProviderFactory(typeof(SqlClientFactory));
        c.SetTraceLevel(TraceLevel.Verbose);
    });

    services.AddSingleton<DataAccessAdapter>();
}