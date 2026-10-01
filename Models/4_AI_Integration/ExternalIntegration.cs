using System.ComponentModel.DataAnnotations;

namespace TASKFLOW_AI.Models._4_AI_Integration
{
    public class ExternalIntegration
    {
        [Key] public int IntegrationId { get; set; }
        public int WorkspaceId { get; set; }
        [Required, MaxLength(50)] public string Provider { get; set; } = string.Empty;
        [MaxLength(255)] public string ApiKey { get; set; } = string.Empty;
        [MaxLength(255)] public string WebhookUrl { get; set; } = string.Empty;
    }
}