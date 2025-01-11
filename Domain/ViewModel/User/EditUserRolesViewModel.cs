namespace Domain.ViewModel.User;

using System.Collections.Generic;

public class EditUserRolesViewModel
{ 
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<string> AssignedRoles { get; set; } = [];
        public List<string> AllRoles { get; set; } = [];
        public List<string> SelectedRoles { get; set; } = [];
}
