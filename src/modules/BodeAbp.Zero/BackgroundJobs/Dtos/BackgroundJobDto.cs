using System;
using Abp.Application.Services.Dto;
using Abp.BackgroundJobs;

namespace BodeAbp.Zero.BackgroundJobs.Dtos
{
    public class GetBackgroundJobListOutput : EntityDto<long>
    {
        /// <summary>
        /// Type of the job.
        /// It's AssemblyQualifiedName of job type.
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Job arguments as JSON string.
        /// </summary>
        public string JobArgs { get; set; }

        /// <summary>
        /// Try count of this job.
        /// A job is re-tried if it fails.
        /// </summary>
        public short TryCount { get; set; }

        /// <summary>
        /// Next try time of this job.
        /// </summary>
        //[Index("IX_IsAbandoned_NextTryTime", 2)]
        public DateTime NextTryTime { get; set; }

        /// <summary>
        /// Last try time of this job.
        /// </summary>
        public DateTime? LastTryTime { get; set; }

        /// <summary>
        /// This is true if this job is continously failed and will not be executed again.
        /// </summary>
        //[Index("IX_IsAbandoned_NextTryTime", 1)]
        public bool IsAbandoned { get; set; }

        /// <summary>
        /// Priority of this job.
        /// </summary>
        public BackgroundJobPriority Priority { get; set; }
    }
}
