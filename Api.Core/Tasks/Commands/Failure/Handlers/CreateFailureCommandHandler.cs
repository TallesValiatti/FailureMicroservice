using Api.Core.Interfaces.Infra.Repositories.Security;

using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Core.Entities.Failure;
using Newtonsoft.Json;

namespace Api.Core.Tasks.Commands.Failure.Handlers
{
    class CreateFailureCommandHandler : IRequestHandler<CreateFailureCommand, ResultTask>
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IFailureRepository<Api.Core.Entities.Failure.Failure> _failureRepository;

        public CreateFailureCommandHandler(
     
            IFailureRepository<Api.Core.Entities.Failure.Failure> failureRepository,
            IMediator mediator,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _mediator = mediator;
            _failureRepository = failureRepository;
        }
        public async Task<ResultTask> Handle(CreateFailureCommand command, CancellationToken cancellationToken)
        {
            try
            {
                //create the object to be returned on the end of this task
                var result = new ResultTask();

                //insert the entity on the database
                await _failureRepository.AddAsync(new Api.Core.Entities.Failure.Failure
                {
                    Body = JsonConvert.SerializeObject(command.Exception),
                    CreateAt = DateTime.UtcNow
                });
                
                return await Task.FromResult(result);
            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }
    }
}
