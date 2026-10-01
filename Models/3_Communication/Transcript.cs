using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TASKFLOW_AI.Models._3_Communication
{
    public class Transcript
    {
        [Key] public int TranscriptId { get; set; }
        public int MeetingId { get; set; }
        public DateTime Timestamp { get; set; }
        [Required] public string Content { get; set; } =string.Empty;
        [ForeignKey("MeetingId")] public Meeting Meeting { get; set; } = null!;
    }
}