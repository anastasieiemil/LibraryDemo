using Library.Core.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Concret
{
    public class RepositoryProvider : IRepositoryProvider
    {
        public IBookRepository Books
        {
            get
            {
                if (books == null)
                {
                    books = new BookRepository();
                }
                return books;
            }
        }

        public ILendedBookRepository BookLends
        {
            get
            {
                if (bookLends == null)
                {
                    bookLends = new LendedBookRepository();
                }
                return bookLends;
            }
        }

        #region private
        private IBookRepository books = null!;
        private ILendedBookRepository bookLends = null!;

        #endregion
    }
}
