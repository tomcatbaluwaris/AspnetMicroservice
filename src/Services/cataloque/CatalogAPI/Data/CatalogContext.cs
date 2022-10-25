using CatalogAPI.Entities;
using MongoDB.Driver;

namespace CatalogAPI.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products
    {
        get;
    }

    public CatalogContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatbaseSettings.ConnectionString"));
        var mongoDatabase = client.GetDatabase(configuration.GetValue<string>("DatbaseSettings.DatabaseName"));
        Products = mongoDatabase.GetCollection<Product>(configuration.GetValue<string>("DatbaseSettings:CollectionName"));

        CatalogContextSeed.SeedData(Products);
    }
    
}