using Inspur.Ecp.Caf.EntityFramework.Api;
using Microsoft.AspNet.OData.Query;
using ODataDemo.Models;
using ODataDemo.Repository.DbContext;
using System.Collections.Generic;
using System.Linq;

namespace ODataDemo.Repository
{
    public class ProductRepository: RepositoryBase<Product>, IProductRepository
    {
        protected override EntityTypeInfo EntityTypeInfo => new EntityTypeInfo(typeof(ODataDbContext));

        public  IQueryable Get(ODataQueryOptions<Product> pdt)
        {
            return new List<Product>().AsQueryable();
        }
     
    }
}
