﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
//using DapperExtensions;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using T4;
using CNet.Model;
using System.Data.SQLite;

namespace CNet.DAL
{
    public partial class BaseDataDapperContribSQLite<T> : IBaseData<T> where T : class ,new()
    {
		
		public BaseDataDapperContribSQLite(string connStr)
		{
			this.ConnStr = connStr;
		}
		public string ConnStr { get; private set; }
		/// <summary>
		/// 插入
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public  long Insert(T model)
        {
            dynamic r = null;
            using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
            {
                cn.Open();
                r = cn.Insert(model);
                cn.Close();
            }

            return r;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public  bool InsertBatch(List<T> models)
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
                {
                    cn.Open();
                    foreach (var model in models)
                    {
                        cn.Insert(model);
                    }
                    cn.Close();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }



        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public  bool Update(T model)
        {
            dynamic r = null;
            using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
            {
                cn.Open();
                r = cn.Update<T>(model);
                cn.Close();
            }

            return r;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public   bool UpdateBatch(List<T> models)
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
                {
                    cn.Open();
                    foreach (var model in models)
                    {
                        cn.Update<T>(model);
                    }
                    cn.Close();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        /// <summary>
        ///根据实体删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public  dynamic Delete(T model)
        {
            dynamic r = null;
            using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
            {
                cn.Open();
                r = cn.Delete(model);
                cn.Close();
            }

            return r;
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public  bool DeleteByWhere(string where, object param = null)
        {
            try
            {
                var tableName = typeof(T).Name;
                StringBuilder sql = new StringBuilder().AppendFormat(" Delete FROM {0} ", tableName);
                if (string.IsNullOrEmpty(where))
                {
                    return false;
                }

                sql.AppendFormat(" where {0} ", where);
                using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
                {
                    cn.Execute(sql.ToString(), param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
          
        }
        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public  bool DeleteBatch(List<T> models)
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
                {
                    cn.Open();
                    foreach (var model in models)
                    {
                        var r = cn.Delete(model);
                    }
                    cn.Close();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        /// <summary>
        /// 根据一个实体对象
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public  T Get(object id)
        {
            T t = default(T); //默认只对int guid主键有作用除非使用ClassMapper
            using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
            {
                cn.Open();
                t = cn.Get<T>(id);
                cn.Close();
            }

            return t;

        }

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public  T Get(object id, string keyName)
        {
            var tableName = typeof(T).Name;
            StringBuilder sql = new StringBuilder().AppendFormat("SELECT  * FROM {0} WHERE {1}=@id Limit 1 ", tableName, keyName);
            var pms = new { id = id };
            using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
            {
                return cn.Query<T>(sql.ToString(), pms).FirstOrDefault();
            }

        }

      

        /// <summary>
        /// 根据条件查询实体列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public  List<T> GetList(string where, string sort = null, int limits = -1, string fileds = " * ")
        {
            var tableName = typeof(T).Name;
            StringBuilder sql = new StringBuilder().AppendFormat("SELECT "  + fileds + "  FROM {0}  ",
                tableName);
            if (!string.IsNullOrEmpty(where))
            {
                sql.AppendFormat(" where {0} ", where);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                sql.AppendFormat(" order by {0} ", sort);
            }
			if (limits > 0)
			{
				sql.AppendFormat($" limit {limits} ");
			}

			using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
            {
                return cn.Query<T>(sql.ToString()).ToList();
            }

        }





        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="sort">分类</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="fields">查询字段</param>
        /// <returns></returns>
        public  PageDateRes<T> GetPage(string where, string sort, int page, int resultsPerPage, string fields = "*", Type result = null)
        {
            var tableName = typeof(T).Name;
            string sqlCount = @$" select count(1) from {tableName} where {where}";
            string sqlPager = @$" select  * from {tableName}  where {where} ORDER BY {sort} LIMIT {page-1}*{resultsPerPage},{resultsPerPage}";


			using (SQLiteConnection cn = new SQLiteConnection(this.ConnStr))
            {

                int totalrow =Convert.ToInt32(cn.ExecuteScalar(sqlCount));
                var data = cn.Query<T>(sqlPager); ;

                var rep = new PageDateRes<T>()
                {
                    code =ResCode.Success,
                    count = totalrow,
                    totalPage = (int)Math.Ceiling(totalrow/Convert.ToDouble(resultsPerPage)),
                    data = data.ToList(),
                    pageNum = page,
                    pageSize = resultsPerPage
                };

                return rep;
            }
        }

        /// <summary>
        /// 修改删除状态
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        
        public  bool ChangeSotpStatus(string where)
        {
            var tableName = typeof(T).Name;
            string sql = "UPDATE "+tableName+" SET StopFlag =1 ";
            if (string.IsNullOrWhiteSpace(where))
            {
                return false;
            }

            sql += " where " + where;

            return new DapperHelperSQLite(this.ConnStr).Excute(sql) > 0;
        }
	}
}