using Library.Core.Abstractions;
using Library.Core.Abstractions.Repositories;
using Library.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Concret
{
    public class BookManager : IBookManager
    {
        private readonly IRepositoryProvider repositories;

        private static readonly int MAX_RENT_PERIOD = 14;       // Days.
        private static readonly decimal EXTRA_PRICE = 0.01M;

        public BookManager()
        {
            repositories = DependencyInjector.Get<IRepositoryProvider>();
        }

        public BookManager(IRepositoryProvider repo)
        {
            repositories = repo;
        }

        public bool Add(Book book)
        {
            // Add book.
            var savedBook = repositories.Books.Add(book);

            return savedBook != null;
        }

        public bool Lend(string ISBN, string personName, string personCode)
        {
            try
            {
                // Lock dal and try to create and save a lendedBook.
                if (ResourceManagerLocker.LockResource(repositories.Books, ISBN))
                {
                    var book = repositories.Books.Get(ISBN);

                    // Check if the book is available.
                    if (book == null || book.CurrentQuantity == 0)
                    {
                        return false;
                    }

                    // Create lend.
                    var lend = new LendedBook
                    {
                        PersonCode = personCode,
                        PersonName = personName,
                        Book = book,
                        TimeStamp = DateTime.Now,
                        Price = book.Price,
                        ID = GetIndex()
                    };

                    // Add Lend;
                    var savedLend = repositories.BookLends.Add(lend);
                    if (savedLend == null)
                    {
                        return false;
                    }

                    // Update the book.
                    --book.CurrentQuantity;
                    repositories.Books.Update(book);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { }
            finally
            {
                ResourceManagerLocker.ReleaseResource(repositories.Books, ISBN);
            }

            return false;
        }

        public bool Return(LendedBook lend)
        {
            if (lend == null || lend.IsReturned)
            {
                return false;
            }

            try
            {
                // Lock dal and try to create and save a lendedBook.
                if (ResourceManagerLocker.LockResource(repositories.BookLends, lend.Key))
                {
                    // Update Lend.
                    lend.IsReturned = true;
                    lend.ReturnedTimeStamp = DateTime.Now;
                    repositories.BookLends.Update(lend);

                    // Update the book.
                    var book = lend.Book;
                    ++book.CurrentQuantity;
                    repositories.Books.Update(book);

                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch { }
            finally
            {
                ResourceManagerLocker.ReleaseResource(repositories.BookLends, lend.Key);
            }

            return false;
        }

        public decimal GetPrice(LendedBook lend)
        {
            var days = TimeSpan.FromTicks(DateTime.Now.Ticks - lend.TimeStamp.Ticks).Days;
            decimal price = lend.Price;

            if (days > MAX_RENT_PERIOD)
            {
                days -= MAX_RENT_PERIOD;
                price += days * EXTRA_PRICE * lend.Price;
            }

            return price;
        }

        public List<Book> GetAll()
        {
            return repositories.Books.GetAll();
        }

        public List<LendedBook> GetLends(string ISBN, string personCode)
        {
            return repositories.BookLends.Get(ISBN, personCode);
        }

        public Book? Search(string isbn)
        {
            return repositories.Books.Get(isbn);
        }

        #region private

        /// <summary>
        /// Provides unique index.
        /// </summary>
        /// <returns></returns>
        private int GetIndex()
        {
            lock (indexLocker)
            {
                return index++;
            }
        }

        private static int index = 1;

        private static volatile object indexLocker = new object();
        #endregion
    }
}
