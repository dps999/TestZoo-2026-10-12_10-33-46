using System.Threading;
using Cysharp.Threading.Tasks;

namespace VContainer.Unity
{
    public interface IAsyncStartable
    {
        UniTask StartAsync(CancellationToken cancellation = default);
    }
}
