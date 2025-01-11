using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Permission;

public class UserRoleMapping : BaseEntity
{
    #region Properties

    public int RoleId { get; set; }

    public int UserId { get; set; }

    #endregion

    #region Relations

    [ForeignKey(nameof(RoleId))] public Role Role { get; set; }

    [ForeignKey(nameof(UserId))] public User User { get; set; }

    #endregion
}