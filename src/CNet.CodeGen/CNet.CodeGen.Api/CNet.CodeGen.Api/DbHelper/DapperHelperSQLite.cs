using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SQLite;

namespace CNet.CodeGen.DB
{
    public  class DapperHelperSQLite: IDapperHelper
    {
		//private static  readonly string connStr = AppConfigurtaionServices.Configuration.GetConnectionString("connMySql");
		//ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
		//public static string ConnStr { get { return connStr; } set; }

		public DapperHelperSQLite(string connStr) {
            this.ConnStr = connStr;

		}

        public  string ConnStr { get; private set; }

        /// <summary>
        /// Excute返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public  int Excute(string sql, object param=null, 
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (IDbConnection conn = new SQLiteConnection(this.ConnStr))
            {
                return conn.Execute(sql, param, transaction, commandTimeout, commandType);
            }
          
        }

        /// <summary>
        ///返回table数据
        /// </summary>
        /// <returns></returns>
        public  DataTable ExecuteReaderToTable(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (IDbConnection conn = new SQLiteConnection(this.ConnStr))
            {
                var reader = conn.ExecuteReader(sql, param, transaction, commandTimeout, commandType);
                DataTable tb = new DataTable();
                tb.Load(reader);
                return tb;
            }
        }

        /// <summary>
        /// 查询返回list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public  List<T> Query<T>(string sql, object param=null,
           IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (IDbConnection conn = new SQLiteConnection(this.ConnStr))
            {
                return conn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType).ToList();
            }
        }
		
		   /// <summary>
        /// 查询返回第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public  T QueryFirst<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (IDbConnection conn = new SQLiteConnection(this.ConnStr))
            {
                return conn.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 查询返回多个列表结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public  SqlMapper.GridReader QueryMultiple(string sql, object param=null,
         IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (IDbConnection conn = new SQLiteConnection(this.ConnStr))
            {
                return conn.QueryMultiple(sql, param,transaction,commandTimeout,commandType);
            }
        }

        /// <summary>
        /// 返回受影响第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public  object ExecuteScalar(string sql, object param=null,
           IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (IDbConnection conn = new SQLiteConnection(this.ConnStr))
            {
                return conn.ExecuteScalar(sql, param, transaction, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 符合条件的第一行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public  T FirstOrDefault<T>(string sql, object param=null) where T : class
        {
            return Query<T>(sql,param).FirstOrDefault();
        }


        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="proName"></param>
        /// <returns></returns>
        public  bool ExecTransaction(Dictionary<string, object> dic, List<string> proName = null)
        {
            using (IDbConnection conn = new SQLiteConnection(this.ConnStr))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in dic)
                        {
                            if (proName != null)
                            {
                                conn.Execute(item.Key, item.Value, transaction: transaction,
                                    commandType: (proName.Contains(item.Key) ? CommandType.StoredProcedure : CommandType.Text));
                            }
                            else
                            {
                                conn.Execute(item.Key, item.Value, transaction: transaction);
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            }
        }

      
    }
}