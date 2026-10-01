using System;
using System.ComponentModel.DataAnnotations;

namespace TASKFLOW_AI.Models._1_Workspace
{
    public class User
    {
        [Key] public int UserId { get; set; }
        //public string? PhoneNumber { get; set; }
        [Required, MaxLength(100)] public string FullName { get; set; } = string.Empty;
        [Required, MaxLength(100), EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, MaxLength(255)] public string PasswordHash { get; set; } = string.Empty;
        [MaxLength(500)] public string Skills { get; set; } = string.Empty;
        public int WorkloadCapacity { get; set; } = 40; 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}