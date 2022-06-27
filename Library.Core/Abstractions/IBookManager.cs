using Library.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Abstractions
{
    /// <summary>
    /// Holds logic for managing books.
    /// </summary>
    public interface IBookManager
    {
        bool Add(Book book);
        bool Lend(string ISBN, string personName, string personCode);
        bool Return(LendedBook lend);

        decimal GetPrice(LendedBook lend);

        Book? Search(string isbn);

        List<Book> GetAll();
    }
}
