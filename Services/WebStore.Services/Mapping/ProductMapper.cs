using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel ToView(this Product product) => product is null ? null : new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand?.Name
        };

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<Product> products) => products.Select(ToView);
        public static Product FromView(this ProductViewModel product) => product is null ? null : new Product
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Brand = product.Brand is null ? null : new Brand { Name = product.Name}
        };

        public static ProductDTO ToDTO(this Product Product)
        {
            return Product is null
                ? null
                : new ProductDTO
                {
                    Id = Product.Id,
                    Name = Product.Name,
                    Price = Product.Price,
                    Order = Product.Order,
                    ImageUrl = Product.ImageUrl,
                    Brand = Product.Brand.ToDTO(),
                    Section = Product.Section.ToDTO(),
                };
        }

        public static Product FromDTO(this ProductDTO ProductDTO)
        {
            return ProductDTO is null
                ? null
                : new Product
                {
                    Id = ProductDTO.Id,
                    Name = ProductDTO.Name,
                    Price = ProductDTO.Price,
                    Order = ProductDTO.Order,
                    ImageUrl = ProductDTO.ImageUrl,
                    BrandId = ProductDTO.Brand?.Id,
                    SectionId = ProductDTO.Section.Id,
                    Brand = ProductDTO.Brand.FromDTO(),
                    Section = ProductDTO.Section.FromDTO(),
                };
        }

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> Products) => Products.Select(ToDTO);

        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> Products) => Products.Select(FromDTO);

    }
}
