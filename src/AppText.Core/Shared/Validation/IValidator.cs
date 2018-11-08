using System.Collections.Generic;

namespace AppText.Core.Shared.Validation
{
    public interface IValidator<T> where T: class
    {
        /// <summary>
        /// Checks if the given object is valid.
        /// </summary>
        /// <param name="objectToValidate"></param>
        /// <returns></returns>
        bool IsValid(T objectToValidate);

        /// <summary>
        /// Returns the list of validation errors.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ValidationError> Errors { get; }
    }
}
