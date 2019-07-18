using System;

namespace Backend.DataLayer.Models
{
    public class Fragment : BaseModel
    {
        public Guid JobId { get; set; }
        public int StepNumber { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public FragmentType Type { get; set; } 
    }
}