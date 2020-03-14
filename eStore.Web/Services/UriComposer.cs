using eStore.Application.Interfaces;
using eStore.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eStore.Web.Services
{
    public class UriComposer : IUriComposer
    {
        private readonly CatalogSettings _catalogSettings;

        public UriComposer(CatalogSettings catalogSettings) => _catalogSettings = catalogSettings;

        public string ComposePicUri(string uriTemplate)
        {
            return uriTemplate.Replace("http://catalogbaseurltobereplaced", _catalogSettings.CatalogBaseUrl);
        }
    }
}
