namespace Domain.ViewModel.Discount;

public class DiscountListAdminViewModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public bool IsPercentage { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; } = true;
}