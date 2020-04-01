using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace NDrop
{
    public class BroadcastResponse
    {
        public string height;
        public string txhash;
        public string raw_log;
        public List<Log> logs;

        public bool IsSuccess
        {
            get
            {
                if (logs == null || logs.Count == 0) return false;
                return logs[0].success;
            }            
        }

        public Error GetError()
        {
            if (IsSuccess) return new Error() { code = 0, message = "success" };

            var jObj = JObject.Parse(raw_log);

            return new Error()
            {
                code = Convert.ToInt32(jObj["code"]),
                message = jObj["message"].ToString()
            };
        }

        public class Error
        {
            public int code;
            public string message;
        }

        public class Log
        {
            public int msg_index;
            public bool success;
            public string log;
            public List<Event> events;

            public class Event
            {
                public string type;
                public List<Attribute> attributes;

                public class Attribute
                {
                    public string key;
                    public string value;
                }
            }
        }
    }
}
