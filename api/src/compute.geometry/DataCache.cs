﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace compute.geometry
{
    static class DataCache
    {
        public static object GetCachedItem(JToken token, Type objectType, JsonSerializer serializer)
        {
            string jsonString = token.ToString();
            if (jsonString.StartsWith("{") &&
                jsonString.EndsWith("}") &&
                jsonString.IndexOf("url", StringComparison.OrdinalIgnoreCase)>0 &&
                jsonString.IndexOf("http", StringComparison.OrdinalIgnoreCase)>0)
            {
                Dictionary<string, string> cacheDictionary = new Dictionary<string, string>();
                cacheDictionary = token.ToObject(cacheDictionary.GetType()) as Dictionary<string, string>;
                string url;
                if (cacheDictionary == null || !cacheDictionary.TryGetValue("url", out url))
                    return null;

                JToken jtoken = null;
                string key = $"url:{url.ToLower()}";
                Tuple<JToken, object> cacheEntry = System.Runtime.Caching.MemoryCache.Default.Get(key) as Tuple<JToken, object>;
                if( cacheEntry!=null)
                {
                    Rhino.Geometry.GeometryBase geometry = cacheEntry.Item2 as Rhino.Geometry.GeometryBase;
                    if (geometry != null)
                        return geometry.DuplicateShallow();
                    jtoken = cacheEntry.Item1;
                }
                if (jtoken == null)
                {
                    using (var client = new System.Net.WebClient())
                    {
                        string cacheString = client.DownloadString(url);
                        object data = string.IsNullOrWhiteSpace(cacheString) ? null : Newtonsoft.Json.JsonConvert.DeserializeObject(cacheString);
                        var ja = data as Newtonsoft.Json.Linq.JArray;
                        jtoken = ja[0];
                    }
                }

                if( jtoken!=null )
                {
                    object rc = null;
                    if (serializer == null)
                        rc = jtoken.ToObject(objectType);
                    else
                        rc = jtoken.ToObject(objectType, serializer);

                    cacheEntry = new Tuple<JToken, object>(jtoken, rc);
                    System.Runtime.Caching.MemoryCache.Default.Add(key, cacheEntry, CachePolicy);
                    Rhino.Geometry.GeometryBase geometry = rc as Rhino.Geometry.GeometryBase;
                    if (geometry != null)
                        return geometry.DuplicateShallow();
                    return rc;
                }
            }
            return null;
        }

        private static System.Runtime.Caching.CacheItemPolicy CachePolicy
        {
            get
            {
                var policy = new System.Runtime.Caching.CacheItemPolicy();
                // no policy yet, but we could do things like evict after 2 weeks with
                //policy.SlidingExpiration = new TimeSpan(14, 0, 0, 0);
                return policy;
            }
        }
    }
}
