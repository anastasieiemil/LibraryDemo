using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Domain
{
    public class Book : IModel
    {
        public string? Name { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int Quantity { get; set; }
        public int CurrentQuantity { get; set; }
        public decimal Price { get; set; }
        public string Key { get => ISBN; }

        public override string ToString()
        {
            var type = typeof(Book);
            StringBuilder stringBuilder = new StringBuilder();
            var properties = type.GetProperties();

            foreach(var property in properties)
            {
                stringBuilder.Append($"{property.Name}:{property.GetValue(this)};");
            }

            return stringBuilder.ToString();
        }
    }
}
