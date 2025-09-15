using Microsoft.EntityFrameworkCore;

namespace LiquourApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Asegurarse de que la base de datos está creada
            context.Database.EnsureCreated();

            // Verificar si ya hay usuarios en la base de datos
            if (context.Users.Any())
            {
                return; // La base de datos ya ha sido inicializada
            }

            // Aquí puedes agregar datos iniciales si es necesario
        }
    }
}