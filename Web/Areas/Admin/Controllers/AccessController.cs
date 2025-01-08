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
    public async Task<IActionResult> AddOrEdit(int? id)
    {
        var viewModel = new RolePermissionsViewModel
        {
            Permissions = await permissionService.GetPermissionsHierarchyAsync()
        };

        if (id.HasValue)
        {
            var role = await permissionService.GetRoleByIdAsync(id.Value);
            if (role == null)
            {
                return NotFound();
            }

            viewModel.RoleId = role.Id;
            viewModel.RoleName = role.RoleName;

            var selectedPermissionIds = await permissionService.GetSelectedPermissionIdsAsync(role.Id);

            MarkSelectedPermissions(viewModel.Permissions, selectedPermissionIds);
        }
        else
        {
            viewModel.RoleId = 0;
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SaveRole(RolePermissionsViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (!await permissionService.IsRoleNameUniqueAsync(viewModel.RoleName, viewModel.RoleId))
            {
                TempData[ErrorMessage] = "نام نقش باید منحصر به فرد باشد";
                viewModel.Permissions = await permissionService.GetPermissionsHierarchyAsync();
                return View(nameof(AddOrEdit), viewModel);
            }

            if (viewModel.RoleId == 0)
            {
                await permissionService.AddRoleAsync(viewModel);
            }
            else
            {
                await permissionService.UpdateRoleAsync(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        viewModel.Permissions = await permissionService.GetPermissionsHierarchyAsync();
        return View(nameof(AddOrEdit), viewModel);
    }

     private void MarkSelectedPermissions(List<PermissionSelectionViewModel> permissions, List<int> selectedPermissionIds)
    {
        foreach (var permission in permissions)
        {
            permission.IsSelected = selectedPermissionIds.Contains(permission.PermissionId);
            MarkSelectedPermissions(permission.Children, selectedPermissionIds);
        }
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