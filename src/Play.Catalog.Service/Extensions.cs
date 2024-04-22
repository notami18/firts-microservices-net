using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service
{
  public static class Extensions
  {

    public static ItemDto AsDto(this Item item)
    {
      return new ItemDto(item.Id, item.Name!, item.Description!, item.Price, item.CreatedDate);
    }
    // public static void ThrowIfNull(this object obj, string name)
    // {
    //   if (obj == null)
    //   {
    //     throw new ArgumentNullException(name);
    //   }
    // }
  }
}