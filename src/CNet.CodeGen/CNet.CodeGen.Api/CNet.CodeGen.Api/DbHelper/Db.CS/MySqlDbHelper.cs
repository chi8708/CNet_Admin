using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace CNet
{
    //以下代码请勿修改
    public class MySqlDbHelper : BaseDbHelper
    {
        public override string PreParameter
        {
            get
            {
                return "?";
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public override List<DbTable> GetDbTables()
        {
            string tables = Config.Tables;
            #region SQL
            if (!string.IsNullOrEmpty(tables))
            {
                tables = string.Format(" and a.TABLE_NAME in ('{0}')", tables.Replace(",", "','"));
            }

            string sql = string.Format(@"SELECT a.TABLE_SCHEMA schemname,a.TABLE_NAME tablename,a.TABLE_ROWS `rows`,CAST(!ISNULL(b.HasPrimaryKey) AS SIGNED) HasPrimaryKey
                        FROM information_schema.`TABLES` AS a
                        LEFT JOIN (
	                        SELECT TABLE_NAME, COUNT(COLUMN_NAME) AS HasPrimaryKey
	                        FROM information_schema.key_column_usage
	                        WHERE CONSTRAINT_NAME = 'PRIMARY' AND TABLE_SCHEMA = '{0}' GROUP BY TABLE_NAME
                        ) AS b ON a.TABLE_NAME = b.TABLE_NAME
                        WHERE a.TABLE_SCHEMA = '{0}' {1}", Config.DbDatabase, tables);
            #endregion
            DataTable dt = GetDataTable(sql);
            return dt.Rows.Cast<DataRow>().Select(row => new DbTable
            {
                TableName = row.Field<string>("tablename"),
                SchemaName = row.Field<string>("schemname"),
                Rows = row.Field<object>("rows")==null?0:Convert.ToInt32(row.Field<object>("rows")),
                HasPrimaryKey = row.Field<Int64>("HasPrimaryKey") > 0
            }).ToList();
        }

        public override List<DbColumn> GetDbColumns(string tableName, string schema = "dbo")
        {
            #region SQL
            string sql = string.Format(@"SELECT ORDINAL_POSITION  ColumnID,COLUMN_KEY='PRI' IsPrimaryKey,COLUMN_NAME ColumnName,
                                DATA_TYPE ColumnType,EXTRA='auto_increment' IsIdentity,IS_NULLABLE='YES' IsNullable,
                                IF(CHARACTER_MAXIMUM_LENGTH IS NULL,0,CHARACTER_MAXIMUM_LENGTH ) ByteLength,
                                IF(CHARACTER_OCTET_LENGTH IS NULL,0,CHARACTER_OCTET_LENGTH ) CharLength,
                                IF(NUMERIC_SCALE IS NULL,0,NUMERIC_SCALE ) Scale,
                                COLUMN_COMMENT Remark FROM information_schema.`COLUMNS` 
                                where TABLE_SCHEMA='{0}'and TABLE_NAME='{1}'", Config.DbDatabase, tableName);
            #endregion
            DataTable dt = GetDataTable(sql);
            return DtColToList(dt, new MySqlDbTypeMap());
        }

        public override DataTable GetDataTable(string commandText, params IDataParameter[] parms)
        {
            using (var connection = new MySqlConnection(Config.ConnectionString))
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                adapter.Dispose();
                command.Dispose();
                connection.Close();
                return dt;
            }
        }

        public override string GetParStr(List<DbColumn> dbComList)
        {
            return BGetParStr(dbComList, PreParameter);
        }

    }

    public class MySqlDbTypeMap : ISqlServerDbTypeMap
    {

        public string MapCsharpType(string dbtype)
        {
            if (string.IsNullOrEmpty(dbtype)) return dbtype;
            dbtype = dbtype.ToLower();
            string csharpType = "object";
            switch (dbtype)
            {
                case "bigint": csharpType = "long"; break;
                case "binary": csharpType = "byte[]"; break;
                case "bit": csharpType = "bool"; break;
                case "char": csharpType = "string"; break;
                case "date": csharpType = "DateTime"; break;
                case "datetime": csharpType = "DateTime"; break;
                case "datetime2": csharpType = "DateTime"; break;
                case "datetimeoffset": csharpType = "DateTimeOffset"; break;
                case "dityint": csharpType = "bool"; break;
                case "decimal": csharpType = "decimal"; break;
                case "float": csharpType = "double"; break;
                case "image": csharpType = "byte[]"; break;
                case "int": csharpType = "int"; break;
                case "money": csharpType = "decimal"; break;
                case "nchar": csharpType = "string"; break;
                case "ntext": csharpType = "string"; break;
                case "numeric": csharpType = "decimal"; break;
                case "nvarchar": csharpType = "string"; break;
                case "real": csharpType = "Single"; break;
                case "smalldatetime": csharpType = "DateTime"; break;
                case "smallint": csharpType = "int"; break;
                case "smallmoney": csharpType = "decimal"; break;
                case "sql_variant": csharpType = "object"; break;
                case "sysname": csharpType = "object"; break;
                case "text": csharpType = "string"; break;
                case "longtext": csharpType = "string"; break;
                case "time": csharpType = "TimeSpan"; break;
                case "timestamp": csharpType = "byte[]"; break;
                case "tinyint": csharpType = "byte"; break;
                case "uniqueidentifier": csharpType = "Guid"; break;
                case "varbinary": csharpType = "byte[]"; break;
                case "varchar": csharpType = "string"; break;
                case "xml": csharpType = "string"; break;
                default: csharpType = "object"; break;
            }
            return csharpType;
        }

        public Type MapCommonType(string dbtype)
        {
            if (string.IsNullOrEmpty(dbtype)) return Type.Missing.GetType();
            dbtype = dbtype.ToLower();
            Type commonType = typeof(object);
            switch (dbtype)
            {
                case "bigint": commonType = typeof(long); break;
                case "binary": commonType = typeof(byte[]); break;
                case "bit": commonType = typeof(bool); break;
                case "char": commonType = typeof(string); break;
                case "date": commonType = typeof(DateTime); break;
                case "datetime": commonType = typeof(DateTime); break;
                case "datetime2": commonType = typeof(DateTime); break;
                case "datetimeoffset": commonType = typeof(DateTimeOffset); break;
                case "dityint": commonType = typeof(Boolean); break;
                case "decimal": commonType = typeof(decimal); break;
                case "float": commonType = typeof(double); break;
                case "image": commonType = typeof(byte[]); break;
                case "int": commonType = typeof(int); break;
                case "money": commonType = typeof(decimal); break;
                case "nchar": commonType = typeof(string); break;
                case "ntext": commonType = typeof(string); break;
                case "numeric": commonType = typeof(decimal); break;
                case "nvarchar": commonType = typeof(string); break;
                case "real": commonType = typeof(Single); break;
                case "smalldatetime": commonType = typeof(DateTime); break;
                case "smallint": commonType = typeof(short); break;
                case "smallmoney": commonType = typeof(decimal); break;
                case "sql_variant": commonType = typeof(object); break;
                case "sysname": commonType = typeof(object); break;
                case "text": commonType = typeof(string); break;
                case "time": commonType = typeof(TimeSpan); break;
                case "timestamp": commonType = typeof(byte[]); break;
                case "tinyint": commonType = typeof(byte); break;
                case "uniqueidentifier": commonType = typeof(Guid); break;
                case "varbinary": commonType = typeof(byte[]); break;
                case "varchar": commonType = typeof(string); break;
                case "xml": commonType = typeof(string); break;
                default: commonType = typeof(object); break;
            }
            return commonType;
        }

        public string ChangSqlSqDbTypeMap(string csharp, bool isNullable)
        {
            string ret = csharp;
            if (!"string".Equals(csharp) && isNullable)
            {
                return ret + "?";
            }
            else
            {
                return ret;
            }
        }
    }

}
