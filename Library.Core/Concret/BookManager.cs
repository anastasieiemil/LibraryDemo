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

        // Protects repository.
        private volatile object repositoryMutex = new object();
        private static readonly int MUTEX_TIMEOUT = 2000;
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
            // Validate.

            // Add book.
            var savedBook = repositories.Books.Add(book);

            return savedBook != null;
        }

        public bool Lend(string ISBN, string personName, string personCode)
        {
            // Validate.

            // Lock dal and try to create and save a lendedBook.
            if (Monitor.TryEnter(repositoryMutex, MUTEX_TIMEOUT))
            {
                var book = repositories.Books.Get(ISBN);

                // Check if the book is available.
                if (book == null || book.CurrentQuantity == 0)
                {
                    Monitor.Exit(repositoryMutex);
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
                };

                // Add Lend;
                var savedLend = repositories.BookLends.Add(lend);
                if (savedLend == null)
                {
                    Monitor.Exit(repositoryMutex);
                    return false;
                }

                // Update the book.
                --book.CurrentQuantity;
                repositories.Books.Update(book);

                Monitor.Exit(repositoryMutex);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Return(LendedBook lend)
        {
            // Lock dal and try to create and save a lendedBook.
            if (Monitor.TryEnter(repositoryMutex, MUTEX_TIMEOUT))
            {
                // Get lend.
                //var lend = lends.Get(ISBN, personCode);
                if (lend == null || lend.IsReturned)
                {
                    Monitor.Exit(repositoryMutex);
                    return false;
                }

                // Update Lend.
                lend.IsReturned = true;
                lend.ReturnedTimeStamp = DateTime.Now;
                repositories.BookLends.Update(lend);

                // Update the book.
                var book = lend.Book;
                --book.CurrentQuantity;
                repositories.Books.Update(book);

                Monitor.Exit(repositoryMutex);
                return true;
            }
            else
            {
                return false;
            }
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

        public Book? Search(string isbn)
        {
            return repositories.Books.Get(isbn);
        }
    }
}
