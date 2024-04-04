using Microsoft.EntityFrameworkCore; //ExecuteUpdate, ExecuteDelete
using Microsoft.EntityFrameworkCore.ChangeTracking; //EntityEntry<T>
using Northwind.EntityModels;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335; //Northwind, Product
using Microsoft.EntityFrameworkCore.Storage; //IDBContextTransaction
partial class Program
{
    private static void ListProducts(int[]? productsToHighlight = null)
    {
        using NorthwindDb db = new();

        if (db.Products is null || !db.Products.Any())
        {
            Fail("There are no Products");
            return;
        }

        WriteLine("| {0,-3} | {1,-35} | {2,8} | {3,5} | {4} |","Id", "Product Name", "Cost", "Stock", "Disc.");

        foreach (Product p in db.Products)
        {
            ConsoleColor previousColor = ForegroundColor;

            if(productsToHighlight is not null && productsToHighlight.Contains(p.ProductId))
            {
                ForegroundColor = ConsoleColor.Green;
            }

            WriteLine("| {0:000} | {1,-35} | {2,8:$#,##0.00} | {3,5} | {4} |", p.ProductId, p.ProductName, p.Cost, p.Stock, p.Discontinued);

            ForegroundColor = previousColor;
        }

    }

    private static (int affected, int productId) AddProduct(int categoryId, string productName, decimal? price, short? stock)
    {
        using NorthwindDb db = new();

        if (db.Products is null) return (0, 0);

        Product newProduct = new()
        {
            CategoryId = categoryId,
            ProductName = productName,
            Cost = price,
            Stock = stock,
            SupplierId = 2
        };
        
        EntityEntry<Product> entity = db.Products.Add(newProduct);
        WriteLine($"State: {entity.State}, Entity: {newProduct.ProductId}");

        int affected = db.SaveChanges();
        WriteLine($"State: {entity.State}, ProductId: {newProduct.ProductId}");

        return (affected, newProduct.ProductId);

    }

    private static (int affected, int productId) IncreasePrice(string productStartsWith, decimal amount)
    {
        using NorthwindDb db = new();

        if(db.Products is null) return (0, 0);

        Product updateProduct = db.Products.First(
            p => p.ProductName.StartsWith(productStartsWith));

        updateProduct.Cost += amount;

        int affected = db.SaveChanges();

        return(affected, updateProduct.ProductId);

    }

    private static int DeleteProducts(string productNameStartsWith)
    {
        using NorthwindDb db = new();
        using IDbContextTransaction t = db.Database.BeginTransaction();

        WriteLine($"Transaction isolation level: {t.GetDbTransaction().IsolationLevel}");

        IQueryable<Product>? products = db.Products?
            .Where(p => p.ProductName.StartsWith(productNameStartsWith));

        if (products is null || !products.Any())
        {
            WriteLine("No products found to delete.");
            return 0;
        }
        else
        {
            if (db.Products is null) return 0;
            db.Products.RemoveRange(products);
        }

        int affected = db.SaveChanges();
        t.Commit();
        return affected;
    }

    private static (int affected, int[]? productIds) IncreaseProductPricesBetter(string productNameStartsWith, decimal amount) 
    {
        using NorthwindDb db = new();

        if (db.Products is null) return (0, null);

        IQueryable<Product>? products = db.Products
            .Where(p => p.ProductName.StartsWith(productNameStartsWith));

        //if( products is null || !products.Any())
        //{
        //    Fail($"No Product starting with \"{productNameStartsWith}\" found");
        //    return(0, null);
        //}

        int affected = products.ExecuteUpdate(s => s.SetProperty(
            p => p.Cost, //Property selector Lambda expression
            p => p.Cost + amount //Value to update lambda expression
            ));

        int[] productIds = [.. products.Select(p => p.ProductId)];

        return(affected, productIds);

    }


}