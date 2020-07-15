using Api.Core.Tasks.Commands.Failure;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Failure_Microservice.Web.Queues
{
    public static class FailureQueueConfig
    {
        private static  IMediator _mediator;
        private static  IServiceCollection _services;
        private static  IConfiguration _configuration;
        private static IQueueClient _queueClient;
      
        public static void FailureQueueConfiguration(IServiceCollection services, IConfiguration config)
        {
            _configuration = config;
            _services = services;

            _mediator = services.BuildServiceProvider().GetService<IMediator>();

            var queueName = _configuration.GetSection("Settings:MicroServices:FailureMicroservice:QueueName").Value;
            var ConnectionString = _configuration.GetSection("Settings:MicroServices:FailureMicroservice:ConnectionString").Value;
            _queueClient = new QueueClient(ConnectionString, queueName, ReceiveMode.PeekLock);
            RegisterOnMessageHandlerAndReceiveMessages();
        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,
                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            try
            {
                // Pcrocess the message.
                Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

                var exception = JsonConvert.DeserializeObject<Exception>(Encoding.UTF8.GetString(message.Body));
                var command = new CreateFailureCommand(exception);
                var result = await _mediator.Send(command);

                // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
                // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
                // to avoid unnecessary exceptions.

            }
            catch(Exception ex)
            {
                var a = ex;
            }

            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            //await _queueClient.AbandonAsync(message.SystemProperties.LockToken);
        }
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
