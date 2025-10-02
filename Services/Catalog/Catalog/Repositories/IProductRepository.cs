using Catalog.Entities;
using Catalog.Specifications;


namespace Catalog.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Pagination<Product>> GetProducts(CatalogSpecParams specParams);
        Task<IEnumerable<Product>> GetProductsByName(string productName);
        Task<IEnumerable<Product>> GetProductsByBrand(string brandName);
        Task<Product> GetProduct(string productId);
        Task<Product> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string productId);
        Task<ProductBrand> GetBrandByBrandIdAsync(string brandId);
        Task<ProductType> GetTypeByTypeIdAsync(string typeId);
    }
}
