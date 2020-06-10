using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Shared.Validation
{
    /// <summary>
    /// Default implementation of IValidator[T] that uses Data Annotation attributes to perform validation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Validator<T> : IValidator<T> where T : class
    {
        private readonly List<ValidationError> _errors = new List<ValidationError>();

        /// <summary>
        /// Checks if the given object is valid. If this method returns true, the errors can be obtained via GetErrors().
        /// </summary>
        /// <param name="objectToValidate"></param>
        /// <returns></returns>
        public async Task<bool> IsValid(T objectToValidate)
        {
            ClearErrors();
            await ValidateAsync(objectToValidate);
            return _errors.Count == 0;
        }
        /// <summary>
        /// Returns the list of validation errors.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ValidationError> Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// Validate the given object with data annotations and perform any custom validations afterwards.
        /// </summary>
        /// <param name="objectToValidate"></param>
        /// <returns></returns>
        protected Task ValidateAsync(T objectToValidate)
        {
            // Data annotations validation
            ValidateDataAnnotations(objectToValidate, String.Empty);
            
            // Hook for custom validation for inheritors.
            if (_errors.Count == 0)
            {
                return ValidateCustom(objectToValidate);
            }
            return Task.CompletedTask;
        }

        private void ValidateDataAnnotations(object objectToValidate, string prefix)
        {
            var props = TypeDescriptor.GetProperties(objectToValidate).Cast<PropertyDescriptor>();
            AddErrors(from prop in props
                      from attribute in prop.Attributes.OfType<ValidationAttribute>()
                      where !prop.Attributes.OfType<IgnoreValidationAttribute>().Any() && !attribute.IsValid(prop.GetValue(objectToValidate))
                      select GenerateValidationError(prefix, prop.Name, attribute));

            // Recurse into collections
            var enumerableProperties = props.Where(p => typeof(ICollection).IsAssignableFrom(p.PropertyType));
            foreach (var prop in enumerableProperties)
            {
                var nestedPrefix = String.IsNullOrEmpty(prefix) ? prop.Name :  $"{prefix}.{prop.Name}";
                var enumerable = (IEnumerable)prop.GetValue(objectToValidate);
                var idx = 0;
                foreach (var obj in enumerable)
                {
                    var objPrefix = $"{nestedPrefix}[{idx}].";
                    ValidateDataAnnotations(obj, objPrefix);
                    idx++;
                }
            }
        }

        private ValidationError GenerateValidationError(string prefix, string propertyName, ValidationAttribute attribute)
        {
            // Post-process validation error by converting everything after the | suffix to params (e.g. StringTooLong|0,20)
            var errorMessage = attribute.FormatErrorMessage(propertyName);
            var errorMessageParts = errorMessage.Split('|');
            var errorMessageText = errorMessageParts[0];
            var errorMessageParams = errorMessageParts.Length > 1
                ? errorMessageParts[1].Split(',')
                : new object[0];
            return new ValidationError { Name = $"{prefix}{propertyName}", ErrorMessage = errorMessageText, Parameters = errorMessageParams };
        }

        protected virtual Task ValidateCustom(T objectToValidate)
        {
            // Nothing in the base class
            return Task.CompletedTask;
        }

        /// <summary>
        /// Add a new error to the errors collection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <param name="messageParams"></param>
        protected void AddError(string name, string message, params object[] messageParams)
        {
            AddError(new ValidationError { Name = name, ErrorMessage = message, Parameters = messageParams });
        }

        /// <summary>
        /// Add a new error to the errors collection.
        /// </summary>
        /// <param name="error"></param>
        protected void AddError(ValidationError error)
        {
            _errors.Add(error);
        }


        /// <summary>
        /// Add a list of errors.
        /// </summary>
        /// <param name="errors"></param>
        protected void AddErrors(IEnumerable<ValidationError> errors)
        {
            _errors.AddRange(errors);
        }

        /// <summary>
        /// Clear the current validation errors.
        /// </summary>
        protected void ClearErrors()
        {
            _errors.Clear();
        }
    }
}
