using Inspur.Ecp.Caf.EntityFramework.Api;
using Microsoft.AspNet.OData.Query;
using ODataDemo.Models;
using System.Linq;

namespace ODataDemo.Repository
{
    public interface IProductRepository: IRepository<Product>
    {
        [UnitOfWork(false)]
        IQueryable Get(ODataQueryOptions<Product> pdt);

    }
}
