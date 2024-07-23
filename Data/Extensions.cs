namespace ContosoPizza.Data;

public static class Extensions
{
    public static void CreateDbIfNotExists(this IHost host)
    {
        {
            // The CreateDbIfNotExists method is defined as an extension of IHost.
            //
            //     A reference to the PizzaContext service is created.
            //
            //     EnsureCreated ensures that the database exists.
            //     Important
            //
            //     If a database doesn't exist, EnsureCreated creates a new database. The new database isn't configured for migrations, so use this method with caution.
            //     The DbIntializer.Initialize method is called. The PizzaContext object is passed as a paramete
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<PizzaContext>();
                context.Database.EnsureCreated();
                DbInitializer.Initialize(context);

            }
        }
    }
}