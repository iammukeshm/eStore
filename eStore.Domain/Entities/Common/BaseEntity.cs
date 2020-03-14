using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Domain.Entities
{
    public class BaseEntity
    {
        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
