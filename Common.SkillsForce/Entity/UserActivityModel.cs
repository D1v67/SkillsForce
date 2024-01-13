using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.Entity
{
    public class UserActivityModel
    {
        public int UserActivityID { get; set; }
        public int UserID { get; set; }
        public string CurrentRole { get; set; }
        public string UrlVisited { get; set; }
        public string HttpMethod { get; set; }
        public string ActionParameters { get; set; }
        public string IpAddress { get; set; }
        public DateTime UrlVisitedTimestamp { get; set; }


        // Additional Fields
        public string UserAgent { get; set; }
        public string SessionID { get; set; }
        public string Referer { get; set; }
        public int? StatusCode { get; set; }
        public string UserLocation { get; set; }
        public bool? IsMobileDevice { get; set; }


    }
}
