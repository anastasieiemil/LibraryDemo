using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Lend
    {
        [StringLength(maximumLength: 100, MinimumLength = 3)]
        public string PersonName { get; set; } = null!;

        [StringLength(maximumLength: 20, MinimumLength = 10)]
        public string PersonCode { get; set; } = null!;

        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string ISBN { get; set; } = null!;
    }
}
