using Application.Services.Interfaces;
using Domain.Entities.Permission;
using Domain.ViewModel.Permission;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

public class AccessController(IPermissionService permissionService) : AdminBaseController
{
    public async Task<IActionResult> AddOrEdit(int? id)
    {
        var viewModel = new RolePermissionsViewModel();

        viewModel.Permissions = await permissionService.GetPermissionsHierarchyAsync();

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
            if (viewModel.RoleId == 0)
            {
                await permissionService.AddRoleAsync(viewModel);
            }
            else
            {
                await permissionService.UpdateRoleAsync(viewModel);
            }

            return RedirectToAction(nameof(Index));
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
}