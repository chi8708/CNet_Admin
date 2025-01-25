﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace T4
{
	//以下代码请勿修改
	public class SQLiteDbHelper : BaseDbHelper
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

			#region SQL
			string sql = string.Format(@"  select * from sqlite_master obj where type IN('table','view') {0} ", tables);

			#endregion
			DataTable dt = GetDataTable(sql);
			return dt.Rows.Cast<DataRow>().Select(row => new DbTable
			{
				TableName = row.Field<string>("name"),
				SchemaName = "main",
				Rows = row.Field<int>("rootpage"),
				HasPrimaryKey = true
			}).ToList();
		}

		public override List<DbColumn> GetDbColumns(string tableName, string schema = "main")
		{

			#region SQL
			string sql = string.Format(@"
                                    WITH all_tables AS (SELECT name FROM sqlite_master WHERE type = 'table') 
										SELECT at.name table_name,
                                         pti.cid as ColumnID,pti.pk IsPrimaryKey, pti.name as ColumnName,pti.pk as IsIdentity,
										 (case 
										 when instr(pti.type,'(')>0 then
										 SUBSTR(pti.type,0,instr(pti.type,'(')) 
										 ELSE pti.type
										 end) 
										as ColumnType 
										,pti.[NOTNULL] as IsNullable,NULL as ByteLength,NULL as CharLength,NULL as Scale,NULL as Remark
										FROM all_tables at INNER JOIN pragma_table_info(at.name) pti where LOWER(table_name)=LOWER('{0}')", tableName);
			#endregion
			DataTable dt = GetDataTable(sql);

			return DtColToList(dt, new SQLiteDbTypeMap());
		}


		public override DataTable GetDataTable(string commandText, params IDataParameter[] parms)
		{
			using (SQLiteConnection connection = new SQLiteConnection(Config.ConnectionString))
			{
				SQLiteCommand command = new SQLiteCommand(commandText, connection);
				command.Parameters.AddRange(parms);
				SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);

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

	public class SQLiteDbTypeMap : ISqlServerDbTypeMap
	{
		public string MapCsharpType(string dbtype)
		{
			if (string.IsNullOrEmpty(dbtype)) return dbtype;
			dbtype = dbtype.ToLower();
			string csharpType = "object";
			switch (dbtype)
			{
				case "integer": csharpType = "int"; break;
                case "int": csharpType = "int"; break;
                case "real": csharpType = "decimal"; break;
				case "text": csharpType = "string"; break;
				case "nchar": csharpType = "string"; break;
				case "nvarchar": csharpType = "string"; break;
				case "char": csharpType = "string"; break;
				case "varchar": csharpType = "string"; break;
				case "boolean": csharpType = "bool"; break;
                case "bit": csharpType = "bool"; break;
                case "datetime":csharpType = "DateTime"; break;
				default: csharpType = "object";break;
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
				case "integer": commonType = typeof(int) ; break;
                case "int": commonType = typeof(int); break;
                case "real": commonType = typeof(decimal); break;
				case "text": commonType = typeof(string); break;
				case "nchar": commonType = typeof(string); break;
				case "nvarchar": commonType = typeof(string); break;
				case "char": commonType = typeof(string); break;
				case "varchar": commonType = typeof(string); break;
				case "boolean": commonType = typeof(bool); break;
                case "bit": commonType = typeof(bool); break;
                case "datetime": commonType = typeof(DateTime); break;
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
