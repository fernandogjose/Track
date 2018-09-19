using System.Threading.Tasks;
using Track.Domain.ConfigurationData.Models;

namespace Track.Domain.ConfigurationData.Interfaces.Services
{
    public interface ILogService
    {
         void AddAsync (LogRequest logRequest);
    }
}