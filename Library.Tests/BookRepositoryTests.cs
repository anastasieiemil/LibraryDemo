using Library.Core.Domain;
using Library.DAL.Concret;
using System;
using System.Collections.Generic;
using Xunit;

namespace Library.Tests
{
    public class BookRepositoryTests
    {

        public BookRepositoryTests()
        {
            repository = new BookRepository();

            BuildBooks();
        }

        [Fact]
        public void CanAddBook()
        {
            // Arange
            var book = books[0];

            // Act
            var addedBook = repository.Add(book);

            // Asert
            Assert.NotNull(addedBook);
            Assert.Equal(books[0], addedBook);
        }

        [Fact]
        public void CanAddMultipleBooks()
        {
            // Arange
            var numberOfAddedBooks = randomGenerator.Next(1, numberOfBooks);
            var booksForAdding = books.GetRange(0, numberOfAddedBooks);

            // Act
            var addedBooks = new List<Book?>();
            foreach (var book in booksForAdding)
            {
                var addedBook = repository.Add(book);
                addedBooks.Add(addedBook);
            }

            // Asert
            Assert.Equal(numberOfAddedBooks, addedBooks.Count);
            Assert.Equal(books[0], addedBooks[0]);
        }

        [Fact]
        public void CanGetBookByISBN()
        {
            // Arange
            var bookIndex = randomGenerator.Next(numberOfBooks);
            foreach (var bookForAdding in books)
            {
                repository.Add(bookForAdding);
            }
            var bookForSearching = books[bookIndex];

            // Act
            var searchedBook = repository.Get(bookForSearching.ISBN);

            // Asert
            Assert.NotNull(searchedBook);
            Assert.Equal(searchedBook, bookForSearching);
        }

        [Fact]
        public void CanUpdateBook()
        {
            // Arange
            var book = new Book
            {
                ISBN = "Test",
                Quantity = 1,
            };
            repository.Add(book);

            // Act
            book.Quantity = 100;
            var updatedBook = repository.Update(book);

            // Asert
            Assert.NotNull(updatedBook);
            Assert.Equal(updatedBook.Quantity, book.Quantity);
        }


        [Fact]
        public void CanGetAllBoks()
        {
            // Arange
            foreach (var book in books)
            {
                repository.Add(book);
            }
            var bookIndex = randomGenerator.Next(numberOfBooks);

            // Act
            var allBooks = repository.GetAll();

            // Asert
            Assert.Equal(allBooks.Count, numberOfBooks);
            Assert.Contains(allBooks[bookIndex], books);
        }

        [Fact]
        public void DoesFaillWhenAddingTheSameBook()
        {
            // Arange
            var bookIndex = randomGenerator.Next(numberOfBooks);
            var book = books[bookIndex];

            // Act
            var addedBook1 = repository.Add(book);
            var addedBook2 = repository.Add(book);

            // Asert
            Assert.Equal(book, addedBook1);
            Assert.Null(addedBook2);
        }

        #region private

        private void BuildBooks()
        {
            books = new List<Book>();

            for (int i = 0; i < numberOfBooks; i++)
            {
                var quantity = randomGenerator.Next(100);
                books.Add(new Book
                {
                    Name = $"Book {i}",
                    ISBN = $"ISBN book {i}",
                    Price = (decimal)(randomGenerator.Next(1000)) / (randomGenerator.Next(10) + 1),
                    Quantity = quantity,
                    CurrentQuantity = quantity,
                });
            }

        }

        private static readonly Random randomGenerator = new Random();
        private static readonly int numberOfBooks = 20;
        private BookRepository repository;

        private List<Book> books = null!;
        #endregion
    }
}