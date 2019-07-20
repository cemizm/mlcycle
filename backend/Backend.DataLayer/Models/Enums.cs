namespace Backend.DataLayer.Models
{
    /// <summary>
    /// The initiator of the job
    /// </summary>
    public enum JobInitiator
    {
        /// <summary>
        /// Manually initiated
        /// </summary>
        Manual = 0,
        /// <summary>
        /// Initiated by git
        /// </summary>
        Git = 1,
        /// <summary>
        /// Initiated by data updates
        /// </summary>
        Data = 2,
    }

    public enum ProcessingState
    {
        /// <summary>
        /// Initial state when a job or step is created
        /// </summary>
        Created = 0,

        /// <summary>
        /// Step and job
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// Job/step is done.
        /// </summary>
        Done = 2,

        /// <summary>
        /// Step is scheduled for beeing processed  next
        /// </summary>
        Scheduled = 21,

        /// <summary>
        /// An error occured while processing the Job/step
        /// </summary>
        Error = 31,
    }

    public enum FragmentType
    {
        Metrics = 0,
        BuildLogs = 1,
        Model = 2
    }
}