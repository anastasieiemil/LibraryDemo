using Library.Core.Abstractions.Repositories;
using Library.Core.Domain;
using Library.DAL.Abstractions;
using Newtonsoft.Json;
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
            var lends = entities.Values.Select(x => JsonConvert.DeserializeObject<LendedBook>(x)).ToList();

            return lends.Where(x => x.PersonCode == personCode && x.Book.ISBN == ISBN)
                           .Where(x => x != null)
                           .Select(x => x)
                           .ToList();
        }
    }
}
