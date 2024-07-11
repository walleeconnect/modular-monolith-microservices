using DependentServices.Interfaces;
using MediatR;
using ModuleA1.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependentServices.Services
{
    public class ModuleAMonolith : IModuleAService
    {
        private readonly IMediator _mediator;

        public ModuleAMonolith(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> GetModuleA11()
        {
            return await _mediator.Send(new GetModuleAQuery());
        }
    }
}
