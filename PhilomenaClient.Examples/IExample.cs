using System.Threading.Tasks;

namespace Philomena.Client.Examples
{
    public interface IExample
    {
        string Description { get; }

        Task RunExample();
    }
}
