using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Interfaces
{
    public interface IUriComposer
    {
        string ComposePicUri(string uriTemplate);
    }
}
