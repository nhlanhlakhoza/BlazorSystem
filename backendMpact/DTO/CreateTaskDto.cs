namespace backendMpact.DTO
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AgentId { get; set; }       // Inspector
        public string AssignedBy { get; set; }    // Manager
        public DateTime DueDate { get; set; }
        public string Priority { get; set; } = string.Empty;
    }
}
