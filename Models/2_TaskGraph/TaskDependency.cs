using System.ComponentModel.DataAnnotations.Schema;

namespace TASKFLOW_AI.Models._2_TaskGraph
{
    public class TaskDependency
    {
        public int PrecedingTaskId { get; set; }
        public int SucceedingTaskId { get; set; }
        [ForeignKey("PrecedingTaskId")] public Task PrecedingTask { get; set; } = null!;
        [ForeignKey("SucceedingTaskId")] public Task SucceedingTask { get; set; } = null!;
    }
}