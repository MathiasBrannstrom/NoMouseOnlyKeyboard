using NoMouseOnlyKeyboard.Services.ActionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMouseOnlyKeyboard
{
    internal class Worker : IHostedService
    {
        private readonly HandlerServices _handlerServices;

        public Worker(HandlerServices handlerServices)
        {
            _handlerServices = handlerServices;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
