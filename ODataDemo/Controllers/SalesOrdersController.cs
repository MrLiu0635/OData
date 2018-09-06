using Inspur.Gsp.OData.Api;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using ODataDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ODataDemo.Controllers
{
    [GSPODataEntity(typeof(SalesOrder))]
    public class SalesOrdersController : GSPODataController
    {

        /// <summary>
        /// 订单缓存（mock数据）
        /// </summary>
        private static IList<SalesOrder> _salesOrders = Global.SALESORDER_IN_MEMORY;

        /// <summary>
        /// 根据实际情况拼接的routeName(和配置文件一致)，格式为api_[application]_[su]_[version]
        /// </summary>
        private const string routeName = "api_dev_main_v1.0";


        #region Part4:OrderItem是SalesOrder的包含实体,针对该包含实体的基本操作

        // GET ~/SalesOrders(100)/OrderItems  
        // 获取订单中的所有订单项，只能通过SalesOrders这一聚合根资源访问OrderItems ,OrderItems没有单独的Controller（独立存在没有意义）
        [HttpGet]
        [ODataRoute("SalesOrders({key})/OrderItems",RouteName =routeName)]
        [EnableQuery]
        public IQueryable<OrderItem> GetOrderItems([FromODataUri]string key)
        {
            var orderItems = _salesOrders.Single(a => a.Id == key).OrderItems.AsQueryable();
            return orderItems;
        }

        // GET ~/SalesOrders(100)/OrderItems('102')
        // 获取订单中的单个订单项。
        // ODataRouteURI段>=4段时，必须显式使用ODataRoute标签指定路由Template
        [HttpGet]
        [ODataRoute("SalesOrders({id})/OrderItems({orderItemId})", RouteName = routeName)]
        [EnableQuery]
        public SingleResult<OrderItem> GetOrderItems([FromODataUri]string id,[FromODataUri]string orderItemId)
        {
            var orderItem = _salesOrders.Single(a => a.Id == id).OrderItems.Where(x=>x.Id== orderItemId).AsQueryable();
            return SingleResult.Create(orderItem);
        }

        // POST ~/SalesOrders(100)/OrderItems
        // 新增一个订单项          
        [HttpPost]
        [ODataRoute("SalesOrders({id})/OrderItems", RouteName = routeName)]
        public IActionResult PostToOrderItem([FromODataUri]string id, [FromBody]OrderItem orderItem)
        {
            var salesOrder = _salesOrders.Single(a => a.Id == id);
            orderItem = new OrderItem()
            {
                Id = Guid.NewGuid().ToString(),
                Name = orderItem.Name
            };
            salesOrder.OrderItems.Add(orderItem);
            return Ok(orderItem);
        }

        // PUT ~/SalesOrders(100)/OrderItems(101)         
        // 全量更新订单中的订单项 
        [HttpPost]
        [ODataRoute("SalesOrders({id})/OrderItems({orderItemId})", RouteName = routeName)]
        public IActionResult PutToOrderItem([FromODataUri]string id, [FromODataUri]string orderItemId, [FromBody]OrderItem orderItem)
        {
            var salesOrder = _salesOrders.Single(a => a.Id == id);
            var originalPi = salesOrder.OrderItems.Single(p => p.Id == orderItemId);
            originalPi.Name = orderItem.Name;
            return Ok(orderItem);
        }

        // PATCH ~/SalesOrders('100')/OrderItems('101')
        // 增量更新订单中的订单项   
        [HttpPatch]
        [ODataRoute("SalesOrders({id})/OrderItems({orderItemId})", RouteName = routeName)]
        public IActionResult PatchOrderItem([FromODataUri]string id, [FromODataUri]string orderItemId, Delta<OrderItem> orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (orderItem.GetChangedPropertyNames().Contains("Id") && orderItem.TryGetPropertyValue("Id", out object id1) && (string)id1 != id)
            {
                return BadRequest("The key from the url must match the key of the entity in the body");
            }
            var originalOrder = _salesOrders.Where(x => x.Id == id).ToList();
            if (originalOrder == null || originalOrder.Count == 0)
            {
                return NotFound();
            }
            else
            {
                var items = originalOrder[0].OrderItems;
                var item = items.Where(x => x.Id == orderItemId).ToList();
                if (item == null || item.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    orderItem.Patch(item[0]);
                    return Updated(orderItem);
                }
            }
        }

        // DELETE ~/SalesOrders(100)/OrderItems(101) 
        // 删除订单项         
        [HttpDelete]
        [ODataRoute("SalesOrders({id})/OrderItems({orderItemId})", RouteName = routeName)]
        public IActionResult DeleteOrderItemFromOrder([FromODataUri]string id, [FromODataUri]string orderItemId)
        {
            var salesOrder = _salesOrders.Single(a => a.Id == id);
            var originalOrderItem = salesOrder.OrderItems.Single(p => p.Id == orderItemId);
            if (salesOrder.OrderItems.Remove(originalOrderItem))
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.NoContent));
            }
            else
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError));
            }
        }

        #endregion



       


   
    }
}