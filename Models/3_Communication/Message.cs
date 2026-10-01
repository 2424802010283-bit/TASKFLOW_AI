using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TASKFLOW_AI.Models._1_Workspace;

namespace TASKFLOW_AI.Models._3_Communication
{
    public class Message
    {
        [Key] public int MessageId { get; set; }
        public int ChannelId { get; set; }
        public int SenderId { get; set; }
        [Required] public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        [ForeignKey("ChannelId")] public Channel Channel { get; set; } = null!;
        [ForeignKey("SenderId")] public User Sender { get; set; } = null!;
    }
}