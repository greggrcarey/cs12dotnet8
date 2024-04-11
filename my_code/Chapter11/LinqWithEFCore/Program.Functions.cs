using Microsoft.EntityFrameworkCore; //Dbset<T>
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Northwind.EntityModels; //NorthwindDb, Cateory, Product

partial class Program
{
    private static void FilterAndSort()
    {
        SectionTitle("Filter and Sort");

        using NorthwindDb db = new();

        DbSet<Product> allProducts = db.Products;

        IQueryable<Product> filteredProducts = allProducts.Where(product => product.UnitPrice < 10M);

        IOrderedQueryable<Product> sortedAndFilteredProducts = filteredProducts.OrderByDescending(product => product.UnitPrice);

        WriteLine("Products that cost less than $10: ");
        //Inefficient since we call all columns 
        WriteLine(sortedAndFilteredProducts.ToQueryString());

        foreach (Product p in sortedAndFilteredProducts)
        {
            WriteLine("{0}: {1} costs {2:$#,##0.00}", p.ProductId, p.ProductName, p.UnitPrice);
        }

    }

    private static void FilterAndSortWithSelect()
    {
        SectionTitle("Filter and Sort");

        using NorthwindDb db = new();

        DbSet<Product> allProducts = db.Products;

        IQueryable<Product> filteredProducts = allProducts.Where(product => product.UnitPrice < 10M);

        IOrderedQueryable<Product> sortedAndFilteredProducts = filteredProducts.OrderByDescending(product => product.UnitPrice);

        //This "projects" the query into the anomous method with only the properties needed.
        var projectedProducts = sortedAndFilteredProducts
            .Select(product => new
            {
                product.ProductId,
                product.ProductName,
                product.UnitPrice,
            });

        WriteLine("Products that cost less than $10: ");
        //Inefficient since we call all columns 
        //WriteLine(sortedAndFilteredProducts.ToQueryString());

        WriteLine(projectedProducts.ToQueryString());
        foreach (var p in projectedProducts)
        {
            WriteLine("{0}: {1} costs {2:$#,##0.00}", p.ProductId, p.ProductName, p.UnitPrice);
        }

    }

    public static void JoinCategoriesAndProducts()
    {
        SectionTitle("Join categories and products");

        using NorthwindDb db = new();

        // Join every product to its category to return 77 matches.
        var queryJoin = db.Categories.Join(
            inner: db.Products,
            outerKeySelector: category => category.CategoryId,
            innerKeySelector: product => product.CategoryId,
            resultSelector: (c, p) => new { c.CategoryName, p.ProductName, p.ProductId })
            .OrderBy(cp => cp.CategoryName);// Order by something category name

        WriteLine(queryJoin.ToQueryString());

        foreach (var p in queryJoin)
        {
            WriteLine($"{p.ProductId}: {p.ProductName} in {p.CategoryName}");
        }
    }

    public static void GroupJoinCategoriesAndProducts()
    {
        SectionTitle("Group join categories and products");

        using NorthwindDb db = new();

        var queryGroup = db.Categories.AsEnumerable().GroupJoin(
            inner: db.Products,
            outerKeySelector: category => category.CategoryId,
            innerKeySelector: product => product.CategoryId,
            resultSelector: (c, matchingProducts) => new
            {
                c.CategoryName,
                Products = matchingProducts.OrderBy(p => p.ProductName)
            });

        /*
         AsEnumerable is required since this query cannot be converted into SQL by EF
        LINQ to Ef Core brings the data to the application 
        LINQ to Objects executes the query in memory, but this is less efficent
         */


        foreach (var c in queryGroup)
        {
            WriteLine($"{c.CategoryName} has {c.Products.Count()} products.");

            foreach (var product in c.Products)
            {
                WriteLine($"    {product.ProductName}");
            }
        }
    }

