using AppText.Shared.Commands;
using AppText.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AppText.Shared.Infrastructure.Mvc
{
    public static class ControllerExtensions
    {
        public static IActionResult HandleCreateCommandResult(this ControllerBase controller, CommandResult commandResult, string uri, object payload)
        {
            switch (commandResult.Status)
            {
                case ResultStatus.Success:
                    return controller.Created(uri, payload);
                case ResultStatus.ValidationError:
                    return controller.UnprocessableEntity(GetValidationErrorsResult(commandResult));
                default:
                    throw new NotImplementedException();
            }
        }

        public static IActionResult HandleUpdateCommandResult(this ControllerBase controller, CommandResult commandResult)
        {
            switch (commandResult.Status)
            {
                case ResultStatus.Success:
                    return controller.Ok();
                case ResultStatus.ValidationError:
                    return controller.UnprocessableEntity(GetValidationErrorsResult(commandResult));
                case ResultStatus.VersionError:
                    return controller.Conflict(commandResult);
                case ResultStatus.NotFound:
                    return controller.NotFound();
                default:
                    throw new NotImplementedException();
            }
        }

        public static IActionResult HandleDeleteCommandResult(this ControllerBase controller, CommandResult commandResult)
        {
            switch (commandResult.Status)
            {
                case ResultStatus.Success:
                    return controller.NoContent();
                case ResultStatus.VersionError:
                    return controller.Conflict(commandResult);
                case ResultStatus.ValidationError:
                    return controller.Conflict(GetValidationErrorsResult(commandResult));
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns an object that only contains validation errors of the CommandResult and converts the property names to camelCase
        /// for easier usage in client apps.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private static object GetValidationErrorsResult(CommandResult result)
        {
            return new
            {
                errors = result.ValidationErrors.Select(ve => new { name = ve.Name.ToCamelCase(), ve.ErrorMessage, ve.Parameters })
            };
        }
    }
}
