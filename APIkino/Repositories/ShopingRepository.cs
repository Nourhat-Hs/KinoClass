using APIkino.Data;
using KinoClass.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static com.sun.tools.@internal.xjc.reader.xmlschema.bindinfo.BIConversion;

namespace APIkino.Repositories
{
    public class ShopingRepository : IShoping
    {
        private readonly Context context;
       
        public ShopingRepository(Context context)
        {
            this.context = context;
           


        }
        //to avoid that an item is added more than once

        private async Task<bool> cartItemExsist(int CartId, int movieId)
        {
            return await this.context.CartItem.AnyAsync(c => c.CartId == CartId &&
                                                 c.MovieId == movieId);


        }

      

        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await cartItemExsist(cartItemToAddDto.CartId, cartItemToAddDto.MovieId) == false)
            {

                // check if the movie exists with link quary
                var Item = await (from movie in this.context.movies
                                  where cartItemToAddDto.MovieId == movie.Id
                                  
                                  select new CartItem
                                  {
                                      
                                      CartId = cartItemToAddDto.CartId,
                                      MovieId = movie.Id,
                                      mengde = cartItemToAddDto.mengde,

                                  }).SingleOrDefaultAsync();
                if (Item != null)
                {
                    //adder to the database
                    var result = await this.context.CartItem.AddAsync(Item);
                   

                    await this.context.SaveChangesAsync();


                    //here we return to the user the entity that has
                    //been added to the cartItem database 
                    return result.Entity;
                }
               
            }
            return null;

        }
        
        public async Task<IEnumerable<CartItem>> CartItems(int UserId)
        {
            return await (from cart in this.context.Cart
                          join cartItem in this.context.CartItem
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == UserId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              MovieId = cartItem.MovieId,
                              mengde = cartItem.mengde,
                              CartId = cartItem.CartId,
                          }).ToListAsync();



        }


        public async Task<CartItem> DeleteItem(int Id)
        {
            var item = await this.context.CartItem.FindAsync(Id);
            if (item != null)
            {
                this.context.CartItem.Remove(item);
                await this.context.SaveChangesAsync();
            }
            return item;
        }


        //getting a movie from the cart
        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in this.context.Cart
                          join cartItem in this.context.CartItem
                          on cart.Id equals cartItem.CartId
                         where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              MovieId = cartItem.MovieId,
                              mengde = cartItem.mengde,
                              CartId = cartItem.CartId
                          }).SingleOrDefaultAsync();
        }


     


      
      

        public async Task<CartItem> UpdateItem(int Id, CartItemMengdeUpdate cartItemMengdeUpdate)
        {
            var item = await this.context.CartItem.FindAsync(Id);
            if (item != null)
            {
                item.mengde = cartItemMengdeUpdate.mengde;
                await this.context.SaveChangesAsync();
            }
            return null;
        }

       

    }
   

}
