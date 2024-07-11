using MediatR;
using ModuleA.Abstraction;
using ModuleA1.Application;

namespace ModuleA.Monolith
{
    public class ModuleA11Service : IModuleAService
    {
        private readonly IMediator _mediator;

        public ModuleA11Service(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> GetModuleA11()
        {
            // Call using MediatR
            return await _mediator.Send(new GetModuleAQuery());
        }
    }
}
