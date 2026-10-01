using System;
using System.ComponentModel.DataAnnotations;

namespace TASKFLOW_AI.Models._3_Communication
{
    public class Channel
    {
        [Key] public int ChannelId { get; set; }
        public int WorkspaceId { get; set; }
        public int? ProjectId { get; set; }
        [Required, MaxLength(100)] public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}