using Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UI
{
    public static class UserInterface
    {
        public static void DrawMenu()
        {
            Console.WriteLine("Chose an option");

            foreach (var option in options)
            {
                Console.WriteLine($"{(int)option.Key} -> {option.Value}");
            }
        }

        public static Option GetOption()
        {
            var option = Console.ReadLine();

            if (Enum.TryParse(option, out Option parsedOption))
            {
                return parsedOption;
            }
            else
            {
                return Option.UNKNOWN;
            }
        }
        public static void Clear()
        {
            Console.Clear();
        }

        public static Response<Book> GetBook()
        {
            var response = new Response<Book>();
            response.Obj = new Book();

            // Read data.
            Console.WriteLine("Enter book name");
            response.Obj.Name = Console.ReadLine()?.Trim();

            Console.WriteLine("Enter book ISBN");
            response.Obj.ISBN = Console.ReadLine()?.Trim();

            Console.WriteLine("Enter rent price");
            var price = Console.ReadLine()?.Trim();
            decimal.TryParse(price, out decimal parsedPrice);
            response.Obj.Price = parsedPrice;


            Console.WriteLine("Enter quantity");
            var quantity = Console.ReadLine()?.Trim();
            int.TryParse(quantity, out int parsedQuantity);
            response.Obj.Quantity = parsedQuantity;


            // Validate data.
            var validationResponse = Utils.Validate(response.Obj);
            response.IsSuccess = string.IsNullOrWhiteSpace(validationResponse);
            response.Message = validationResponse;

            return response;

        }

        public static Response<Lend> GetLend()
        {
            var response = new Response<Lend>();
            response.Obj = new Lend();

            // Read data.
            Console.WriteLine("Enter Person name");
            response.Obj.PersonName = Console.ReadLine()?.Trim();

            Console.WriteLine("Enter Person code");
            response.Obj.PersonCode = Console.ReadLine()?.Trim();

            Console.WriteLine("Enter book ISBN");
            response.Obj.ISBN = Console.ReadLine()?.Trim();


            // Validate data.
            var validationResponse = Utils.Validate(response.Obj);
            response.IsSuccess = string.IsNullOrWhiteSpace(validationResponse);
            response.Message = validationResponse;

            return response;

        }

        public static Response<ReturnedBook> GetReturnedBook()
        {
            var response = new Response<ReturnedBook>();
            response.Obj = new ReturnedBook();

            // Read data.
            Console.WriteLine("Enter book ISBN");
            response.Obj.ISBN = Console.ReadLine()?.Trim();

            Console.WriteLine("Enter Person code");
            response.Obj.PersonCode = Console.ReadLine()?.Trim();

            // Validate data.
            var validationResponse = Utils.Validate(response.Obj);
            response.IsSuccess = string.IsNullOrWhiteSpace(validationResponse);
            response.Message = validationResponse;

            return response;
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static string GetInput(string message)
        {
            Console.WriteLine(message);
            var input = Console.ReadLine()?.Trim();

            return input;
        }

        public static void NewLine()
        {
            Console.WriteLine();
        }

        // Prints all objects as an table.
        public static void PrintTable<TModel>(List<TModel> objs) where TModel : class
        {
            if (objs == null)
            {
                return;
            }

            // Get Header.
            var type = typeof(TModel);

            // Get all properties.
            var propeties = type.GetProperties();


            var header = string.Empty;
            foreach (var property in propeties)
            {
                header += $"{property.Name}|";
            }
            Console.WriteLine(header);
            Console.WriteLine("-------------------------------------------------");

            StringBuilder data = new StringBuilder();
            // Write values.
            foreach (var value in objs)
            {
                foreach (var property in propeties)
                {
                    data.Append($"{property.GetValue(value)?.ToString() ?? string.Empty}|");
                }
            }

            Console.WriteLine(data.ToString());
        }


        #region private



        private static readonly Dictionary<Option, string?> options = Enum.GetValues<Option>()
                                                                        .Where(x => x != Option.UNKNOWN)
                                                                        .ToDictionary(x => x, y => Utils.GetDescriptionAttribute(y));
        #endregion

    }
}
