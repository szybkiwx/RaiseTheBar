using log4net;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiseTheBar.Services
{
    public class RedisCachingService : ICashingService
    {
        private RedisClient _client;
        private static readonly ILog log = LogManager.GetLogger(typeof(RedisCachingService));
        public RedisCachingService(RedisClient client)
        {
            _client = client;
        }

        public T Get<T>(string key)
        {   try {
                return _client.Get<T>(key);
            }
            catch(RedisException e)
            {
                log.Info("Error getting data from Redis", e);
                return default(T);
            }
        }

        public void Set<T>(string key, T cachedObject)
        {
            try
            {
                _client.Set(key, cachedObject);
            }
            catch (RedisException e)
            {
                log.Info("Error putting data into Redis", e);
            }
        }
    }
}