using System;
using System.ComponentModel.DataAnnotations;

namespace TASKFLOW_AI.Models._4_AI_Integration
{
    public class ContactForm
    {
        [Key] public int FormId { get; set; }
        [MaxLength(100)] public string SubmitterName { get; set; } = string.Empty;
        [Required, MaxLength(100), EmailAddress] public string Email { get; set; } =string.Empty;
        [Required] public string Message { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public bool IsProcessed { get; set; } = false;
    }
}