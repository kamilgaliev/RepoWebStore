using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapper
    {
        public static SectionDTO ToDTO(this Section Section)
        {
            return Section is null
                ? null
                : new SectionDTO
                {
                    Id = Section.Id,
                    Name = Section.Name,
                    Order = Section.Order,
                    ParentId = Section.ParenId,
                };
        }

        public static Section FromDTO(this SectionDTO SectionDTO)
        {
            return SectionDTO is null
                ? null
                : new Section
                {
                    Id = SectionDTO.Id,
                    Name = SectionDTO.Name,
                    Order = SectionDTO.Order,
                    ParenId = SectionDTO.ParentId,
                };
        }
        public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Section> Sections) => Sections.Select(ToDTO);

        public static IEnumerable<Section> FromDTO(this IEnumerable<SectionDTO> Sections) => Sections.Select(FromDTO);
    }
}
