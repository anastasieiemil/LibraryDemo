using Library.Validators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Validators
{
    public class StringLength : ValidatorAttribute
    {

        public StringLength(int max)
        {
            this.max = max;
        }

        public StringLength(int min, int max)
        {
            this.max = max;
            this.min = min;
        }

        public override ValidatorResponse Validate(object obj)
        {
            var response = new ValidatorResponse
            {
                IsSuccess = false,
                Message = string.Empty,
            };

            // Validate 
            if (obj is string stringObj)
            {
                if (stringObj.Length > max || stringObj.Length < min)
                {
                    response.Message = $"The string input {stringObj} is not in the range {min} - {max}";
                    return response;
                }
                else
                {
                    response.IsSuccess = true;
                    return response;
                }
            }
            else
            {
                return response;
            }
        }

        #region private

        private int min = 0;
        private int max;

        #endregion

    }
}
