using AppText.Core.ContentManagement;
using AppText.Core.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppText.Core.Shared.Commands
{
    public class CommandResult
    {
        private readonly List<ValidationError> _validationErrors;

        public bool IsSuccess { get; set; }

        public IEnumerable<ValidationError> ValidationErrors
        {
            get { return _validationErrors; }
        }

        public CommandResult()
        {
            _validationErrors = new List<ValidationError>();
        }

        public void AddValidationError(ValidationError validationError)
        {
            _validationErrors.Add(validationError);
        }

        public override string ToString()
        {
            var validationErrorsString = String.Empty;
            foreach (var validationError in _validationErrors)
            {
                validationErrorsString += Environment.NewLine + validationError;
            }
            return $"CommandResult.IsSuccess: {IsSuccess}" + validationErrorsString;
        }

        public void UpdateFromValidator(ContentCollectionValidator validator)
        {
            IsSuccess = !validator.Errors.Any();
            foreach (var validationError in validator.Errors)
            {
                AddValidationError(validationError);
            }
        }
    }
}
