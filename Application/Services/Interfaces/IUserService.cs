using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ViewModel.User.Admin;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListViewModel>> GetUserListAsync();
    }
}
