using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;

namespace MyInvestAPI.Extensions
{
    public static class ActiveUpdateMigrationsExtensions
    {
        public static void ActiveUpdateDatabaseMigrations(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            if (env.IsProduction())
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<MyInvestContext>();

                    context.Database.Migrate();
                }
            }
        }
    }
}
