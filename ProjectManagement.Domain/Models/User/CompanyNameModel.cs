using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Models.User
{
    public class CompanyNameModel
    {
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }


        public virtual CompanyNameModel MapFromEntity(string name, int id)
        {
            CompanyName = name;
            CompanyId = id;
            return this;
        }
    }
}
