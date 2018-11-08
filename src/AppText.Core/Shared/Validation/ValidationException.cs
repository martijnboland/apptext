using System;
using System.Collections.Generic;
using System.Text;

namespace AppText.Core.Shared.Validation
{
    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<ValidationError> validationErrors)
            : this("Validation Exception, see ValidationErrors property for details.", validationErrors)
        {}

        public ValidationException(string message, IEnumerable<ValidationError> validationErrors)
            : base(message)
        {
            ValidationErrors = validationErrors;
        }


        public IEnumerable<ValidationError> ValidationErrors { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(this.Message);
            sb.AppendLine();
            foreach (var error in this.ValidationErrors)
            {
                var message = String.Format(error.ErrorMessage, error.Parameters);
                sb.AppendLine($"{error.Name}: {message}");
            }
            return sb.ToString();
        }
    }
}
