using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TASKFLOW_AI.Models._1_Workspace;

namespace TASKFLOW_AI.Models._2_TaskGraph
{
    public class Project
    {
        [Key] public int ProjectId { get; set; }
        public int WorkspaceId { get; set; }
        [Required, MaxLength(150)] public string Name { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? Deadline { get; set; }
        public decimal Progress { get; set; } = 0;
        [MaxLength(50)] public string Status { get; set; } = "PLANNING";
        [ForeignKey("WorkspaceId")] public Workspace Workspace { get; set; } = null!;
    }
}