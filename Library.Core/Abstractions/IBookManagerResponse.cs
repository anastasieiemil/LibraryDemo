using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Abstractions
{
    public interface IBookManagerResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
