using ProjectManagement.Domain.Entities.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Service.DTOs.User
{
    public class UserForUpdateDTO
    {
        public required string Email { get; set; }
        public  string? Name { get; set; }
        public  string? Password { get; set; }
        public  string? Surname { get; set; }
        public  string? PhoneNumber { get; set; }
        public List<int>? TeamId { get; set; }
    }
}
