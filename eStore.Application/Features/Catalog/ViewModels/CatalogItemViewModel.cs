using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Features.Catalog
{
    public class CatalogItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUri { get; set; }
        public decimal Price { get; set; }
    }
}
