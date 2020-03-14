using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.DTOs
{
    public class Envelope<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
