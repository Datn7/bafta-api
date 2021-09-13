using bafta_api.Data;
using bafta_api.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bafta_api.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext storeContext;

        public BuggyController(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var item = storeContext.Products.Find(55);

            if(item == null)
            {
                return NotFound(new ApiResponse(400));
            }

            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var item = storeContext.Products.Find(55);
            var itemToReturn = item.ToString();

            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }
    }
}
