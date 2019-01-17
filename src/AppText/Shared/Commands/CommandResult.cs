using AppText.Shared.Validation;
using System;
using System.Collections.Generic;

namespace AppText.Shared.Commands
{
    public class CommandResult
    {
        private readonly List<ValidationError> _validationErrors;

        public ResultStatus Status { get; private set; }

        public object ResultData { get; private set; }

        public IEnumerable<ValidationError> ValidationErrors
        {
            get { return _validationErrors; }
        }

        public CommandResult()
        {
            _validationErrors = new List<ValidationError>();
            Status = ResultStatus.Success;
        }

        public void AddValidationError(ValidationError validationError)
        {
            _validationErrors.Add(validationError);
            Status = ResultStatus.ValidationError;
        }

        public void AddValidationErrors(IEnumerable<ValidationError> validationErrors)
        {
            foreach (var validationError in validationErrors)
            {
                AddValidationError(validationError);
            }
        }
        public void SetVersionError()
        {
            Status = ResultStatus.VersionError;
        }

        public void SetNotFound()
        {
            Status = ResultStatus.NotFound;
        }

        public void SetResultData(object data)
        {
            ResultData = data;
        }

        public override string ToString()
        {
            var validationErrorsString = String.Empty;
            foreach (var validationError in _validationErrors)
            {
                validationErrorsString += Environment.NewLine + validationError;
            }
            return $"CommandResult.Status: {Status}" + validationErrorsString;
        }
    }

    public enum ResultStatus
    {
        Success,
        ValidationError,
        VersionError,
        NotFound
    }
}
