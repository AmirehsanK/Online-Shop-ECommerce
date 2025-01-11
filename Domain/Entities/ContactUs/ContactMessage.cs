namespace Domain.Entities.ContactUs;

public class ContactMessage
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string Subject { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FullName { get; set; }
    public bool IsAnswered { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? AdminResponse { get; set; }

    public DateTime? RespondedAt { get; set; }
}