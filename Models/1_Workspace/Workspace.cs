using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TASKFLOW_AI.Models._1_Workspace
{
    public class Workspace
    {
        [Key]
        public int WorkspaceId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public int OwnerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User Owner { get; set; } = null!;

        public ICollection<WorkspaceMember> Members { get; set; }
            = new List<WorkspaceMember>();
    }
}