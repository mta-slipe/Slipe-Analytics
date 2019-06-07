using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlipeAnalyticsServer
{
    class AnalyticsEntry
    {
        [Key]
        public int Id { get; set; }

        public string Project { get; set; }
        public string Command { get; set; }
        public string IpHash { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
