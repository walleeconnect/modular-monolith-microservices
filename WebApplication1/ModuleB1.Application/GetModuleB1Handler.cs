using DependentServices.Interfaces;
using MediatR;

namespace ModuleB1.Application
{
    public class GetModuleB1Handler : IRequestHandler<GetModuleB1Query, string>
    {
        IModuleAService moduleAService;
        public GetModuleB1Handler(IModuleAService moduleAService)
        {
            this.moduleAService = moduleAService;
        }

        public async Task<string> Handle(GetModuleB1Query request, CancellationToken cancellationToken)
        {
            var result = await moduleAService.GetModuleA11();
            return result;
        }
    }
}
