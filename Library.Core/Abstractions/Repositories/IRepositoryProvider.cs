using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Abstractions.Repositories
{
    public interface IRepositoryProvider
    {
        IBookRepository Books { get; }
        ILendedBookRepository BookLends { get; }
    }
}
