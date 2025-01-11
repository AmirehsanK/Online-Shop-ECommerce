using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Permission;

public class Permission:BaseEntity
{
    public int? ParentId { get; set; }
    public string UniqueName { get; set; }
    public string DisplayName { get; set; }

    #region Relations
    [ForeignKey(nameof(ParentId))]
    public Permission Parent { get; set; }
    public ICollection<Permission> Children { get; set; }
    public ICollection<Role> Role { get; set; }
    public ICollection<RolePermissionMapping> RolePermissionMappings { get; set; }
    
    #endregion
}