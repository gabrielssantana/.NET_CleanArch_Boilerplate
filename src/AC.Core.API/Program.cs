
using AC.Core.API.Configurations;

using Serilog;

namespace AC.Core.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAPI(builder.Configuration);
            Log.Information("Starting application 🤘");
            var app = builder.Build();
            app.UseAPI(builder.Configuration);
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            //Exceção esperada, lançada pelo CLI do Entity Framework.
            var type = ex.GetType().Name;
            if (type.Equals("HostAbortedException", StringComparison.Ordinal))
            {
                throw;
            }

            Log.Fatal(ex, "Error starting application 😰");
            throw;
        }
        finally
        {
            Log.Information("Finishing application 😴");
            Log.CloseAndFlush();
        }
    }
}
