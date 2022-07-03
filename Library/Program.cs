using Library;
using Library.Core.Abstractions;
using Library.Core.Abstractions.Repositories;
using Library.Core.Concret;
using Library.DAL.Abstractions;
using Library.DAL.Concret;
using Library.UI;

void Configure()
{
    DependencyInjector.AddSingletone<IRepositoryProvider, RepositoryProvider>();
    DependencyInjector.AddSingletone<IBookManager, BookManager>();
}

void testMutex()
{
    Thread thread1 = new Thread(() =>
    {
        lock (DependencyInjector.Get<IRepositoryProvider>())
        {
            Console.WriteLine("Thread 1 is sleeping...");
            Thread.Sleep(5000);
            Console.WriteLine("Thread 1 awake");

        }
    });

    Thread thread2 = new Thread(() =>
    {
        lock (DependencyInjector.Get<IRepositoryProvider>())
        {
            Thread.Sleep(2000);
            Console.WriteLine("Thread 2 has accessed repo.");
        }

    });

    thread1.Start();
    thread2.Start();


    thread1.Join();
}

void Run()
{
    var bookManager = DependencyInjector.Get<IBookManager>();

    UserInterface.DrawMenu();

    do
    {
        var option = UserInterface.GetOption();

        switch (option)
        {
            case Option.ADD_BOOK:
                {
                    var response = UserInterface.GetBook();

                    if (response.IsSuccess)
                    {
                        if (bookManager.Add(new Library.Core.Domain.Book
                        {
                            Name = response.Obj.Name,
                            ISBN = response.Obj.ISBN,
                            Price = response.Obj.Price,
                            Quantity = response.Obj.Quantity,
                            CurrentQuantity = response.Obj.Quantity,
                        }))
                        {
                            UserInterface.Success("The book was succesfuly added!");
                        }
                        else
                        {
                            UserInterface.Error("An error has occured on saving the book.");
                        }
                    }
                    else
                    {
                        UserInterface.Error(response.Message);
                    }
                    break;
                }

            case Option.LEND_BOOK:
                {
                    var response = UserInterface.GetLend();

                    if (response.IsSuccess)
                    {
                        if (bookManager.Lend(response.Obj.ISBN, response.Obj.PersonName, response.Obj.PersonCode))
                        {
                            UserInterface.Success("The book was succesfuly lended!");
                        }
                        else
                        {
                            UserInterface.Error("An error has occured on saving the lend.");
                        }
                    }
                    else
                    {
                        UserInterface.Error(response.Message);
                    }
                    break;
                }

            case Option.SEARCH_BOOK:
                {
                    var isbn = UserInterface.GetInput("Enter ISBN");
                    if (string.IsNullOrWhiteSpace(isbn))
                    {
                        UserInterface.Error("The isbn is mandatory");
                    }

                    var book = bookManager.Search(isbn);

                    if (book == null)
                    {
                        UserInterface.Error("The book doesn't exist");
                    }
                    else
                    {
                        UserInterface.PrintTable(new List<Library.Core.Domain.Book> { book });
                    }

                    break;
                }

            case Option.RETURN_BOOK:
                {
                    var response = UserInterface.GetReturnedBook();

                    if (response.IsSuccess)
                    {
                        // Get Lends.
                        var lends = bookManager.GetLends(response.Obj.ISBN, response.Obj.PersonCode);

                        // Filter lends. Show only the lends wich must be returned.
                        lends = lends.Where(x => !x.IsReturned).ToList();
                        UserInterface.PrintTable(lends);
                        UserInterface.NewLine();

                        var input = UserInterface.GetInput("Enter the Id of the lend for the book which must be returned");

                        if (int.TryParse(input, out int id))
                        {
                            // Select the lend
                            var selectedLend = lends.FirstOrDefault(x => x.ID == id);

                            // Verify the selecte lend
                            if(selectedLend == null)
                            {
                                UserInterface.Error("The selcted lend doesn't exist.");
                                break;
                            }

                            if(bookManager.Return(selectedLend))
                            {
                                var price = bookManager.GetPrice(selectedLend);
                                UserInterface.Success($"The book has succesfully been returned.");
                                UserInterface.Success($"Total price: {price}");
                                break;
                            }
                            else
                            {
                                UserInterface.Error("An error has occured on processing the return. Please address to support team.");
                            }

                        }

                    }
                    else
                    {
                        UserInterface.Error(response.Message);
                    }
                    break;
                }
            case Option.SHOW_ALL:
                {
                    var books = bookManager.GetAll();
                    UserInterface.PrintTable(books);
                    break;
                }
            case Option.CLEAR:
                {
                    UserInterface.Clear();
                    UserInterface.DrawMenu();

                    break;
                }
            case Option.EXIT:
                {
                    return;
                }
            default:
                UserInterface.Error("Invalid option");
                break;
        }


    } while (true);
}

Configure();
Run();
