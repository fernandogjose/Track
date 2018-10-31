using System.Collections.Generic;
using System.Threading.Tasks;
using Track.Domain.Common.Models;

namespace Track.Domain.Common.Interfaces.Services {
    
    public interface ISecurityService {
    
        Task AddAsync (SecurityAddRequest securityAddRequest);

        Task<List<SecurityListResponse>> ListAsync ();
    }
}