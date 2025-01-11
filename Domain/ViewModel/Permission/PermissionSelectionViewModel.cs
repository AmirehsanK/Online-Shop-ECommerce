namespace Domain.ViewModel.Permission;

public class PermissionSelectionViewModel
{
    public int PermissionId { get; set; }
    public string DisplayName { get; set; }
    public bool IsSelected { get; set; }
    public int? ParentId { get; set; }
    public List<PermissionSelectionViewModel> Children { get; set; } = [];
    
}