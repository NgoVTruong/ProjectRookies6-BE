using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Category
    {
        public Guid CategoryId {get; set;}
        public string CategoryCode {get; set;}
        public string CategoryName {get; set;}

        public virtual ICollection<Asset>? Assets {get; set;}
    }
}