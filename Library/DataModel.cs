using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class DataModel
    {

        public class Rootobject
        {
            public string command { get; set; }
            public string result { get; set; }
            public string what { get; set; }
            public int found { get; set; }
            public Entry[] entries { get; set; }
        }

        public class Entry
        {
            public string _class { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string time { get; set; }
            public string lasttime { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string symbol { get; set; }
            public string srccall { get; set; }
            public string dstcall { get; set; }
            public string comment { get; set; }
            public string path { get; set; }
            public string altitude { get; set; }
            public string speed { get; set; }
            public string temp { get; set; }
            public string pressure { get; set; }
            public string humidity { get; set; }
            public string wind_direction { get; set; }
            public string wind_speed { get; set; }
        }

    }
}
