using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Domain.Settings
{
    public class CatalogSettings
    {
        public string CatalogBaseUrl { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
