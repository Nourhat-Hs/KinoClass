using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoClass.Models
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string? MovieName { get; set; }
        public string? description { get; set; }
        public decimal price { get; set; }
        public int mengde { get; set; }
    }
}
