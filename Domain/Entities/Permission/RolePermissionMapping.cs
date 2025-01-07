using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities.Permission;

public class RolePermissionMapping : BaseEntity
{
    public RolePermissionMapping(int roleId,int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
    #region Properties

    public int PermissionId { get; set; }

    public int RoleId { get; set; }
    
    #endregion

    #region Relations

    public Role Role { get; set; }

    [ForeignKey(nameof(PermissionId))]
    public Permission Permission { get; set; }
    
    #endregion
}