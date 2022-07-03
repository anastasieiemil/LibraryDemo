using Library.Core.Abstractions.Repositories;
using Library.Core.Concret;
using Library.Core.Domain;
using Library.DAL.Concret;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Library.Tests
{
    public class BookManagerTests
    {

        public BookManagerTests()
        {
            bookManager = new BookManager(new RepositoryProvider());
            books = Utils.BuildBooks(100);
        }

        [Fact]
        public void CanAddBook()
        {
            // Arange
            var book = books[0];

            // Act
            var result = bookManager.Add(book);

            // Asert
            Assert.True(result);
        }

        [Fact]
        public void CanGetLendedBooks()
        {
            // Arange
            var personCode = "John code";
            var book = books[0];
            bookManager.Add(book);
            bookManager.Lend(book.ISBN, "John", personCode);

            // Act
            var lendedBooks = bookManager.GetLends(book.ISBN, personCode);

            // Asert
            Assert.Single(lendedBooks);
            Assert.Equal(personCode, lendedBooks[0].PersonCode);
        }

        [Fact]
        public void FailsWhenAddingTheSameBook()
        {
            // Arange
            var book = books[0];

            // Act
            var result1 = bookManager.Add(book);
            var result2 = bookManager.Add(book);

            // Asert
            Assert.True(result1);
            Assert.False(result2);
        }

        [Fact]
        public void CanLendBook()
        {
            // Arange
            var book = books[0];
            book.ISBN = "test";
            book.Quantity = 5;
            book.CurrentQuantity = 4;
            bookManager.Add(book);

            // Act
            var lendResult = bookManager.Lend(book.ISBN, "John", "John321");

            // Asert
            Assert.True(lendResult);
        }

        [Fact]
        public void CanComputePrice()
        {
            // Arange
            var personCode = "John321";
            var book = books[0];
            book.Price = 10.5M;

            // Add and lend book
            bookManager.Add(book);
            bookManager.Lend(book.ISBN, "John", personCode);

            var lendedBook = bookManager.GetLends(book.ISBN, personCode).FirstOrDefault();
            lendedBook.TimeStamp = lendedBook.TimeStamp.AddDays(-15);

            // Act
            var price = bookManager.GetPrice(lendedBook);

            // Asert
            Assert.Equal(book.Price + book.Price * 1 * 0.01M, price);
        }

        [Fact]
        public void CanReturnBook()
        {
            // Arange
            var personCode = "John321";
            var book = books[0];

            // Add and lend book
            bookManager.Add(book);
            bookManager.Lend(book.ISBN, "John", personCode);

            var lendedBook = bookManager.GetLends(book.ISBN, personCode).FirstOrDefault();

            // Act
            var result = bookManager.Return(lendedBook);

            // Asert
            Assert.True(result);
        }

        [Fact]
        public void FailsWhenReturningTheSameBook()
        {
            // Arange
            var personCode = "John321";
            var book = books[0];

            // Add and lend book
            bookManager.Add(book);
            bookManager.Lend(book.ISBN, "John", personCode);

            var lendedBook = bookManager.GetLends(book.ISBN, personCode).FirstOrDefault();

            // Act
            var result1 = bookManager.Return(lendedBook);

            var result2 = bookManager.Return(lendedBook);

            // Asert
            Assert.True(result1);
            Assert.False(result2);
        }

        [Fact]
        public void FailsWhenReturningTheSameBookConcurential()
        {
            // Arange
            var personCode = "John321";
            var book = books[0];

            // Add and lend book
            bookManager.Add(book);
            bookManager.Lend(book.ISBN, "John", personCode);

            var lendedBook = bookManager.GetLends(book.ISBN, personCode).FirstOrDefault();

            // Act
            bool result1 = false;
            bool result2 = false;

            var task1 = new Task(() =>
            {
                Thread.Sleep(2000);
                result1 = bookManager.Return(lendedBook);

            });

            var task2 = new Task(() =>
            {
                result2 = bookManager.Return(lendedBook);

            });

            task1.Start();
            task2.Start();

            Task.WaitAll(task1, task2);


            // Asert
            Assert.False(result1);
            Assert.True(result2);
        }

        [Fact]
        public void FailsWhenLendingTheSameBookConcurential()
        {
            // Arange
            var book = books[0];
            book.ISBN = "test";
            book.Quantity = 5;
            book.CurrentQuantity = 1;
            bookManager.Add(book);

            // Act
            bool result1 = false;
            bool result2 = false;

            // Define tasks.
            var task1 = new Task(() =>
            {
                Thread.Sleep(2000);
                result1 = bookManager.Lend(book.ISBN, "John", "John321");
            });

            var task2 = new Task(() =>
            {
                result2 = bookManager.Lend(book.ISBN, "Smith", "Smith");
            });

            // Start tasks.
            task1.Start();
            task2.Start();

            // Wait Tasks.
            Task.WaitAll(task1, task2);

            // Asert
            Assert.False(result1);
            Assert.True(result2);
        }

        #region private

        private readonly BookManager bookManager = null!;
        private readonly List<Book> books = null!;
        #endregion
    }
}
