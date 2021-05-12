using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration Configuration) : base(Configuration, WebAPI.Products)
        {
        }

        public BrandDTO GetBrandById(int id)
        {
            return Get<BrandDTO>($"{Address}/brands/{id}");
        }

        public IEnumerable<BrandDTO> GetBrands()
        {
            return Get<IEnumerable<BrandDTO>>($"{Address}/brands");
        }

        public ProductDTO GetProductById(int id)
        {
            return Get<ProductDTO>($"{Address}/{id}");
        }

        public PageProductsDTO GetProducts(ProductFilter Filter = null)
        {
            return Post(Address, Filter ?? new ProductFilter())
                .Content
                .ReadAsAsync<PageProductsDTO>()
                .Result;
        }

        public SectionDTO GetSectionById(int id)
        {
            return Get<SectionDTO>($"{Address}/sections/{id}");
        }

        public IEnumerable<SectionDTO> GetSections()
        {
            return Get<IEnumerable<SectionDTO>>($"{Address}/sections");
        }
    }
}
