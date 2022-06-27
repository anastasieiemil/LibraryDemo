using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Utils
    {
        public static string? GetDescriptionAttribute(Enum val)
        {
            Type type = val.GetType();

            var members = type.GetMember(val.ToString());
            if (members.Length == 0)
            {
                return null;
            }
            var attribute = members[0].GetCustomAttributes(true)
                                      .FirstOrDefault(x => x.GetType() == descriptionAttributeType) as DescriptionAttribute;

            return attribute?.Description;
        }

        /// <summary>
        /// Validate object.
        /// </summary>
        /// <typeparam name="TObjectModel"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Validate<TObjectModel>(TObjectModel obj) where TObjectModel : class
        {
            var type = typeof(TObjectModel);
            StringBuilder response = new StringBuilder();

            // Get all validation properties.
            var properties = type.GetProperties()
                                 .Where(x => x.GetCustomAttributes(false)
                                            .Any(y => y.GetType().BaseType == validationAttributeType))
                                 .ToList();


            foreach (var property in properties)
            {
                var validators = property.GetCustomAttributes(false)
                                         .Where(y => y.GetType().BaseType == validationAttributeType)
                                         .Select(x => (ValidationAttribute)x)
                                         .ToList();

                var value = property.GetValue(obj);
                foreach (var validator in validators)
                {

                    try
                    {
                        validator.Validate(value, property.Name);
                    }
                    catch (ValidationException ex)
                    {
                        response.AppendLine(ex.Message);
                    }
                    catch { }

                }

            }

            return response.ToString();
        }

        #region private
        private static readonly Type descriptionAttributeType = typeof(DescriptionAttribute);
        private static readonly Type validationAttributeType = typeof(ValidationAttribute);

        #endregion

    }
}
