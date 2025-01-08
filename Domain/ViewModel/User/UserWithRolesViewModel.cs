using Domain.Shared;

namespace Domain.ViewModel.User;

public class UserWithRolesViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } = [];
}
public class FilterUserWithRolesViewModel : Paging.BasePaging<UserWithRolesViewModel>
{
    public string UserName { get; set; }
    public string Email { get; set; }
}
