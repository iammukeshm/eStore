using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Domain.Entities
{
    public class BaseEntity
    {
        public virtual int Id { get; protected set; }
    }
}
