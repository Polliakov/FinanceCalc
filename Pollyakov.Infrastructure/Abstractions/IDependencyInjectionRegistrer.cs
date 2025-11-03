using Microsoft.Extensions.DependencyInjection;

namespace Pollyakov.Infrastructure.Abstractions
{
    public interface IDependencyInjectionRegister
    {
        public IServiceCollection Register(IServiceCollection services);
    }
}
