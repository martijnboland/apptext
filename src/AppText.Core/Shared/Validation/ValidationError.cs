using System;

namespace AppText.Core.Shared.Validation
{
    /// <summary>
    /// Represents a validation error.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The (property) name of this validation error.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The error message.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Optional parameters for the error message.
        /// </summary>
        public object[] Parameters { get; private set; }

        /// <summary>
        /// Protected constructor. Use Create() to create a new ValidationError.
        /// </summary>
        protected ValidationError()
        {}

        /// <summary>
        /// Create a new validator with the given name and error message.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static ValidationError Create(string name, string errorMessage)
        {
            return new ValidationError {Name = name, ErrorMessage = errorMessage, Parameters = null};
        }

        /// <summary>
        /// Create a new validator with the given name, error message and parameters that can occur in the error message.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="errorMessage"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ValidationError Create(string name, string errorMessage, params object[] parameters)
        {
            return new ValidationError {Name = name, ErrorMessage = errorMessage, Parameters = parameters};
        }

        /// <summary>
        /// ToString() override. Returns the error message merged with the optional parameters.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Parameters != null)
            {
                return String.Format(this.ErrorMessage, this.Parameters);
            }
            return this.ErrorMessage;
        }
    }
}