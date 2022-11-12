 using Blazor.Shared.Commands.Auth.Login;
using Blazor.Shared.Commands.Auth.Refresh;
using Blazor.Shared.Commands.Auth.Register;

namespace Blazor.App;

public interface IDataService : IDisposable
{
    Task<List<string>> Super();

    Task<List<string>> Duper();

}