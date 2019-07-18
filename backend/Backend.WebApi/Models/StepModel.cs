using Backend.DataLayer.Models;

namespace Backend.WebApi.Models
{
    public class StepModel
    {
        public string Name { get; set; }

        public DockerConfiguration Docker { get; set; }
    }
}