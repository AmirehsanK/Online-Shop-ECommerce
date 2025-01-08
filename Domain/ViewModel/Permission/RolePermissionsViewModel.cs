using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Permission;

public class RolePermissionsViewModel
{ 
        public int RoleId { get; set; }
        [Required(ErrorMessage = "نام نقش الزامی است")]
        [MaxLength(50)]
        public string RoleName { get; set; }
        public List<PermissionSelectionViewModel> Permissions { get; set; } = new List<PermissionSelectionViewModel>();
        
}
