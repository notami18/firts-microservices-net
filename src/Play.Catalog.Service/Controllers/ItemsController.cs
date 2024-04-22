using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
  // https://localhost:5001/items
  [ApiController]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    private static readonly List<ItemDto> items = new()
    {
      new ItemDto(Guid.NewGuid(), "Potion", "Restore a small amount of HP", 5, DateTimeOffset.UtcNow),
      new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
      new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
    };

    [HttpGet]   // GET /items
    public IEnumerable<ItemDto> Get() => items;

    [HttpGet("{id}")]   // GET /items/{id}
    public ActionResult<ItemDto> GetById(Guid id)
    {
      var item = items.Where(item => item.Id == id).SingleOrDefault();
      if (item == null)
      {
        return NotFound();
      }
      return item;
    }

    [HttpPost]   // POST /items
    public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
    {
      var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
      items.Add(item);
      return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]   // PUT /items/{id}
    public ActionResult Put(Guid id, UpdateItemDto updateItemDto)
    {
      var existingItem = items.FirstOrDefault(item => item.Id == id);
      if (existingItem == null)
      {
        return NotFound();
      }
      var updatedItem = existingItem with
      {
        Name = updateItemDto.Name,
        Description = updateItemDto.Description,
        Price = updateItemDto.Price
      };
      var index = items.FindIndex(existingItem => existingItem.Id == id);
      items[index] = updatedItem;
      return NoContent();
    }

    [HttpDelete("{id}")]   // DELETE /items/{id}
    public IActionResult Delete(Guid id)
    {
      var index = items.FindIndex(existingItem => existingItem.Id == id);
      if (index == -1)
      {
        return NotFound();
      }
      items.RemoveAt(index);
      return NoContent();
    }
  }
}