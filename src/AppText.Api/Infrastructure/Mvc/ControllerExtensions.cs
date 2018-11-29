using AppText.Core.Shared.Commands;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AppText.Api.Infrastructure.Mvc
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
                    return controller.UnprocessableEntity(commandResult);
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
                    return controller.UnprocessableEntity(commandResult);
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
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
