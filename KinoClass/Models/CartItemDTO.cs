using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoClass.Models
{
    //cartItem //cart
   public class CartItemDTO
    {
        public int Id { get; set; }
        public int CartId { get; set; }

        public int MovieId { get; set; } //movie Id
        public string MovieName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int Mengde { get; set; }

       
    }
}

