using KinoClass.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIkino.Repositories
{
    public interface IShoping
    {
        
      
        Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItem> DeleteItem(int Id);
        Task<CartItem> GetItem(int id);
        Task<IEnumerable<CartItem>> CartItems(int UserId);
        Task<CartItem> UpdateItem(int Id, CartItemMengdeUpdate cartItemMengdeUpdate);
        
    }
}
