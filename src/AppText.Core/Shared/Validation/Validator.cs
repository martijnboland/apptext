using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AppText.Core.Shared.Validation
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
        public bool IsValid(T objectToValidate)
        {
            _errors.Clear();
            Validate(objectToValidate);
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
        protected void Validate(T objectToValidate)
        {
            // Data annotations validation
            AddErrors(from prop in TypeDescriptor.GetProperties(objectToValidate).Cast<PropertyDescriptor>()
                from attribute in prop.Attributes.OfType<ValidationAttribute>()
                where ! prop.Attributes.OfType<IgnoreValidationAttribute>().Any() && !attribute.IsValid(prop.GetValue(objectToValidate))
                select new ValidationError { Name = prop.Name, ErrorMessage = attribute.FormatErrorMessage(prop.Name) });
            // Hook for custom validation for inheritors.
            ValidateCustom(objectToValidate);
        }

        /// <summary>
        /// Perform custom validation logic.
        /// </summary>
        protected virtual void ValidateCustom(T objectToValidate)
        {
            // nothing in the base class.
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
    }
}
