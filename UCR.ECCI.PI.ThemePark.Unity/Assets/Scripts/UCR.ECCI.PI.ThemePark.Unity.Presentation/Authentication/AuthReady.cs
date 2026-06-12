// Presentation/Auth/AuthReady.cs
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.Authentication
{
    public sealed class AuthReady : IAuthReady
    {
        private readonly TaskCompletionSource<bool> _tcs = new();
        public Task Ready => _tcs.Task;
        public void SignalReady() => _tcs.TrySetResult(true);
    }
}
