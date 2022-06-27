using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UI
{
    public class Response<TModel>
    {
        public TModel? Obj { get; set; }
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = null!;
    }
}
