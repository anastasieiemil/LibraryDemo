using Library.Core.Abstractions.Repositories;
using Library.Core.Domain;
using Library.DAL.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Concret
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public override Book? Update(Book book)
        {
            var currentBook = Get(book.Key);
            if (currentBook != null)
            {
                var data = entities[book.Key];

                currentBook.Name = book.Name;
                currentBook.Price = book.Price;
                currentBook.Quantity = book.Quantity;
                currentBook.CurrentQuantity = book.CurrentQuantity;

                entities.TryUpdate(currentBook.Key, JsonConvert.SerializeObject(currentBook), data);
                return book;
            }
            else
            {
                return null;
            }
        }

    }
}
