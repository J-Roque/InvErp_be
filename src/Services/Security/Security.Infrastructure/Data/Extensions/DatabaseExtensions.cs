using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Security.Infrastructure.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.MigrateAsync().GetAwaiter().GetResult();

            await SeedAsync(context);
        }

        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedProfileAsync(context);
        }

        private static async Task SeedProfileAsync(ApplicationDbContext context)
        {
            if (!await context.Profiles.AnyAsync())
            {
                await context.Profiles.AddRangeAsync(InitialData.Profiles);
                await context.SaveChangesAsync();
            }
        }
    }
}
