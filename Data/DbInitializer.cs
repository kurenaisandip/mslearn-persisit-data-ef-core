﻿using ContosoPizza.Models;

namespace ContosoPizza.Data;

public static class DbInitializer
{
    // The DbInitializer class and Initialize method are both defined as static.
    // Initialize accepts a PizzaContext object as a parameter.
    public static void Initialize(PizzaContext context)
    {
        //If there are no records in any of the three tables, Pizza, Sauce, and Topping objects are created.
        if (context.Pizzas.Any() && context.Toppings.Any() && context.Sauces.Any())
        {
            return; // DB has been seeded
        }
        
        var pepperoniTopping = new Topping { Name = "Pepperoni", Calories = 130 };
        var sausageTopping = new Topping { Name = "Sausage", Calories = 100 };
        var hamTopping = new Topping { Name = "Ham", Calories = 70 };
        var chickenTopping = new Topping { Name = "Chicken", Calories = 50 };
        var pineappleTopping = new Topping { Name = "Pineapple", Calories = 75 };
        
        var tomatoSauce = new Sauce { Name = "Tomato", IsVegan = true };
        var alfredoSauce = new Sauce { Name = "Alfredo", IsVegan = false };
        
        var pizzas = new Pizza[]
        {
            new Pizza
            { 
                Name = "Meat Lovers", 
                Sauce = tomatoSauce, 
                Toppings = new List<Topping>
                {
                    pepperoniTopping, 
                    sausageTopping, 
                    hamTopping, 
                    chickenTopping
                }
            },
            new Pizza
            { 
                Name = "Hawaiian", 
                Sauce = tomatoSauce, 
                Toppings = new List<Topping>
                {
                    pineappleTopping, 
                    hamTopping
                }
            },
            new Pizza
            { 
                Name="Alfredo Chicken", 
                Sauce = alfredoSauce, 
                Toppings = new List<Topping>
                {
                    chickenTopping
                }
            }
        };

        //The Pizza objects (and their Sauce and Topping navigation properties) are added to the object graph by using AddRange.
        //The object graph changes are committed to the database by using SaveChanges.
        context.Pizzas.AddRange(pizzas);
        context.SaveChanges();
    }
    
}