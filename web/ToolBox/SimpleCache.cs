using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace webapp.ToolBox
{
    /// <summary>
    /// Egyszerű singleton memória gyorsítótár implementáció
    /// </summary>
    public class SimpleCache
    {
        private MemoryCache _cache;
        private static SimpleCache _instance;

        public SimpleCache()
        {
            _cache = new MemoryCache(this.GetType().Name);
        }

        /// <summary>
        /// A szingleton példány elérése
        /// </summary>
        public static SimpleCache Current
        {
            get
            {
                if (_instance == null)
                    _instance = new SimpleCache();
                return _instance;
            }
        }

        /// <summary>
        /// Okos getter, ami egyben setter is
        /// </summary>
        /// <typeparam name="T">Típus</typeparam>
        /// <param name="key">Kulcs</param>
        /// <param name="acquire">Setter, ha nincs még a cache-ben</param>
        /// <param name="cacheTime">Evikciós idő</param>
        /// <returns></returns>
        public virtual T Get<T>(string key, Func<T> acquire, int cacheTime = 60)
        {
            if (_cache.Contains(key))
            {
                return (T)_cache[key];
            }
            else
            {
                T result = default(T);
                result = acquire();
                if (result != null)
                {
                    var policy = new CacheItemPolicy();
                    _cache.Set(new CacheItem(key, result), policy);
                }
                return result;
            }
        }

        
    }
}