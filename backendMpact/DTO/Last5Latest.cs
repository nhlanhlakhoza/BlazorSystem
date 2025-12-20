namespace backendMpact.DTO
{
    public class Last5Latest
    {

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AgentId { get; set; }       
        public string AssignedBy { get; set; }    
        public DateTime DueDate { get; set; }
        public string Priority { get; set; } = string.Empty;
    }
}
