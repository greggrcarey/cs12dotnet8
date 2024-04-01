using Microsoft.EntityFrameworkCore;//Include
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Northwind.EntityModels;// NorthwindDb, Category, Product
using Microsoft.EntityFrameworkCore.ChangeTracking;

partial class Program
{
    private static void QueryingCategories()
    {
        using NorthwindDb db = new();

        SectionTitle("Categories and how many products they have");

        //A Query to get all categories and their related products.
        IQueryable<Category>? categories = db.Categories;
            //.Include(c => c.Products); //.Include() -> Eager loading

        if(categories is  null || !categories.Any())
        {
            Fail("No categories found.");
            return;
        }

        //Execute query and enumerate results
        foreach(Category c in categories)
        {
            WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
        }
    }

    private static void FilteredIncludes()
    {
        using NorthwindDb db = new();

        SectionTitle("Products with a minimum number of units in stock");

        string? input;
        int stock;

        do
        {
            Write("Enter a minium for units in stock: ");
            input = ReadLine();

        }while(!int.TryParse(input, out stock));

        IQueryable<Category>? categories = db.Categories?
            .TagWith("Categories by stock with minimum amount")
            .Include(c => c.Products.Where(p => p.Stock >= stock));

        if (categories is null || !categories.Any())
        {
            Fail("No catergories found");
            return;
        }
        Info($"ToQueryString{categories.ToQueryString()}");

        foreach (Category c in categories)
        {
            WriteLine($"{c.CategoryName} has {c.Products.Count} Products in stock.");

            foreach(Product p in c.Products)
            {
                WriteLine($"     {p.ProductName} has {p.Stock} units in stock");
            }
        }
    }

    private static void QueryingProducts()
    {
        using NorthwindDb db = new();

        SectionTitle("Products that cost more than a Price, highest at top");

        string? input;
        decimal price;

        do
        {
            WriteLine("Enter a product price");
            input = ReadLine();

        } while (!decimal.TryParse(input, out price));

        IQueryable<Product>? products = db.Products?
            .TagWith("Products that cost more than an amount, sorted descending")
            .Where(p => p.Cost > price)
            .OrderByDescending(p =>  p.Cost);

        if( products is null || !products.Any())
        {
            Fail("No Products found.");
            return;
        }

        Info($"ToQueryString{products.ToQueryString()}");

        foreach (Product p in products)
        {
            WriteLine("{0}: {1} costs {2:$#,##0.00} and has {3} in stock.", p.ProductId, p.ProductName, p.Cost, p.Stock);
        }
    }

    private static void GettingOneProduct()
    {
        using NorthwindDb db = new();

        SectionTitle("Getting a single product");

        string? input;
        int id;

        do
        {
            WriteLine("Enter a product ID: ");
            input = ReadLine();

        }while(!int.TryParse(input, out id));

        Product? product = db.Products?
            .First(p => p.ProductId == id);

        Info($"First: {product?.ProductName}");

        if (product is null) Fail("No product found using First");

        product = db.Products?
            .Single(p => p.ProductId == id);

        Info($"Single: {product?.ProductName}");

        if (product is null) Fail("No product found using single");
        /*
         Difference between .First, .FirstOrDefault, and .Single
            .First - EF generates a LIMIT 1 on the SQL
                    - Throws an Exception if there are no records returned

            .FirstOrDefault - EF generates a LIMIT 1 on the SQL 
                    - Throws a NULL if there are no records returned

            .Single - EF generates a LIMIT 2 on the SQL 
                    - Throw an exception if more than 1 record is returned

         Good practice - use .First to avoid retrieving 2 records
         */

    }

    private static void QueryingWithLike()
    {
        using NorthwindDb db = new();

        SectionTitle("Pattern Matching with Like");

        Write("Enter part of a product name: ");

        string? input = ReadLine();

        if(string.IsNullOrWhiteSpace(input))
        {
            Fail("You did not enter part of a product name");
            return;
        }

        IQueryable<Product>? products = db.Products?
            .Where(p => EF.Functions.Like(p.ProductName, $"%{input}%"));

        if(products is null || !products.Any())
        {
            Fail("No products found");
            return;
        }

        foreach(Product p in products)
        {
            WriteLine($"{p.ProductName} has {p.Stock} units in stock. Discontinued: {p.Discontinued}");
        }

    }

    private static void GetRandomProduct()
    {
        using NorthwindDb db = new();

        SectionTitle("Get a random product");

        int? rowCount = db.Products?.Count();

        if(rowCount is null)
        {
            Fail("Products table is empty");
            return;
        }

        Product? p = db.Products?.FirstOrDefault(p => p.ProductId == (int)(EF.Functions.Random() * rowCount));

        if(p is null)
        {
            Fail("Product not found.");
            return;
        }

        WriteLine($"Random Product: {p.ProductId} - {p.ProductName}");

    }

    private static void QueryingCategoriesWithExplicitLoading()
    {
        using NorthwindDb db = new();

        SectionTitle("Categories and how many products they have");

        //A Query to get all categories and their related products.
        IQueryable<Category>? categories;
        //= db.Categories;
        //.Include(c => c.Products); //.Include() -> Eager loading

        db.ChangeTracker.LazyLoadingEnabled = false;

        Write("Enable eager loading (Y/N)");

        bool eagerLoading = (ReadKey().Key == ConsoleKey.Y);
        bool explicitLoading = false;

        WriteLine();
        if(eagerLoading)
        {
            categories = db.Categories?.Include(c => c.Products);
        }
        else
        {
            categories = db.Categories;
            Write("Enable explict loading? (Y/N): ");
            explicitLoading = (ReadKey().Key == ConsoleKey.Y);
            WriteLine();
        }

        if (categories is null || !categories.Any())
        {
            Fail("No categories found.");
            return;
        }

        //Execute query and enumerate results
        foreach (Category c in categories)
        {
            if (explicitLoading)
            {
                Write($"Explicitly load products for {c.CategoryName}? (Y/N): ");
                ConsoleKeyInfo key = ReadKey();
                WriteLine();
                if (key.Key == ConsoleKey.Y)
                {
                    CollectionEntry<Category, Product> products = db.Entry(c).Collection(c2 => c2.Products);
                    if (!products.IsLoaded) products.Load();
                }
            }

            WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
        }
        
        //article about when to choose what form of loading, Eager, Lazy or Explicit
        //https://learn.microsoft.com/en-us/ef/core/querying/relateddata

        //page 583
    }

}
