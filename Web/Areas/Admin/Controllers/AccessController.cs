using Application.Services.Interfaces;
using Domain.Entities.Permission;
using Domain.Interface;
using Domain.ViewModel.Permission;
using Domain.ViewModel.User;
using Infra.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;
using Infra.Data.Statics;

namespace Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.AccessManagement)]
public class AccessController(IPermissionService permissionService, IUserService userService) : AdminBaseController
{
    
    #region Role

    [InvokePermission(PermissionName.RoleList)]
    public async Task<IActionResult> RoleList()
    {
        var roles = await permissionService.GetAllRolesAsync();
        return View(roles);
    }

    [HttpPost]
    [InvokePermission(PermissionName.CreateRole)]
    public async Task<IActionResult> AddRole(string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
        {
            return BadRequest("نام نقش الزامی است.");
        }

        var role = new RolePermissionsViewModel() { RoleName = roleName };
        await permissionService.AddRoleAsync(role);
        return Ok();
    }

    [HttpPost]
    [InvokePermission(PermissionName.UpdateRole)]
    public async Task<IActionResult> SaveRole(RolePermissionsViewModel viewModel)
    {
        var validPermissions = viewModel.Permissions
            .Where(p => p.PermissionId > 0 && (p.ParentId == null || p.ParentId > 0))
            .ToList();
        viewModel.Permissions = validPermissions;
        await permissionService.UpdateRoleAsync(viewModel);

        return RedirectToAction("RoleList");
    }

    [InvokePermission(PermissionName.UpdateRole)]
    public async Task<IActionResult> EditRole(int id)
    {
        var role = await permissionService.GetRoleByIdAsync(id);
        if (role == null!)
        {
            return NotFound();
        }
        if (!await permissionService.CanEditOrDeleteRoleAsync(id))
        {
            TempData[ErrorMessage] = "شما مجاز به حذف این نقش نیستید.";
            return RedirectToAction(nameof(RoleList));
        }

        var permissions = await permissionService.GetPermissionsHierarchyAsync();
        var selectedPermissionIds = await permissionService.GetSelectedPermissionIdsAsync(id);

        var viewModel = new RolePermissionsViewModel
        {
            RoleId = role.Id,
            RoleName = role.RoleName,
            Permissions = permissions
        };

        MarkSelectedPermissions(viewModel.Permissions, selectedPermissionIds);

        return View(viewModel);
    }

    [HttpPost]
    [InvokePermission(PermissionName.DeleteRole)]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await permissionService.CanEditOrDeleteRoleAsync(id))
        {
            TempData[ErrorMessage] = "شما مجاز به حذف این نقش نیستید.";
            return RedirectToAction(nameof(RoleList));
        }

        try
        {
            await permissionService.SoftDeleteRoleAsync(id);
            TempData[SuccessMessage] = "نقش با موفقیت حذف شد.";
        }
        catch (Exception ex)
        {
            TempData[ErrorMessage] = "خطا در حذف نقش: " + ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region User

    [InvokePermission(PermissionName.UserRoleList)]
    public async Task<IActionResult> Index(FilterUserWithRolesViewModel filter)
    {
        var users = await permissionService.GetUsersWithRolesAsync(filter);
        return View(users);
    }

    [HttpPost]
    [InvokePermission(PermissionName.AssignRoleToUser)]
    public async Task<IActionResult> AssignRolesToUser(UserRoleAssignmentViewModel model)
    {
        try
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Model is null.");
            }

            if (model.UserId == 0)
            {
                throw new ArgumentException("User ID is not valid.");
            }

            var selectedRoles = model.AllRoles
                .Where(r => r.IsSelected)
                .Select(r => r.RoleName)
                .ToList();

            if (selectedRoles == null || selectedRoles.Count == 0)
            {
                throw new ArgumentException("No roles selected.");
            }

            await permissionService.UpdateUserRolesAsync(model.UserId, selectedRoles);
            TempData[SuccessMessage] = "نقش‌ها با موفقیت به کاربر اختصاص داده شدند.";
        }
        catch (Exception ex)
        {
            TempData[ErrorMessage] = "خطا در اختصاص نقش‌ها: " + ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [InvokePermission(PermissionName.AssignRoleToUser)]
    public async Task<IActionResult> AssignRolesToUser(int? userId)
    {
        var users = await userService.GetAllUsersForRolesAsync();
        ViewBag.Users = users;

        var model = new UserRoleAssignmentViewModel();
        if (userId.HasValue)
        {
            var userWithRoles = await permissionService.GetUserWithRolesAsync(userId.Value);
            if (userWithRoles == null!)
            {
                TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد.";
                return RedirectToAction(nameof(AssignRolesToUser));
            }

            model.UserId = userWithRoles.UserId;
            model.UserName = userWithRoles.UserName;

            var allRoles = await permissionService.GetAllRolesAsync();
            model.AllRoles = allRoles.Select(r => new RoleViewModel
            {
                RoleId = r.Id,
                RoleName = r.RoleName,
                IsSelected = userWithRoles.Roles.Contains(r.RoleName)
            }).ToList();
        }
        else
        {
            var allRoles = await permissionService.GetAllRolesAsync();
            model.AllRoles = allRoles.Select(r => new RoleViewModel
            {
                RoleId = r.Id,
                RoleName = r.RoleName,
                IsSelected = false
            }).ToList();
        }

        return View(model);
    }

    [InvokePermission(PermissionName.AssignRoleToUser)]
    public async Task<IActionResult> EditUserRoles(int userId)
    {
        var userWithRoles = await permissionService.GetUserWithRolesAsync(userId);
        if (userWithRoles == null!)
        {
            TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد.";
            return RedirectToAction(nameof(Index));
        }

        var allRoles = await permissionService.GetAllRolesAsync();
        ViewBag.AllRoles = allRoles;

        return View(userWithRoles);
    }

    [HttpPost]
    [InvokePermission(PermissionName.AssignRoleToUser)]
    public async Task<IActionResult> UpdateUserRoles(int userId, List<string> selectedRoles)
    {
        try
        {
            if (selectedRoles == null! || selectedRoles.Count == 0)
            {
                TempData[ErrorMessage] = "هیچ نقشی انتخاب نشده است.";
                return RedirectToAction(nameof(EditUserRoles), new { userId });
            }

            await permissionService.UpdateUserRolesAsync(userId, selectedRoles);
            TempData[SuccessMessage] = "نقش‌ها با موفقیت به‌روزرسانی شدند.";
        }
        catch (Exception ex)
        {
            TempData[ErrorMessage] = "خطا در به‌روزرسانی نقش‌ها: " + ex.Message;
        }

        return RedirectToAction(nameof(Index), new { userId });
    }

    #endregion

    #region Permission

    private static void MarkSelectedPermissions(List<PermissionSelectionViewModel> permissions, List<int> selectedPermissionIds)
    {
        foreach (var permission in permissions)
        {
            permission.IsSelected = selectedPermissionIds.Contains(permission.PermissionId);
            if (permission.Children.Count != 0)
            {
                MarkSelectedPermissions(permission.Children, selectedPermissionIds);
            }
        }
    }

    #endregion
    
}