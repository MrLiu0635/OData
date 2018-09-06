using Inspur.Ecp.Caf.EntityFramework.Api;

namespace ODataDemo.Models
{
    public class Product:ICafEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string SupplierId { get; set; }
        public  Supplier Supplier { get; set; }
    }
    public class ChangeSet
    {
        ///结构可以自定义
    }

  
}
