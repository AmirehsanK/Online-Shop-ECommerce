namespace Domain.ViewModel.User.Admin;

public class UserDetailViewModel
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public required string Email { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsActive { get; set; }
}