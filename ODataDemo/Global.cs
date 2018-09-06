using ODataDemo.Models;
using System.Collections.Generic;

namespace ODataDemo
{
    /// <summary>
    /// 存放一些全局mock数据
    /// </summary>
    public class Global
    {
        //服务基路径
        private static readonly string serviceUrl = "http://localhost:16003/odata";
        //服务对应的批量请求URl（所有批量请求都通过此URl发送请求）
        private static readonly string batchRequestUrl = string.Format("{0}/$batch", serviceUrl);
        //SalesOrders服务（某个服务样例）的绝对路径（包含基路径），请求体中也可以直接使用相对路径（不含基路径）
        private static readonly string absoluteSalesOrderUri = string.Format("{0}/SalesOrders", serviceUrl);

        /// <summary>
        /// Batch请求请求体样例
        /// </summary>
        public string BATCH_SALESORDERS = @"
                        {
                           ""requests"": [{
                                  ""id"": ""-1"",
                                  ""method"": ""GET"",
                                  ""url"": """ + absoluteSalesOrderUri + @""",
                                  ""headers"": {
                                      ""OData-Version"": ""4.0"",
                                      ""Content-Type"": ""application/json;odata.metadata=minimal"",
                                      ""Accept"": ""application/json;odata.metadata=minimal""
                                  }
                              }, {
                                  ""id"": ""1"",
                                  ""atomicityGroup"": ""f7de7314-2f3d-4422-b840-ada6d6de0f18"",
                                  ""method"": ""POST"",
                                  ""url"": """ + absoluteSalesOrderUri + @""",
                                  ""headers"": {
                                      ""OData-Version"": ""4.0"",
                                      ""Content-Type"": ""application/json;odata.metadata=minimal"",
                                      ""Accept"": ""application/json;odata.metadata=minimal""
                                  },
                                  ""body"": {
                                      ""Id"":1001,
                                      ""Name"":""CreatedByJsonBatch_11""
                                  }
                              }, {
                                  ""id"": ""2"",
                                  ""atomicityGroup"": ""f7de7314-2f3d-4422-b840-ada6d6de0f18"",
                                  ""method"": ""POST"",
                                  ""url"": """ + absoluteSalesOrderUri + @""",
                                  ""headers"": {
                                      ""OData-Version"": ""4.0"",
                                      ""Content-Type"": ""application/json;odata.metadata=minimal"",
                                      ""Accept"": ""application/json;odata.metadata=minimal""
                                  },
                                  ""body"": {
                                      ""Id"":1002,
                                      ""Name"":""CreatedByJsonBatch_12""
                                  }
                              }, {
                                  ""id"": ""3"",
                                  ""method"": ""POST"",
                                  ""url"": """ + absoluteSalesOrderUri + @""",
                                  ""headers"": {
                                      ""OData-Version"": ""4.0"",
                                      ""Content-Type"": ""application/json;odata.metadata=minimal"",
                                      ""Accept"": ""application/json;odata.metadata=minimal""
                                  },
                                  ""body"": {
                                      ""Id"": 1003,
                                      ""Name"":""CreatedByJsonBatch_3""
                                  }
                              }
                          ]
                        }";


        /// <summary>
        /// Product缓存
        /// </summary>
        public static List<Product> PRODUCT_IN_MEMORY = new List<Product>()
        {
            new Product()
            {
                Id = "1",
                Name="product1",
                Price=100,
                Supplier=new Supplier()
                {
                    Id="01",
                    Name="supplier1"
                },
                Category="Clothing",
                SupplierId="01"

            },
            new Product()
            {
                Id="2",
                Name="product2",
                Price=200,
                Category="Clothing"

            },
            new Product()
            {
                Id="3",
                Name="product3",
                Price=4000,
                Category="Vehicle"
            },
                 new Product()
            {
                Id="4",
                Name="product4",
                Price=1200,
                Category="Vehicle"
            }

        };

        /// <summary>
        /// Supplier缓存
        /// </summary>
        public static List<Supplier> SUPPLIER_IN_MEMORY = new List<Supplier>()
        {
            new Supplier()
            {
                Id = "01",
                Name="Supplier1",
                Products=new List<Product>()
                {
                    new Product()
                    {
                        Id="1",
                        Name="product1",
                        Price=100,
                        Supplier=new Supplier()
                        {
                            Id="01",
                            Name="supplier1"
                        },
                        SupplierId="01"
                    }
                }

            },
            new Supplier()
            {
                Id="02",
                Name="Supplier2"
            }
        };


        /// <summary>
        /// SalesOrder缓存
        /// </summary>
        public static List<SalesOrder> SALESORDER_IN_MEMORY = new List<SalesOrder>()
        {
            new SalesOrder()
            {
                Id = "100",
                Name = "Name100",
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem()
                    {
                        Id = "103",
                        Name = "101 first OI",
                    },
                    new OrderItem()
                    {
                        Id = "102",
                        Name = "102 second OI",
                    },
                },
            },
        };
    }

}
