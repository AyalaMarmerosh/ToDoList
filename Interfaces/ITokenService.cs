
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tasks.Services;
using Tasks.Models;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Tasks.Interfaces
{
    public interface ITokenService
    {
        SecurityToken GetToken(List<Claim> claims);
        TokenValidationParameters GetTokenValidationParameters();
        string WriteToken(SecurityToken token);
    }
}