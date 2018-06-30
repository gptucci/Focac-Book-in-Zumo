using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Focac_BookService.DataObjects;
using Focac_BookService.Models;

namespace Focac_BookService.Controllers
{
    public class FocaccePostController : TableController<FocaccePost>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            Focac_BookContext context = new Focac_BookContext();
            DomainManager = new EntityDomainManager<FocaccePost>(context, Request);
        }

        // GET tables/TodoItem
        public IQueryable<FocaccePost> GetAllFocaccePost()
        {

            return Query();
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<FocaccePost> GetFocaccePost(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<FocaccePost> PatchFocaccePost(string id, Delta<FocaccePost> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostFocaccePost(FocaccePost item)
        {
            FocaccePost current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteFocaccePost(string id)
        {
            return DeleteAsync(id);
        }
    }
}