using System;

namespace AppText.Shared.Validation
{
    /// <summary>
    /// Represents a validation error.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The (property) name of this validation error.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Optional parameters for the error message.
        /// </summary>
        public object[] Parameters { get; set; }

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
