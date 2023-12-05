using Serilog;
using IdentityServer;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("STARTING UP");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(
            outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    Log.Information("SEEDING DATABASE...");
    SeedData.EnsureSeedData(app);
    Log.Information("DONE SEEDING DATABASE. EXITING.");

    app.Run();
}
catch (Exception ex) when (
    ex.GetType().Name is not "StopTheHostException"
    && ex.GetType().Name is not "HostAbortedException"
)
{
    Log.Fatal(ex, "UNHANDLED EXCEPTION");
}
finally
{
    Log.Information("SHUT DOWN COMPLETE");
    Log.CloseAndFlush();
}