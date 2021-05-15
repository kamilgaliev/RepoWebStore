using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData ProductData) => _ProductData = ProductData;
        public IViewComponentResult Invoke()
        {
            var sections = _ProductData.GetSections();

            var parent_sections = sections.Where(s => s.ParentId == null);

            var parent_sections_views = parent_sections
                .Select(s => new SectionViewModel 
                { 
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,
                    ProductsCount = _ProductData.GetProducts(new Domain.ProductFilter { SectionId = s.Id }).Count()
                }).ToList();

            int OrderSortMethod(SectionViewModel a,SectionViewModel b) => Comparer<int>.Default.Compare(a.Order,b.Order);

            foreach (var parent_section in parent_sections_views)
            {
                var childs = sections.Where(s => s.ParentId == parent_section.Id);

                foreach (var child_sections in childs)
                {
                    parent_section.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_sections.Id,
                        Name = child_sections.Name,
                        Order = child_sections.Order,
                        Parent = parent_section,
                        ProductsCount = 1
                    });
                }

                parent_section.ChildSections.Sort(OrderSortMethod);
            }

            parent_sections_views.Sort(OrderSortMethod);

            return View(parent_sections_views);
        }
    }
}
