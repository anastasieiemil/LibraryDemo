using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Book
    {
        [StringLength(maximumLength: 100, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string ISBN { get; set; } = null!;

        [Range(1, maximum: int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, maximum: int.MaxValue)]
        public decimal Price { get; set; }
    }
}
