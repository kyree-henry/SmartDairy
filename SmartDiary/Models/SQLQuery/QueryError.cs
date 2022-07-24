using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDiary.Models.SQLQuery
{
    public class QueryError
    {
        public QueryError()
        {

        }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}
