using System;
using System.ComponentModel.DataAnnotations;

namespace TASKFLOW_AI.Models._4_AI_Integration
{
    public class AIInsight
    {
        [Key] public int InsightId { get; set; }
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        [MaxLength(50)] public string InsightType { get; set; } = string.Empty;
        [Required] public string Content { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
    }
}