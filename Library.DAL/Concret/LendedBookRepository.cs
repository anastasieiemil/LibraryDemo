using Library.Core.Abstractions.Repositories;
using Library.Core.Domain;
using Library.DAL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Concret
{
    public class LendedBookRepository : Repository<LendedBook>, ILendedBookRepository
    {

        List<LendedBook> ILendedBookRepository.Get(string ISBN, string personCode)
        {
            return entities.Where(x => x.Value.PersonCode == personCode && x.Value.Book.ISBN == ISBN)
                           .Select(x => x.Value)
                           .ToList();
        }
    }
}
