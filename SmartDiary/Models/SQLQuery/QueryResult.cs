using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDiary.Models.SQLQuery
{
    public class QueryResult
    {
        public QueryResult()
        {

        }
        public bool Succeeded { get; set; }
        public IEnumerable<QueryError> Errors { get; set; }
    }
}
