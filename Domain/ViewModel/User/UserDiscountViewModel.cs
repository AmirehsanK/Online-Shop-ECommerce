namespace Domain.ViewModel.User;

public class UserDiscountViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public string Code { get; set; }
    public DateTime EndDate { get; set; }
}