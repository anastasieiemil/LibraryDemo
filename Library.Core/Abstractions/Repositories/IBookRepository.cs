using Library.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Abstractions.Repositories
{
    /// <summary>
    /// Book repository methods.
    /// </summary>
    public interface IBookRepository : IRepository<Book>
    {
    }
}
