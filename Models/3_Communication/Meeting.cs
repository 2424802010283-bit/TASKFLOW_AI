using System;
using System.ComponentModel.DataAnnotations;

namespace TASKFLOW_AI.Models._3_Communication
{
    public class Meeting
    {
        [Key] public int MeetingId { get; set; }
        public int ProjectId { get; set; }
        [Required, MaxLength(200)] public string Title { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [MaxLength(255)] public string MeetingUrl { get; set; } = null!;
    }
}