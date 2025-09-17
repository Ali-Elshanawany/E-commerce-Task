using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public DateTime LastLoginTime { get; set; }

    public List<RefreshToken>? RefreshTokens { get; set; }

}
