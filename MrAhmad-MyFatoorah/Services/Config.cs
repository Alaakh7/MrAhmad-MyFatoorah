using Microsoft.EntityFrameworkCore;
using Models;

namespace MrAhmad_MyFatoorah.Services
{
    public class Config
    {
        static DbContextOptionsBuilder<Context> dbBuilder;
        static IConfigurationBuilder configbuilder;
        static IConfigurationRoot configuration;
        public static Context GetContext()
        {
            dbBuilder = new DbContextOptionsBuilder<Context>();
            dbBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new Context(dbBuilder.Options);
        }

        public static void SetJeson()
        {
            configbuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
            configuration = configbuilder.Build();
        }
        public static string GetConnectionString()
        {
            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}
