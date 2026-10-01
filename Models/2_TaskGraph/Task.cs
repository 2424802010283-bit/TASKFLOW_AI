using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TASKFLOW_AI.Models._2_TaskGraph
{
    public class Task
    {
        [Key] public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public int? CreatorId { get; set; }
        [Required, MaxLength(200)] public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [MaxLength(20)] public string Priority { get; set; } = "MEDIUM";
        [MaxLength(50)] public string Status { get; set; } = "TODO";
        public DateTime? StartDate { get; set; }
        public DateTime? Deadline { get; set; }
        public decimal? EstimatedHours { get; set; }
        public decimal ActualHours { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [ForeignKey("ProjectId")] public Project Project { get; set; } = null!;
    }
}