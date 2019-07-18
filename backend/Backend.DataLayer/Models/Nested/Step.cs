using System;
using System.Collections.Generic;

namespace Backend.DataLayer.Models
{
    public class Step
    {
        /// <summary>
        /// The number/order of the step inside the job
        /// </summary>
        public int Number { get; set; }
        
        /// <summary>
        /// Name of the Task
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Current state of the Task
        /// </summary>
        public ProcessingState State { get; set; }

        /// <summary>
        /// Processing start time
        /// </summary>
        public DateTime? Start { get; set; }

        /// <summary>
        /// Processing end time
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// Configuration of the docker container for task execution
        /// </summary>
        public DockerConfiguration Docker{ get; set; }

        public Dictionary<string, object> Metrics { get; set; }
    }
}