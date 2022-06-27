using Library.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Abstractions.Repositories
{
    public interface ILendedBookRepository : IRepository<LendedBook>
    {
        List<LendedBook> Get(string ISBN,string personCode);
    }
}
