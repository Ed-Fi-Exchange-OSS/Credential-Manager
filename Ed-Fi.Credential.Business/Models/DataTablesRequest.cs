using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ed_Fi.Credential.Business.Models
{
    public class DataTablesRequest
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public DataTablesSearch search { get; set; }
        public DataTablesOrder[] order { get; set; }
        public DataTablesColumn[] columns { get; set; }
    }

    public class DataTablesOrder
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class DataTablesColumn
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public DataTablesSearch search { get; set; }


    }

    public class DataTablesSearch
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
}