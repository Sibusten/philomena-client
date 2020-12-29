using System.Threading.Tasks;

namespace Sibusten.Philomena.Client.Examples
{
    public interface IExample
    {
        string Description { get; }

        Task RunExample();
    }
}
