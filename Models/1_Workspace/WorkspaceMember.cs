using System;
using System.ComponentModel.DataAnnotations;

namespace TASKFLOW_AI.Models._1_Workspace
{
    public class WorkspaceMember
    {
        public int WorkspaceId { get; set; }

        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "Member";

        public decimal PerformanceScore { get; set; } = 0;

        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public Workspace Workspace { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}