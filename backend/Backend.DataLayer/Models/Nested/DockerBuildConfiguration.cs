using System.Collections.Generic;

namespace Backend.DataLayer.Models
{
    public class DockerBuildConfiguration
    {
        public string Context { get; set; }
        public string Dockerfile { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}