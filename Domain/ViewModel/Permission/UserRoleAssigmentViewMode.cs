namespace Domain.ViewModel.Permission;

public class UserRoleAssignmentViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public List<RoleViewModel> AllRoles { get; set; } = new();
}

public class RoleViewModel
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public bool IsSelected { get; set; }
}