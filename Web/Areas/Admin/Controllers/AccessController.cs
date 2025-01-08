using Application.Services.Interfaces;
using Domain.Entities.Permission;
using Domain.Interface;
using Domain.ViewModel.Permission;
using Domain.ViewModel.User;
using Infra.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

public class AccessController(IPermissionService permissionService,IUserService userService ) : AdminBaseController
{
    public async Task<IActionResult> Index(FilterUserWithRolesViewModel filter)
    {
        var users = await permissionService.GetUsersWithRolesAsync(filter);
        return View(users);
    }
    public async Task<IActionResult> RoleList()
    {
        var roles = await permissionService.GetAllRolesAsync();
        return View(roles);
    }
    [HttpPost]
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
    public async Task<IActionResult> EditRole(int id)
    {
        var role = await permissionService.GetRoleByIdAsync(id);
        if (role == null)
        {
            return NotFound();
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

    private void MarkSelectedPermissions(List<PermissionSelectionViewModel> permissions, List<int> selectedPermissionIds)
    {
        foreach (var permission in permissions)
        {
            permission.IsSelected = selectedPermissionIds.Contains(permission.PermissionId);
            if (permission.Children.Any())
            {
                MarkSelectedPermissions(permission.Children, selectedPermissionIds);
            }
        }
    }
    [HttpPost]
    public async Task<IActionResult> SaveRole(RolePermissionsViewModel viewModel)
    {
        await permissionService.UpdateRoleAsync(viewModel);

        return RedirectToAction("RoleList");
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
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
    [HttpPost]
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

            if (selectedRoles == null || !selectedRoles.Any())
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

        return RedirectToAction(nameof(AssignRolesToUser), new { userId = model.UserId });
    }
    public async Task<IActionResult> AssignRolesToUser(int? userId)
    {
        var users = await userService.GetAllUsersForRolesAsync();
        ViewBag.Users = users;

        var model = new UserRoleAssignmentViewModel();
        if (userId.HasValue)
        {
            var userWithRoles = await permissionService.GetUserWithRolesAsync(userId.Value);
            if (userWithRoles == null)
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
}