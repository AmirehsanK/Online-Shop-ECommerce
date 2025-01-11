using Domain.ViewModel.User.Admin;

namespace Application.Services.Interfaces;

public interface IAdminService
{
    #region Get Admin Panel

    Task<AdminPanelViewModel> GetAdminPanelAsync();

    #endregion
}