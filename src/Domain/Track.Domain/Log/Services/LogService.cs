using System;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;
using System.Threading.Tasks;

namespace Track.Domain.ConfigurationData.Services {
    public class LogService: ILogService {

        private readonly ILogMongoRepository _logMongoRepository;

        public LogService (ILogMongoRepository logMongoRepository) {
            _logMongoRepository = logMongoRepository;
        }

        public async Task AddAsync(LogRequest logRequest)
        {
            await _logMongoRepository.AddAsync(logRequest);
        }
    }
}