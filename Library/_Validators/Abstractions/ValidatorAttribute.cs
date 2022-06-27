using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Validators.Abstractions
{
    /// <summary>
    /// This class should be used for defining new valdators
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidatorAttribute : Attribute, IValidator
    {
        public abstract ValidatorResponse Validate(object obj);

    }
}
