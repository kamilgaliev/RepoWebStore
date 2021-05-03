using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandMapper
    {
        public static BrandDTO ToDTO(this Brand Brand)
        {
            return Brand is null
                ? null
                : new BrandDTO
                {
                    Id = Brand.Id,
                    Name = Brand.Name,
                    Order = Brand.Order,
                    ProductsCount = Brand.Products.Count(),
                };
        }

        public static Brand FromDTO(this BrandDTO BrandDTO)
        {
            return BrandDTO is null
                ? null
                : new Brand
                {
                    Id = BrandDTO.Id,
                    Name = BrandDTO.Name,
                    Order = BrandDTO.Order,
                };
        }

        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> Brands) => Brands.Select(ToDTO);

        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO> Brands) => Brands.Select(FromDTO);
    }
}
