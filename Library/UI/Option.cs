using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UI
{
    public enum Option : int
    {
        UNKNOWN = 0,

        [Description("Add new book")]
        ADD_BOOK = 1,

        [Description("Lend book")]
        LEND_BOOK = 2,

        [Description("Return book")]
        RETURN_BOOK = 3,

        [Description("Search book")]
        SEARCH_BOOK = 4,

        [Description("Show all books")]
        SHOW_ALL = 5,

        [Description("Clear console")]
        CLEAR = 6,

        [Description("Exit")]
        EXIT = 7,
    }
}
