using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisXML
{
    public class RedisCommand
    {
        public string MainCommand { get; set; } = string.Empty; //hset
        public List<RedisParameters> Parameters { get; set; } = new(); //key field value
    }

    public class RedisParameters
    {
        public string Key { get; set; } = string.Empty;
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
