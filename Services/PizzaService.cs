using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService
{
    private readonly PizzaContext _context;
    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    public IEnumerable<Pizza> GetAll()
    {
        //The Pizzas collection contains all the rows in the pizzas table.
        //The AsNoTracking extension method instructs EF Core to disable change tracking. Because this operation is read-only, AsNoTracking can optimize performance.
        //All of the pizzas are returned with ToList.
        return _context.Pizzas.AsNoTracking().ToList();
    }

    public Pizza? GetById(int id)
    {
        // The Include extension method takes a lambda expression to specify that the Toppings and Sauce navigation properties are to be included in the result by using eager loading. Without this expression, EF Core returns null for those properties.
        //     The SingleOrDefault method returns a pizza that matches the lambda expression.
        //     If no records match, null is returned.
        //     If multiple records match, an exception is thrown.
        //     The lambda expression describes records where the Id property is equal to the id parameter.
        return _context.Pizzas.Include(p => p.Toppings).Include(p => p.Sauce).AsNoTracking()
            .SingleOrDefault(p => p.Id == id);
    }

    public Pizza? Create(Pizza newPizza)
    {
        // newPizza is assumed to be a valid object. EF Core doesn't do data validation, so the ASP.NET Core runtime or user code must handle any validation.
        //     The Add method adds the newPizza entity to the EF Core object graph.
        //     The SaveChanges method instructs EF Core to persist the object changes to the database.
        _context.Pizzas.Add(newPizza);
        _context.SaveChanges();

        return newPizza;
    }

    public void AddTopping(int PizzaId, int ToppingId)
    {
        // References to existing Pizza and Topping objects are created by using Find.
        //     The Topping object is added to the Pizza.Toppings collection with the .Add method. A new collection is created if it doesn't exist.
        //     The SaveChanges method instructs EF Core to persist the object changes to the database.
            
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var toppingToAdd = _context.Toppings.Find(ToppingId);

        if (pizzaToUpdate is null || toppingToAdd is null)
        {
            throw new InvalidOperationException("Pizza or topping does not exist.");
        }

        if (pizzaToUpdate.Toppings is null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
        }
        
        pizzaToUpdate.Toppings.Add(toppingToAdd);

        _context.SaveChanges();
    }

    public void UpdateSauce(int PizzaId, int SauceId)
    {
        // References to existing Pizza and Sauce objects are created by using Find. Find is an optimized method to query records by their primary key. Find searches the local entity graph first before it queries the database.
        //     The Pizza.Sauce property is set to the Sauce object.
        //     An Update method call is unnecessary because EF Core detects that you set the Sauce property on Pizza.
        //     The SaveChanges method instructs EF Core to persist the object changes to the database.
        
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var sauceToUpdate = _context.Sauces.Find(SauceId);

        if (pizzaToUpdate is null || sauceToUpdate is null)
        {
            throw new InvalidOperationException("Pizza or sauce doesn't exist");
        }

        pizzaToUpdate.Sauce = sauceToUpdate;
        _context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        // The Find method retrieves a pizza by the primary key (which is Id in this case).
        // The Remove method removes the pizzaToDelete entity in EF Core's object graph.
        //     The SaveChanges method instructs EF Core to persist the object changes to the database.
        
        var pizzaToDelete = _context.Pizzas.Find(id);
        if (pizzaToDelete is not null)
        {
            _context.Pizzas.Remove(pizzaToDelete);
            _context.SaveChanges();
        }
    }
}