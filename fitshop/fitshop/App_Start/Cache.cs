using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fitshop.App_Start
{
    public class Cache
    {
        private static Cache _instance;
        private Dictionary<string, object> _cache;

        public object this[string key]
        {
            get
            {
                if (Contains(key))
                    return _cache[key];

                throw new NullReferenceException();
            }
        }

        private Cache()
        {
            _cache = new Dictionary<string, object>();
        }

        public static Cache GetInstance()
        {
            if (_instance == null)
                _instance = new Cache();

            return _instance;
        }

        public void Add(string key, object obj)
        {
            if (key != null && obj != null)
                _cache.Add(key, obj);
        }

        public void Remove(string key)
        {
            if (_cache.ContainsKey(key))
                _cache.Remove(key);
        }

        public bool Contains(string key)
        {
            return _cache.ContainsKey(key);
        }
    }
}