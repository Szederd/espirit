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
    public class AdvancedCache
    {
        private MemoryCache _cache;
        private static AdvancedCache _instance;
        private MemoryCache _tempCache;

        public AdvancedCache()
        {
            _cache = new MemoryCache(this.GetType().Name);
            _tempCache = new MemoryCache(this.GetType().Name + "_temp");
        }

        /// <summary>
        /// A szingleton példány elérése
        /// </summary>
        public static AdvancedCache Current
        {
            get
            {
                if (_instance == null)
                    _instance = new AdvancedCache();
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
                
                if(_tempCache.Contains(key))
                {
                    result = (T)_tempCache[key];

                    //beolvassuk egy másik szálon a háttérben eladdig, ha még nem kérte senki
                    if (!_tempCache.Contains(GetReadFlagKey(key)))
                    {
                        System.Threading.ThreadPool.QueueUserWorkItem(o =>
                        {
                            ReaderImpl(key, acquire, cacheTime);
                        });
                    }
                }
                else
                {
                    //Első olvasás várakoztatni kell
                    result = ReaderImpl(key, acquire, cacheTime);
                }
                return result;
            }
        }
        private string GetReadFlagKey(string key)
        {
            return string.Format("{0}__________alreadyreading", key);
        }
        /// <summary>
        /// Az olvasás rész külön, hogy lehessen kétféleképpen használni
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="acquire"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        private T ReaderImpl<T>(string key, Func<T> acquire, int cacheTime)
        {

            //Egy flag az adott kulcsra, hogy már olvasás alatt van
            _tempCache.Set(new CacheItem(GetReadFlagKey(key), true), new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddYears(1)
            });

            T innerResult = acquire();
            
            
            if (innerResult != null)
            {
                var policy = new CacheItemPolicy();
                //setup callback
                policy.AbsoluteExpiration = DateTime.UtcNow + TimeSpan.FromMinutes(cacheTime);
                _cache.Set(new CacheItem(key, innerResult), policy);
                //Egy backup példányt átmenetileg itt tárolunk, hogy a háttérlekérés ideéig a régit visszaadhassuk
                _tempCache.Set(new CacheItem(key, innerResult), new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddYears(1)
                });
            }
            //törölni az olvasó flag-et
            _tempCache.Remove(GetReadFlagKey(key));

            return innerResult;
        }
        /// <summary>
        /// Callback amikor a fő cacheből egy-egy elem törlődik
        /// </summary>
        /// <param name="arg"></param>
        private void ItemRemoved(CacheEntryRemovedArguments arg)
        {
            
        }
    }
}