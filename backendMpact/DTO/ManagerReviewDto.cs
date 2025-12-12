namespace backendMpact.DTO
{
    public class ManagerReviewDto
    {
        public bool IsApproved { get; set; }           
        public string Comments { get; set; } = string.Empty;
        public string ExternalRecipients { get; set; } = "";
    }
}
