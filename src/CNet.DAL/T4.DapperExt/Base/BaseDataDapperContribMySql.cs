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
using MySql.Data.MySqlClient;
using CNet.Model;

namespace CNet.DAL
{
    public partial class BaseDataDapperContribMySql<T> : IBaseData<T> where T : class, new()
    {
        //public BaseDataDapperContribMySql() 
        //{
        
        //}
		public BaseDataDapperContribMySql(string connStr)
		{
            this.ConnStr = connStr;
		}
		public string ConnStr { get;  set; }

		/// <summary>
		/// 插入
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public long Insert(T model)
        {
            dynamic r = null;
            using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
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
        public bool InsertBatch(List<T> models)
        {
            try
            {
                using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
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
            catch (Exception ex)
            {

                return false;
            }
        }



        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(T model)
        {
            dynamic r = null;
            using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
            {
                cn.Open();
                r = cn.Update(model);
                cn.Close();
            }

            return r;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool UpdateBatch(List<T> models)
        {
            try
            {
                using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
                {
                    cn.Open();
                    foreach (var model in models)
                    {
                        cn.Update(model);
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
        public dynamic Delete(T model)
        {
            dynamic r = null;
            using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
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
        public bool DeleteByWhere(string where, object param = null)
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
                using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
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
        public bool DeleteBatch(List<T> models)
        {
            try
            {
                using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
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
        public T Get(object id)
        {
            T t = default(T); //默认只对int guid主键有作用除非使用ClassMapper
            using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
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
        public T Get(object id, string keyName)
        {
            var tableName = typeof(T).Name;
            StringBuilder sql = new StringBuilder().AppendFormat("SELECT   * FROM {0} WHERE {1}=@id Limit 1 ", tableName, keyName);
            var pms = new { id = id };
            using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
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
        public List<T> GetList(string where, string sort = null, int limits = -1, string fileds = " * ")
        {
            var tableName = typeof(T).Name;
            StringBuilder sql = new StringBuilder().AppendFormat("SELECT " + fileds + "  FROM {0}  ",
                tableName);
            if (!string.IsNullOrEmpty(where))
            {
                sql.AppendFormat(" where {0} ", where);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                sql.AppendFormat(" order by {0} ", sort);
            }
            if (limits>0)
            {
                sql.AppendFormat($" limit {limits} ");
            }

            using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
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
        public PageDateRes<T> GetPage(string where, string sort, int page, int resultsPerPage, string fields = "*", Type result = null)
        {
            var tableName = typeof(T).Name;
            var p = new DynamicParameters();
            p.Add("@p_table_name", tableName);
            p.Add("@p_fields", fields);
            p.Add("@p_page_size", resultsPerPage);
            p.Add("@p_page_now", page);
            p.Add("@p_order_string", sort);
            p.Add("@p_where_string", where);
            p.Add("@p_out_rows", 0, direction: ParameterDirection.Output);

            using (MySqlConnection cn = new MySqlConnection(this.ConnStr))
            {

                var data = cn.Query<T>("pr_pager", p, commandType: CommandType.StoredProcedure, commandTimeout: 120);
               // int totalPage = p.Get<int>("@TotalPage");
                int totalrow = p.Get<int>("@p_out_rows");

                var rep = new PageDateRes<T>()
                {
                    code =ResCode.Success,
                    count = totalrow,
                    totalPage = totalrow%resultsPerPage==0 ? totalrow/ resultsPerPage: totalrow / resultsPerPage+1,
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
        
        public bool ChangeSotpStatus(string where)
        {
            var tableName = typeof(T).Name;
            string sql = "UPDATE "+tableName+" SET StopFlag =1 ";
            if (string.IsNullOrWhiteSpace(where))
            {
                return false;
            }

            sql += " where " + where;

            return new DapperHelperMySql(this.ConnStr).Excute(sql) > 0;
        }
    }
}