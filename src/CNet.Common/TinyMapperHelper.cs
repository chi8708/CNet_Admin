using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Common
{
    public static class TinyMapperHelper
    {
        public static TDestination TinyMapTo<TSource, TDestination>(this TSource source, bool isBind = true) where TSource : class, new()
        {
            if (isBind)
            {
                TinyMapperBind<TSource, TDestination>();
            }
            return TinyMapper.Map<TDestination>(source);
        }

        public static IList<TDestination> TinyMapToList<TSource, TDestination>(this IEnumerable<TSource> source, bool isBind = true) where
           TSource : class, new()
        {
            if (isBind)
            {
                TinyMapperBind<TSource, TDestination>();
            }
            IList<TDestination> tResult = new List<TDestination>();
            
            foreach (TSource item in source)
            {
                tResult.Add(TinyMapper.Map<TDestination>(item));
            }

            return tResult;
        }

        public static void TinyMapperBind<TSource, TDestination>()
        {
            TinyMapper.Bind<TSource, TDestination>();
        }

        //public static TDestination MapTo<TDestination>(this object source)
        //{
        //    return TinyMapper.Map<TDestination>(source);
        //}

        //public static IList<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source) where
        //    TSource : class,new() 
        //{
        //    IList<TDestination> tResult = new List<TDestination>();
        //    foreach (var item in source)
        //    {
        //        tResult.Add(item.MapTo<TDestination>());
        //    }

        //    return tResult;
        //}
    }
}
