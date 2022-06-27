using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Domain
{
    public class LendedBook : IModel
    {
        public int ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime ReturnedTimeStamp { get; set; }
        public bool IsReturned { get; set; }
        public Book Book { get; set; } = null!;
        public string PersonName { get; set; } = null!;
        public string PersonCode { get; set; } = null!;
        public decimal Price { get; set; }

        public string Key => $"{Book.ISBN}_{PersonCode}_{TimeStamp.Ticks}";
    }
}
