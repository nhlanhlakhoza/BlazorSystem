using System;

namespace backendMpact.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string AgentId { get; set; }            // Inspector
        public string AssignedBy { get; set; }         // Manager

        public DateTime DueDate { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = "pending";

        public string InspectorNotes { get; set; } = string.Empty;
        public string PdfFile { get; set; } = string.Empty;
       
        public string ImagePathsJson { get; set; } = "[]";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        
        public string ManagerComments { get; set; } = string.Empty;
        public DateTime? ReviewedAt { get; set; }
        public bool? IsApproved { get; set; }

        public string ExternalRecipients { get; set; } = "";
    }
}
