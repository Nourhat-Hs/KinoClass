using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoClass.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        //public int userId { get; set; }
        public int CartId { get; set; }
        public int MovieId { get; set; }
        public int mengde { get; set; }

    }
}
