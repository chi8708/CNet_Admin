﻿namespace CNet.CodeGen.Api.Template
{
    public class TableModel
    {
        public string TableName { get; set; }
        public string TableRemark { get; set; }
        public List<DbColumn> Columns { get; set; }

        public string RootFunction { get; set; }
    }

}
