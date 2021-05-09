using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;
using WebStore.Domain.ViewModels;
using Microsoft.Extensions.Configuration;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private const string __CatalogPageSize = "CatalogPageSize";

        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData ProductData, IConfiguration Configuration )
        {
            _ProductData = ProductData;
            _Configuration = Configuration;
        }

        public IActionResult Index(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            var page_size = PageSize
                ?? (int.TryParse(_Configuration[__CatalogPageSize], out var value) ? value : null);
            var filter = new ProductFilter
            { 
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = page_size,
            };

            var (products, total_count) = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel 
            { 
                SectionId = SectionId,
                BrandId = BrandId,
                Products = products
                .OrderBy(p => p.Order).FromDTO().ToView(),
                PageViewModel = new PageViewModel
                {
                    Page = Page,
                    PageSize = page_size ?? 0,
                    TotalItems = total_count
                },

            });
        }

        public IActionResult Details(int id)
        {
            var p = _ProductData.GetProductById(id);
            return View(p.FromDTO().ToView());
        }

        #region WebAPI

        public IActionResult GetFeaturesItems(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            return PartialView("Partial/_FeaturesItems",GetProducts(BrandId,SectionId,Page,PageSize));
        }

        private IEnumerable<ProductViewModel> GetProducts(int? BrandId, int? SectionId, int Page, int? PageSize)
        {
            return _ProductData.GetProducts(new ProductFilter { 
                        SectionId = SectionId,
                        BrandId = BrandId,
                        Page = Page,
                        PageSize = PageSize ?? (int.TryParse(_Configuration[__CatalogPageSize],out var size) ? size : null)
                    })
                    .Product.OrderBy(p => p.Order).FromDTO().ToView();
        }

        #endregion
    }
}
