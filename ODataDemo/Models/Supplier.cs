using Inspur.Ecp.Caf.EntityFramework.Api;
using System.Collections.Generic;

namespace ODataDemo.Models
{
    public class Supplier:ICafEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public  IList<Product> Products { get; set; }
    }
}
