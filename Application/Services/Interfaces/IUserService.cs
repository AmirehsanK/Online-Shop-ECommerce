using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ViewModel.User;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUserListAsync();
    }
}
