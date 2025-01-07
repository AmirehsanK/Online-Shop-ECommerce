namespace Domain.ViewModel.Permission;

public class RolePermissionsViewModel
{ 
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PermissionSelectionViewModel> Permissions { get; set; } = new List<PermissionSelectionViewModel>();
}
