using System.Collections.Generic;

namespace Backend.DataLayer.Models
{
    public class DockerConfiguration
    {
        public string Image { get; set; }

        public DockerBuildConfiguration BuildConfiguration { get; set; }

        public string MountTarget { get; set; }

        public string Command { get; set; }
        
        public Dictionary<string, object> Properties { get; set; }
    }
}