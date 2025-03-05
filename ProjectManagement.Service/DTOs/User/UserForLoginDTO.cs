using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Service.DTOs.User
{
    public class UserForLoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
