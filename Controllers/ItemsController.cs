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
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IDocumentDBRepository<Item> Respository;
        public ItemsController(IDocumentDBRepository<Item> Respository)
        {
            this.Respository = Respository;
        }

        // GET: api/Itemsx
        [EnableQuery()]
        [HttpGet]
        public async Task<IEnumerable<Item>> Get()
        {
            return await Respository.GetItemsAsync(d => !d.Completed);
        }

        //GET: api/Items/5  
        [EnableQuery()]
        [HttpGet("{key}")]
        public async Task<Item> Get(string key)
        {
            return await Respository.GetItemAsync(key);
        }

        //Put api/Items/5
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            await Respository.CreateItemAsync(item);

            return Created("items", item);
        }

        //Put api/Items/5
        [EnableQuery()]
        [HttpPost("/odata/items('{key}')")]
        public async Task<IActionResult> Post(string key, [FromBody] Item item)
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

            return Created("items", item);
        }
    }
}