using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Mvc;
using todo;
using todo.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;

namespace todo.Controllers
{
    public class ItemsController : ODataController
    {
        private readonly IDocumentDBRepository<Item> Respository;
        public ItemsController(IDocumentDBRepository<Item> Respository)
        {
            this.Respository = Respository;
        }

        // GET: api/Itemsx
        [EnableQuery()]
        public async Task<IEnumerable<Item>> Get()
        {
            return await Respository.GetItemsAsync(d => !d.Completed);
        }

        //GET: api/Items/5  
        [EnableQuery()]
        public async Task<Item> Get([FromODataUri] string key)
        {
            return await Respository.GetItemAsync(key);
        }

        //Put api/Items/5
        [EnableQuery()]
        public async Task<IActionResult> Put([FromODataUri] string key, [FromBody] Item item)
        {
            if (item == null || item.Id != key)
            {
                return BadRequest();
            }

            var todo = await Respository.GetItemAsync(key);
            if (todo == null)
            {
                return NotFound();
            }

            await Respository.UpdateItemAsync(key, item);

            return new NoContentResult();
        }
    }

}   