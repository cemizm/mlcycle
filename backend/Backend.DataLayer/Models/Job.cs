using System;
using System.Collections.Generic;

namespace Backend.DataLayer.Models
{
    public class Job : BaseModel
    {
        public Guid ProjectId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }
        public JobInitiator Initiator { get; set; }
        public ProcessingState State { get; set; }
        public List<Step> Steps { get; set; }
    }
}