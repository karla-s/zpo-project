using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace fitshop.Controllers
{
    [RoutePrefix("api/shop")]
    public class ShopController : ApiController
    {
        /// <summary>
        /// Place an order for a pet
        /// </summary>
        /// <remarks></remarks>
        /// <param name="body">order placed for purchasing the pet</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid Order</response>
        [HttpPost]
        [Route("order")]
        public virtual void AddOrder()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dodaj nowy produkt
        /// </summary>
        /// <remarks></remarks>
        /// <param name="body">Pet object that needs to be added to the store</param>
        /// <response code="405">Invalid input</response>
        [HttpPost]
        [Route("product")]
        public virtual void AddShopProduct()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete purchase order by ID
        /// </summary>
        /// <remarks>For valid response try integer IDs with positive integer value.         Negative or non-integer values will generate API errors</remarks>
        /// <param name="orderId">ID of the order that needs to be deleted</param>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Order not found</response>
        [HttpDelete]
        [Route("order/{orderId}")]
        public virtual void DeleteOrder(long? orderId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns pet inventories by status
        /// </summary>
        /// <remarks>Returns a map of status codes to quantities</remarks>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("")]
        public virtual void GetInventory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find purchase order by ID
        /// </summary>
        /// <remarks>For valid response try integer IDs with value &gt;&#x3D; 1 and &lt;&#x3D; 10.         Other values will generated exceptions</remarks>
        /// <param name="orderId">ID of pet that needs to be fetched</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Order not found</response>
        [HttpGet]
        [Route("order/{orderId}")]
        public virtual void GetOrderById(long? orderId)
        {
            throw new NotImplementedException();
        }
    }
}