    private static void ProductsLookup()
    {
        SectionTitle("Products Lookup");

        using NorthwindDb db = new();

        //Join all products o their categories to return 77 matches
        var productQuery = db.Categories.Join(
            inner: db.Products,
            outerKeySelector: category => category.CategoryId,
            innerKeySelector: product => product.CategoryId,
            resultSelector: (c, p) => new { c.CategoryName, Product = p });

        //The ToLookup method keeps the lookup available in memory 
        ILookup<string, Product> productLookup = productQuery.ToLookup(
            keySelector: cp => cp.CategoryName,
            elementSelector: cp => cp.Product);

        foreach (IGrouping<string, Product> group in productLookup)
        {
            WriteLine($"{group.Key} has {group.Count()} products.");

            foreach (Product product in group)
            {
                WriteLine($"    {product.ProductName}");
            }
        }

        Write("Enter a category name: ");
        string categoryName = ReadLine()!;
        WriteLine();
        WriteLine($"Products in {categoryName}: ");

        IEnumerable<Product> productsInCategory = productLookup[categoryName];

        foreach (Product product in productsInCategory)
        {
            WriteLine($"    {product.ProductName}");
        }


    }

    private static void AggregateProducts()
    {
        SectionTitle("Aggregate Products");

        using NorthwindDb db = new();

        //Try to get an efficient cont from EF Core DbSet<T>.
        if (db.Products.TryGetNonEnumeratedCount(out int dBSetCount))
        {
            WriteLine($"{"Product count from DbSet:",-25} {dBSetCount,10}");
        }
        else
        {
            WriteLine("Product DbSet does not have a Count Property");
        }

        //Try to get an effiecnt count from List<T>
        List<Product> products = [.. db.Products];

        if (products.TryGetNonEnumeratedCount(out int listCount))
        {
            WriteLine($"{"Product count from product list:",-25} {listCount,10}");
        }
        else
        {
            WriteLine("Product list does not have a Count Property");
        }

        WriteLine($"{"Product count:",-25} {db.Products.Count(),10}");

        WriteLine($"{"Discount product count:",-27} {db.Products.Count(p => p.Discontinued),8}");

        WriteLine($"{"Highest product price:",-25} {db.Products.Max(p => p.UnitPrice),10:$#,##0.00}");

        WriteLine($"{"Sum of units in stock:",-25} {db.Products.Sum(p => p.UnitsInStock),10:N0}");

        WriteLine($"{"Sum of units on order:",-25} {db.Products.Sum(p => p.UnitsOnOrder),10:N0}");

        WriteLine($"{"Average unit price:",-25} {db.Products.Average(p => p.UnitPrice),10:$#,##0.00}");

        WriteLine($"{"Value of units in stock:",-25} {db.Products.Sum(p => p.UnitPrice * p.UnitsInStock),10:$#,##0.00}");
    }

    private static void OutputTableOfProducts(Product[] products, int currentPage, int totalPages)
    {
        string line = new('-', count: 73);

        string lineHalf = new('-', count: 30);

        WriteLine(line);
        WriteLine("{0,4} {1,-40} {2,12} {3,-15}", "ID", "Product Name", "Unit Price", "Discontinued");
        WriteLine(line);

        foreach (Product p in products)
        {
            WriteLine("{0,4} {1,-40} {2,12:C} {3,-15}", p.ProductId, p.ProductName, p.UnitPrice, p.Discontinued);
        }

        WriteLine("{0} Page {1} of {2} {3}", lineHalf, currentPage + 1, totalPages + 1, lineHalf);
    }

    private static void OutputPageOfProducts(IQueryable<Product> products, int pageSize, int currentPage, int totalPages)
    {
        // We must order data before skipping and taking to ensure
        // the data is not randomly sorted in each page.
        var pagingQuery = products
            .OrderBy(p => p.ProductId)
            .Skip(currentPage * pageSize).Take(pageSize);

        Clear(); // Clear the console/screen.

        SectionTitle(pagingQuery.ToQueryString());

        OutputTableOfProducts([.. pagingQuery], currentPage, totalPages);
    }

    private static void PagingProducts()
    {
        SectionTitle("Paging products");
        using NorthwindDb db = new();
        int pageSize = 10;
        int currentPage = 0;
        int productCount = db.Products.Count();
        int totalPages = productCount / pageSize;
        while (true) // Use break to escape this infinite loop.
        {
            OutputPageOfProducts(db.Products, pageSize, currentPage, totalPages);
            Write("Press <- to page back, press -> to page forward, any key to exit.");
            ConsoleKey key = ReadKey().Key;
            if (key == ConsoleKey.LeftArrow)
                currentPage = currentPage == 0 ? totalPages : currentPage - 1;
            else if (key == ConsoleKey.RightArrow)
                currentPage = currentPage == totalPages ? 0 : currentPage + 1;
            else
                break; // Break out of the while loop.
            WriteLine();
        }
    }
}