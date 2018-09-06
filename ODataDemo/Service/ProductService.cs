using Inspur.Ecp.Caf.EntityFramework.Api;
using Inspur.Ecp.Caf.ServiceMgr;
using ODataDemo.Models;
using ODataDemo.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ODataDemo.Service
{
    public class ProductService
    {

        private IUnitOfWorkManager unitOfWorkManager;
        private IUnitOfWorkManager UnitOfWorkManager
        {
            get
            {
                if (this.unitOfWorkManager == null)
                {
                    unitOfWorkManager = ServiceManager.GetService<IUnitOfWorkManager>();
                }
                return unitOfWorkManager;
            }
        }

        public static ProductService Instance = new ProductService();

        private object syncObject = new object();
        private IProductRepository _ProductRepo;
        private IProductRepository ProductRepo
        {
            get
            {
                if (_ProductRepo == null)
                {
                    lock (syncObject)
                    {
                        if (_ProductRepo == null)
                        {
                            _ProductRepo = RepositoryFactory.Create<IProductRepository, ProductRepository>();
                        }
                    }
                }
                return _ProductRepo;
            }
        }



        /// <summary>
        /// 使用规约查询获取实体列表
        /// </summary>
        /// <param name="spe"></param>
        /// <returns></returns>
        internal List<Product> GetList(Specification<Product> spe)
        {
            //使用规约，调用查询
            List<Product> result = ProductRepo.QuerySatisfyingItems(spe).ToList();
            return result;
        }

 
    }

   



}
