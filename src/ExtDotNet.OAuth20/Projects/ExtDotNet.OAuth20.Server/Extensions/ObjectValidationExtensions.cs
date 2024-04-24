// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ExtDotNet.OAuth20.Server.Extensions;

public static class ObjectValidationExtensions
{
    public static IReadOnlyCollection<ValidationResult> TryValidate(this object value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(value, new ValidationContext(value), validationResults, true))
        {
            return validationResults;
        }

        var properties = value.GetType().GetProperties().Where(x => !x.PropertyType.IsSimple());

        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);

            if (propertyValue == null) continue;

            if (propertyValue is ICollection propertyItems)
            {
                foreach (var propertyItem in propertyItems)
                {
                    validationResults.AddRange(propertyItem.TryValidate());
                }
            }
            else
            {
                validationResults.AddRange(propertyValue.TryValidate());
            }
        }

        return validationResults;
    }
}
