﻿using System.Collections.Generic;

namespace WebStore.Domain.DTO
{
    public record PageProductsDTO(IEnumerable<ProductDTO> Product, int TotalCount);
}
