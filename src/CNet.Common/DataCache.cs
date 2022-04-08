using Microsoft.Extensions.Caching.Memory;
using System;

namespace CNet.Common
{
	/// <summary>
	/// 缓存相关的操作类
    /// Copyright (C) Maticsoft
	/// </summary>
	public class DataCache
	{
		/// <summary>
		/// 获取当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <returns></returns>
		public static object GetCache(string CacheKey)
		{
			IMemoryCache objCache = new MemoryCache(new MemoryCacheOptions());
			return objCache.Get(CacheKey);
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <param name="objObject"></param>
		public static void SetCache(string CacheKey, object objObject)
		{
			IMemoryCache objCache = new MemoryCache(new MemoryCacheOptions());
			objCache.Set(CacheKey, objObject);
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <param name="objObject"></param>
		public static void SetCache(string CacheKey, object objObject, MemoryCacheEntryOptions memoryCacheEntryOptions)
		{
			IMemoryCache objCache = new MemoryCache(new MemoryCacheOptions());
			objCache.Set(CacheKey, objObject, memoryCacheEntryOptions);
		}

        public static void Remove(object CacheKey) 
        {
			IMemoryCache objCache = new MemoryCache(new MemoryCacheOptions());
		    objCache.Remove(CacheKey);   
        }
	}
}
