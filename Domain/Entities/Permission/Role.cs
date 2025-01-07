using Domain.Entities.Common;

namespace Domain.Entities.Permission;

public class Role : BaseEntity
{
    #region Properties

    public string RoleName { get; set; }

    #endregion

    #region Relations

    public ICollection<UserRoleMapping> UserRoleMappings { get; set; }
    public ICollection<RolePermissionMapping> RolePermissionMappings { get; set; }

    #endregion
}