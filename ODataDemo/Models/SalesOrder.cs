
using Inspur.Ecp.Caf.EntityFramework.Api;
using System.Collections.Generic;

namespace ODataDemo.Models
{
    public class SalesOrder:ICafEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }


    public class OrderItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}