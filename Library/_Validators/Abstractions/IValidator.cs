using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Validators
{
    public interface IValidator
    {
        /// <summary>
        ///  Validates the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ValidatorResponse Validate(object obj);
    }
}
