namespace backendMpact.DTO
{
    public class InspectorUpdateTaskDto
    {
        public string Notes { get; set; } = string.Empty;
        public IFormFile? PdfFile { get; set; }
        // unlimited images
        public List<IFormFile>? Images { get; set; }
    }
}
