﻿using Microsoft.AspNetCore.Identity;

namespace laptrinhweb2.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
