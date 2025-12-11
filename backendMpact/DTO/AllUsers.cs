namespace backendMpact.DTO
{
    public class AllUsers
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
