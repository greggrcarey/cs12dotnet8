using Northwind.EntityModels;

//using NorthwindDb db = new();
//WriteLine($"Provider: {db.Database.ProviderName}");

//ConfigureConsole();
//QueryingCategories();
//FilteredIncludes();

//QueryingProducts();

//GettingOneProduct();
//QueryingWithLike();

//GetRandomProduct();

//QueryingCategoriesWithExplicitLoading();
//LazyLoadingWithNoTracking();

//ListProducts();

//var resultAdd = AddProduct(categoryId: 6, productName: "Bob's Burgers", price: 500M, stock: 72);

//if (resultAdd.affected == 1)
//{
//    WriteLine($"Add product succesful with ID: {resultAdd.productId}");
//}

//ListProducts(productsToHighlight: [resultAdd.productId]);

//var (affected, productId) = IncreasePrice(productStartsWith: "Bob", amount: 20M);

//if (affected == 1)
//{
//    WriteLine($"Increase price succesful with ID: {productId}");
//}

//ListProducts(productsToHighlight: [productId]);


var resultUpdateBetter = IncreaseProductPricesBetter(productNameStartsWith: "Bob", amount: 20M);

if (resultUpdateBetter.affected > 0)
{
    WriteLine("Increase product price successful.");
}
ListProducts(productsToHighlight: resultUpdateBetter.productIds);

//page 594