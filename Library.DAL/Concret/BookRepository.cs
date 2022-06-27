using Library.Core.Abstractions.Repositories;
using Library.Core.Domain;
using Library.DAL.Abstractions;
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
            if(entities.ContainsKey(book.ISBN))
            {
                var currentBook = entities[book.ISBN];

                currentBook.Name = book.Name;
                currentBook.Price = book.Price;
                currentBook.Quantity = book.Quantity;
                currentBook.CurrentQuantity = book.CurrentQuantity;

                entities.TryUpdate(currentBook.Key, currentBook, entities[book.ISBN]);
                return book;
            }
            else
            {
                return null;
            }
        }

    }
}
