﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace T4
{
    //以下代码请勿修改
    public class MsSqlDbHelper : BaseDbHelper
    {
        public override string PreParameter
        {
            get
            {
                return "@";
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public override List<DbTable> GetDbTables()
        {
            string tables = Config.Tables;
            if (!string.IsNullOrEmpty(tables))
            {
                tables = string.Format(" and obj.name in ('{0}')", tables.Replace(",", "','"));
            }
            string views = Config.Tables;
            if (!string.IsNullOrEmpty(tables))
            {
                views = string.Format(" and v.[name] in ('{0}')", views.Replace(",", "','"));
            }
            #region SQL
            string sql = string.Format(@"SELECT
									obj.name tablename,
									schem.name schemname,
									idx.rows,
									CAST
									(
										CASE 
											WHEN (SELECT COUNT(1) FROM sys.indexes WHERE object_id= obj.OBJECT_ID AND is_primary_key=1) >=1 THEN 1
											ELSE 0
										END 
									AS BIT) HasPrimaryKey                                         
									from {0}.sys.objects obj 
									inner join {0}.dbo.sysindexes idx on obj.object_id=idx.id and idx.indid<=1
									INNER JOIN {0}.sys.schemas schem ON obj.schema_id=schem.schema_id
									where type='U' {1} ", Config.DbDatabase, tables);

            //cts 添加
            sql += string.Format(@" UNION ALL								
                select v.[name] as tablename, s.[name] as schemname, 0 rows,'0' HasPrimaryKey from sys.views as v,sys.schemas as s where v.schema_id = s.schema_id
                and s.[name] = 'dbo' {0}", views);
            #endregion
            DataTable dt = GetDataTable(sql);
            return dt.Rows.Cast<DataRow>().Select(row => new DbTable
            {
                TableName = ToCamelCase(row.Field<string>("tablename")),
                SchemaName = row.Field<string>("schemname"),
                Rows = row.Field<int>("rows"),
                HasPrimaryKey = row.Field<bool>("HasPrimaryKey")
            }).ToList();
        }

        public override List<DbColumn> GetDbColumns(string tableName, string schema = "dbo")
        {

            #region SQL
            string sql = string.Format(@"
                                    WITH indexCTE AS
                                    (
	                                    SELECT 
                                        ic.column_id,
                                        ic.index_column_id,
                                        ic.object_id    
                                        FROM {0}.sys.indexes idx
                                        INNER JOIN {0}.sys.index_columns ic ON idx.index_id = ic.index_id AND idx.object_id = ic.object_id
                                        WHERE  idx.object_id =OBJECT_ID(@tableName) AND idx.is_primary_key=1
                                    )
                                    select
									colm.column_id ColumnID,
                                    CAST(CASE WHEN indexCTE.column_id IS NULL THEN 0 ELSE 1 END AS BIT) IsPrimaryKey,
                                    colm.name ColumnName,
                                    systype.name ColumnType,
                                    colm.is_identity IsIdentity,
                                    colm.is_nullable IsNullable,
                                    cast(colm.max_length as int) ByteLength,
                                    (
                                        case 
                                            when systype.name='nvarchar' and colm.max_length>0 then colm.max_length/2 
                                            when systype.name='nchar' and colm.max_length>0 then colm.max_length/2
                                            when systype.name='ntext' and colm.max_length>0 then colm.max_length/2 
                                            else colm.max_length
                                        end
                                    ) CharLength,
                                    cast(colm.precision as int) Precision,
                                    cast(colm.scale as int) Scale,
                                    prop.value Remark
                                    from {0}.sys.columns colm
                                    inner join {0}.sys.types systype on colm.system_type_id=systype.system_type_id and colm.user_type_id=systype.user_type_id
                                    left join {0}.sys.extended_properties prop on colm.object_id=prop.major_id and colm.column_id=prop.minor_id
                                    LEFT JOIN indexCTE ON colm.column_id=indexCTE.column_id AND colm.object_id=indexCTE.object_id                                        
                                    where colm.object_id=OBJECT_ID(@tableName)
                                    order by colm.column_id", Config.DbDatabase);
            #endregion
            SqlParameter param = new SqlParameter("@tableName", SqlDbType.NVarChar, 100) { Value = string.Format("{0}.{1}.{2}", Config.DbDatabase, schema, tableName) };
            DataTable dt = GetDataTable(sql, param);

            return DtColToList(dt, new MsSqlDbTypeMap());
        }

        //protected override List<DbColumn> DtColToList(DataTable dt, ISqlServerDbTypeMap sqlServerDbTypeMap)
        //{
        //    IEnumerable<DataRow> dr = dt.Rows.Cast<DataRow>();
        //    List<DbColumn> dbList = new List<DbColumn>();
        //    DbColumn dbCoModel = null;
        //    foreach (var row in dr)
        //    {
        //        string columnType = row.Field<string>("ColumnType");
        //        bool isNullable = row.Field<bool>("IsNullable");
        //        string cSharpType = sqlServerDbTypeMap.MapCsharpType(columnType);
        //        string changCSharpType = sqlServerDbTypeMap.ChangSqlSqDbTypeMap(cSharpType, isNullable);
        //        Type commonType = sqlServerDbTypeMap.MapCommonType(columnType);

        //        dbCoModel = new DbColumn()
        //        {
        //            ColumnID = int.Parse(row.Field<object>("ColumnID").ToString()),
        //            IsPrimaryKey = row.Field<bool>("IsPrimaryKey"),
        //            ColumnName = row.Field<string>("ColumnName"),
        //            ColumnType = row.Field<string>("ColumnType"),
        //            IsIdentity = row.Field<bool>("IsIdentity"),
        //            IsNullable = isNullable,
        //            ByteLength = int.Parse(row.Field<object>("ByteLength").ToString()),
        //            CharLength = int.Parse(row.Field<object>("CharLength").ToString()),
        //            Scale = int.Parse(row.Field<object>("Scale").ToString()),
        //            Remark = row["Remark"].ToString(),
        //            CSharpType = cSharpType,
        //            IsNullCSharpType = changCSharpType,
        //            CommonType = commonType
        //        };
        //        dbList.Add(dbCoModel);
        //    }
        //    return dbList;
        //}

        public override DataTable GetDataTable(string commandText, params IDataParameter[] parms)
        {
            using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.Parameters.AddRange(parms);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }

        public override string GetParStr(List<DbColumn> dbComList)
        {
            return BGetParStr(dbComList, PreParameter);
        }

    }

    public class MsSqlDbTypeMap : ISqlServerDbTypeMap
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
                case "smallint": csharpType = "short"; break;
                case "smallmoney": csharpType = "decimal"; break;
                case "sql_variant": csharpType = "object"; break;
                case "sysname": csharpType = "object"; break;
                case "text": csharpType = "string"; break;
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
