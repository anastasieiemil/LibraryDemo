using Library.Core.Abstractions.Repositories;
using Library.Core.Concret;
using Library.Core.Domain;
using Library.DAL.Concret;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            book.Quantity = 4;
            bookManager.Add(book);

            // Act
            var lendResult = bookManager.Lend(book.ISBN,"John","John321");

            // Asert
            Assert.True(lendResult);
        }


        #region private

        private readonly BookManager bookManager = null!;
        private readonly List<Book> books = null!;
        #endregion
    }
}
