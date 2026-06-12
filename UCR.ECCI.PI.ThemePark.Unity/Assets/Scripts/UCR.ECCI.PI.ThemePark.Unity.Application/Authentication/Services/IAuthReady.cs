using System.Threading.Tasks;

// Application/Auth/IAuthReady.cs
namespace UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services
{
    public interface IAuthReady
    {
        Task Ready { get; }
        void SignalReady();
    }
}
