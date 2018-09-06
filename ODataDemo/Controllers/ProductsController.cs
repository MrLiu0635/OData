using Inspur.Ecp.Caf.EntityFramework.Api;
using Inspur.Gsp.OData.Api;
using Inspur.Gsp.OData.EF.Util;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using ODataDemo.Models;
using ODataDemo.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ODataDemo.Controllers
{
    [GSPODataEntity(typeof(Product))]
    public class ProductsController : GSPODataController
    {
        /// <summary>
        /// 根据实际情况拼接的routeName(和配置文件一致)，格式为api_[application]_[su]_[version]
        /// </summary>
        private const string routeName = "api_dev_main_v1.0";


        #region Part1:基本CRUD

        // GET ~/Products?$filter=Id eq "123"&$orderby=Id
        // 获取所有产品（通用查询方法）
        [HttpGet]
        [ODataRoute("Products",RouteName = routeName)]
        [EnableQuery]
        public IQueryable<Product> Get(ODataQueryOptions<Product> opts)
        {
            //调用转化工具，将查询选项转为查询规约
            Specification<Product> spe = SpecificationBuilder.Build(opts, out bool needTotalCount);
            //调用Service->Repo，使用规约查询结果
            List<Product> result = ProductService.Instance.GetList(spe);
            //如果请求要求返回总行数，进行如下设置
            if (needTotalCount)
            {
                long count = spe.TotalCount;
                SetCount(opts, count);
            }
            //将List<Product>转化为IQuerable再返回
            return result.AsQueryable();
        }

        // GET ~/Products('123')
        // 获取单个产品
        [HttpGet]
        [ODataRoute("Products({key})",RouteName =routeName)]
        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri]string key,ODataQueryOptions<Product> opts)
        {
            var result=Global.PRODUCT_IN_MEMORY.Where(x=>x.Id== key).AsQueryable();
            return SingleResult.Create(result);
        }

        
        // POST ~/Products
        // 新增一个产品
        [HttpPost]
        [ODataRoute("Products",RouteName =routeName)]
        public IActionResult Post([FromBody] Product product)
        {
            Global.PRODUCT_IN_MEMORY.Add(product);
            return Created(product);
        }

        // PUT ~/Products('1')
        // request body (全量数据):
        // {
        //  "Id":"123", "Name":"123","Price":100,"Category":"Clothing","SupplierId":"321","Supplier":{"Id":"123"}
        // }
        // 全量更新一个产品
        [HttpPut]
        [ODataRoute("Products({key})",RouteName =routeName)]
        public IActionResult Put([FromODataUri] string key ,[FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = Global.PRODUCT_IN_MEMORY.Where(x => x.Id == key).ToList();
            if (key != product.Id||result.Count==0)
            {
                return BadRequest();
            }
            //TODO:数据库执行全量更新，先删后增
            return Updated(product);
        }

        // PATCH ~/Products('123')
        // request body (增量数据):
        // {
        //  "Id":"123","Price":150
        // }
        // 增量更新一个产品
        [HttpPatch]
        [ODataRoute("Products({key})",RouteName =routeName)]
        public  IActionResult Patch([FromODataUri] string key, Delta<Product> product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = Global.PRODUCT_IN_MEMORY.Where(x => x.Id == key).ToList();
            if (result.Count==0)
            {
                return NotFound();
            }
            //Use Patch() Function to Apply the Dleta<Product> To origin Product
            product.Patch(result[0]);
            return Updated(result[0]);
        }

        // DELETE ~/Products('1')
        // 删除单个的产品
        [HttpDelete]
        [ODataRoute("Products({key})",RouteName =routeName)]
        public  IActionResult Delete([FromODataUri] string key)
        {
            var product = Global.PRODUCT_IN_MEMORY.Where(x => x.Id == key).ToList();
            if (product.Count == 0)
            {
                return NotFound();
            }
            Global.PRODUCT_IN_MEMORY.Remove(product[0]);
            return StatusCode(Convert.ToInt32(HttpStatusCode.NoContent));
        }

        #endregion

        #region Part2:不符合CRUD的操作，自定义操作（Action和Function）

        // POST ~/Products/RateService
        // 绑定在Product实体上的一个自定义Action对一个产品进行评级，自定义Action的默认HttpMethod为POST，也可使用[HttpMethod]标签改为Patch，Put等。
        // request body(键值对格式，键为形参名称，值为参数类型对应的Json结构，在本例中 ，body写作如下方式)
        // {"rating":5}
        [HttpPost]
        [ODataRoute("Products/RateService", RouteName = routeName)]
        [GSPODataAction(typeof(int))]
        public IActionResult RateService(ODataActionParameters rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //通过这种方式获取ODataActionParameters中的参数（int rating）
            int param = (int)rating["rating"];
           //TODO:Save To DB
            return StatusCode(Convert.ToInt32(HttpStatusCode.NoContent));
        }

        // GET ~/Products/MostExpensive
        // 绑定在Product实体上的一个自定义Function,获取最昂贵的产品，自定义Function的默认HttpMethod为GET。不允许改为其他HttpMethod
        [HttpGet]
        [ODataRoute("Products/MostExpensive(filter={filter})", RouteName = routeName)]
        [GSPODataFunction]
        public int MostExpensive([FromODataUri] string filter)
        {
            var product = Global.PRODUCT_IN_MEMORY.Max(x => x.Price);
            return 100;
        }


        // GET ~/SalesTaxRate(250001)
        // 未绑定实体的一个自定义Function，根据邮编获取一个地区的税率，Template中不需要包含实体，直接写Function的名称和参数部分即可。此方法可以位于任一Controller中
        [HttpGet]
        [ODataRoute("SalesTaxRate(postalCode={postalCode})", RouteName = routeName)]
        [GSPODataFunction(unBound:true)]
        public double SalesTaxRate([FromODataUri] int postalCode)
        {
            double rate = 5.6;  // Use a fake number for the sample.
            return rate;
        }

        #endregion

        #region Part3:变更集操作，自定义Patch

        // Patch ~/Products/PatchService
        //自定义Patch，自定义变更集结构，前端传输ChangeSet，后端可以自行解析结构并自定义处理逻辑
       [HttpPatch]
       [ODataRoute("Products/PatchService", RouteName = routeName)]
       [GSPODataAction(paramType:typeof(ChangeSet))]
        public IActionResult PatchService(ODataActionParameters ChangeSet)
        {
            var changeSet = ChangeSet["ChangeSet"];
            //根据自定义的ChangeSet结构，进行一系列操作
            return Ok(changeSet);

        }
        #endregion








    }
}
