using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
  // https://localhost:5001/items
  [ApiController]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    private readonly ItemsRepository itemsRepository = new();
    private static readonly List<ItemDto> items = new()
    {
      new ItemDto(Guid.NewGuid(), "Potion", "Restore a small amount of HP", 5, DateTimeOffset.UtcNow),
      new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
      new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
    };

    [HttpGet]   // GET /items
    public async Task<IEnumerable<ItemDto>> GetAsync()
    {
      var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
      return items;
    }

    [HttpGet("{id}")]   // GET /items/{id}
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
      var item = await itemsRepository.GetAsync(id);
      if (item == null)
      {
        return NotFound();
      }
      return item.AsDto();
    }

    [HttpPost]   // POST /items
    public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
    {
      var item = new Item
      {
        Id = Guid.NewGuid(),
        Name = createItemDto.Name,
        Description = createItemDto.Description,
        Price = createItemDto.Price,
        CreatedDate = DateTimeOffset.UtcNow
      };

      await itemsRepository.CreateAsync(item);
      return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item.AsDto());
    }

    [HttpPut("{id}")]   // PUT /items/{id}
    public async Task<ActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
    {
      var existingItem = await itemsRepository.GetByIdAsync(id);
      if (existingItem == null)
      {
        return NotFound();
      }

      existingItem.Name = updateItemDto.Name;
      existingItem.Description = updateItemDto.Description;
      existingItem.Price = updateItemDto.Price;

      await itemsRepository.UpdateAsync(existingItem);

      return NoContent();
    }

    [HttpDelete("{id}")]   // DELETE /items/{id}
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
      var item = await itemsRepository.GetByIdAsync(id);
      if (item == null)
      {
        return NotFound();
      }
      await itemsRepository.RemoveAsync(id);
      return NoContent();
    }
  }
}