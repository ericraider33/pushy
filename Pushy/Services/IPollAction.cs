using System.Threading.Tasks;

namespace Pushy.Services
{
    public interface IPollActionAsync
    {
        Task poll();
    }
}