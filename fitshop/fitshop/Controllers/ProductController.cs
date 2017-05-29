using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace fitshop.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        /// <summary>
        /// Dodaj nowy produkt
        /// </summary>
        /// <remarks></remarks>
        /// <param name="body">Pet object that needs to be added to the store</param>
        /// <response code="405">Invalid input</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("")]
        public IHttpActionResult AddProduct()
        {
            //var aa = new UserModel();
            //aa.UserName = "aaa";
            //aa.Password = "bbb";
            //var bb = new List<UserModel>() { aa, aa };

            return Ok();
        }


        /// <summary>
        /// Deletes products
        /// </summary>
        /// <remarks></remarks>
        /// <param name="productId">Product id to delete</param>
        /// <param name="apiKey"></param>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Pet not found</response>
        [HttpDelete]
        [Route("{productId}")]
        public virtual void DeleteProduct()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Finds Pets by tags
        /// </summary>
        /// <remarks>Muliple tags can be provided with comma separated strings. Use         tag1, tag2, tag3 for testing.</remarks>
        /// <param name="name">Tags to filter by</param>
        /// <param name="category">Tags to filter by</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid tag value</response>
        [HttpGet]
        [Route("search/{category}/{name}")]
        public virtual void FindByCategoryName()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Finds products by status
        /// </summary>
        /// <remarks>Multiple status values can be provided with comma separated strings</remarks>
        /// <param name="name">Status values that need to be considered for filter</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid status value</response>
        [HttpGet]
        [Route("search/{name}")]
        public virtual void FindProduct(string name)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Find pet by ID
        /// </summary>
        /// <remarks>Returns a single pet</remarks>
        /// <param name="productId">ID of pet to return</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Pet not found</response>
        [HttpGet]
        [Route("{productId}")]
        public virtual void GetProductById(long? productId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Updates a pet in the store with form data
        /// </summary>
        /// <remarks></remarks>
        /// <param name="productId">ID of pet that needs to be updated</param>
        /// <param name="name">Updated name of the pet</param>
        /// <param name="status">Updated status of the pet</param>
        /// <response code="405">Invalid input</response>
        [HttpPost]
        [Route("{productId}")]
        public virtual void UpdatePetWithForm(long? productId, string name, string status)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Update an existing produkt
        /// </summary>
        /// <remarks></remarks>
        /// <param name="body">Product that needs to be added to the store</param>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Pet not found</response>
        /// <response code="405">Validation exception</response>
        [HttpPut]
        [Route("{productId}")]

        public virtual void UpdateProduct()
        {
            throw new NotImplementedException();
        }
    }
}
