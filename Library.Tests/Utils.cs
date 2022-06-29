using Library.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests
{
    public static class Utils
    {
        /// <summary>
        /// Builds books.
        /// </summary>
        /// <param name="numberOfBooks"></param>
        /// <returns></returns>
        public static List<Book> BuildBooks(int numberOfBooks)
        {
            var books = new List<Book>();
            var randomGenerator = new Random(); 

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

            return books;
        }
    }
}
