using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.Tasks.Commands.Failure
{
    public class CreateFailureCommand : IRequest<ResultTask>
    {
        public Exception Exception { get; set; }
        public CreateFailureCommand(Exception exception)
        {
            this.Exception = exception;
        }
    }
}
