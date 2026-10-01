using System;
using System.ComponentModel.DataAnnotations.Schema;
using TASKFLOW_AI.Models._1_Workspace;

namespace TASKFLOW_AI.Models._2_TaskGraph
{
    public class TaskAssignee
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        [ForeignKey("TaskId")] public Task Task { get; set; } = null!;
        [ForeignKey("UserId")] public User User { get; set; } = null!;
    }
}