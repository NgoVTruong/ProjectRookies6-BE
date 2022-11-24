using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; } //false

        public virtual ICollection<Asset>? Assets { get; set; }
    }
}