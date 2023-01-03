using KinoClass.Models;
using NPOI.SS.Formula.Functions;

namespace APIkino.Extantions
{
    public static class Convert
    {
        // here we join collection Product catagories
        // with collection product  to return collection ProductDTO
      
        
        public static IEnumerable<CartItemDTO> ConvertToDto(this IEnumerable<CartItem> cartItems,
                                                      IEnumerable<Movies> movies)
        {
            return (from cartItem in cartItems
                    join movie in movies
                    on cartItem.MovieId equals movie?.Id
                    select new CartItemDTO
                    {
                        Id = cartItem.Id,
                        MovieId = cartItem.MovieId,
                        MovieName = movie.MovieName,
                        Description = movie.description,
                        Price = movie.price,
                        CartId = cartItem.CartId,
                        Mengde=cartItem.mengde,
                        TotalPrice= movie.price * cartItem.mengde
                    }).ToList();
        }
        public static CartItemDTO convertionDTO(this CartItem cartItem,
                                                   Movies movie)
        {
            return ( new CartItemDTO
                    {
                        Id = cartItem.Id,
                        CartId = cartItem.CartId,
                        MovieId = cartItem.MovieId,
                        MovieName = movie.MovieName,
                        Description = movie.description,
                        Price = movie.price,
                        Mengde = cartItem.mengde,
                        TotalPrice = movie.price * cartItem.mengde

                    });
        }
    }
}
