namespace PensionMCP.Models
{
    public class Client
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
