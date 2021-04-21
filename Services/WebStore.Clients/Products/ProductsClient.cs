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

        public IEnumerable<BrandDTO> GetBrands()
        {
            return Get<IEnumerable<BrandDTO>>($"{Address}/brands");
        }

        public ProductDTO GetProductById(int id)
        {
            return Get<ProductDTO>($"{Address}/{id}");
        }

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null)
        {
            return Post(Address, Filter ?? new ProductFilter())
                .Content
                .ReadAsAsync<IEnumerable<ProductDTO>>()
                .Result;
        }

        public IEnumerable<SectionDTO> GetSections()
        {
            return Get<IEnumerable<SectionDTO>>($"{Address}/sections");
        }
    }
}
