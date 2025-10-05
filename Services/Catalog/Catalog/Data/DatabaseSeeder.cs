using Catalog.Entities;
using MongoDB.Driver;
using System;
using System.Text.Json;

namespace Catalog.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(IConfiguration configration)
        {
            var client = new MongoClient(configration["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(configration["DatabaseSettings:DatabaseName"]);
            var products = db.GetCollection<Product>(configration["DatabaseSettings:ProductCollectionName"]);
            var brands = db.GetCollection<ProductBrand>(configration["DatabaseSettings:BrandsCollectionName"]);
            var types = db.GetCollection<ProductType>(configration["DatabaseSettings:TypeCollectionName"]);

            var SeedBasePath = Path.Combine("Data", "SeedData");

            // Seed Brands
            List<ProductBrand> brandList = new();
            if((await brands.CountDocumentsAsync(_ => true)) == 0)
            {
                var brandData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "brands.json"));
                brandList = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                await brands.InsertManyAsync(brandList);
            }
            else
            {
                brandList = await brands.Find(_ => true).ToListAsync();
            }

            // Seed Types
            List<ProductType> typeList = new();
            if ((await types.CountDocumentsAsync(_ => true)) == 0)
            {
                var typeData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "types.json"));
                typeList = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                await types.InsertManyAsync(typeList);
            }
            else
            {
                typeList = await types.Find(_ => true).ToListAsync();
            }

            // Seed Products
            if ((await products.CountDocumentsAsync(_ => true)) == 0)
            {
                var productData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "products.json"));
                var productList = JsonSerializer.Deserialize<List<Product>>(productData);
                foreach (var product in productList)
                {
                    // Reset Id to let Mongo generate a new one
                    product.Id = null;
                    // Default CreatedData if not set
                    if(product.CreatedDate == default)
                        product.CreatedDate = DateTime.UtcNow;
                }
                await products.InsertManyAsync(productList);
            }

        }
    }
}
