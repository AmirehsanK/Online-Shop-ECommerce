using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Permission;

public class RolePermissionMapping(int roleId, int permissionId) : BaseEntity
{
    #region Properties

    public int PermissionId { get; set; } = permissionId;

    public int RoleId { get; set; } = roleId;

    #endregion

    #region Relations

    [ForeignKey(nameof(RoleId))] public Role Role { get; set; }

    [ForeignKey(nameof(PermissionId))] public Permission Permission { get; set; }

    #endregion
}