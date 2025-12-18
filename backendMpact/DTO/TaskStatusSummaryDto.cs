using backendMpact.Models;

namespace backendMpact.DTO
{
    public class TaskStatusSummaryDto
    {
        public List<TaskItem> Pending { get; set; } = new();
        public List<TaskItem> Completed { get; set; } = new();
        public List<TaskItem> Submitted { get; set; } = new();
        public List<TaskItem> Overdue { get; set; } = new();
    }
}
