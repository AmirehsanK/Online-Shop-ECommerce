using Domain.ViewModel.User.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminPanelViewModel> GetAdminPanelAsync();
    }
}
