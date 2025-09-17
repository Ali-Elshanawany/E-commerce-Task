using Ecommerce.Application.DTOs;
using Ecommerce.Application.Helpers;
using Ecommerce.Application.IServices;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Services;

public class AuthService : IAuthService
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JWT _Jwt;

    public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
    {
        _userManager = userManager;
        _Jwt = jwt.Value;
    }

    public async Task<GenericResponseDto<AuthDto>> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
        {
            return new GenericResponseDto<AuthDto>()
            {
                Message = "Email is already exists",
                Data = new AuthDto() { },
                Status = 400
            };
        }

        if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
        {
            return new GenericResponseDto<AuthDto>()
            {
                Message = "UserName is already exists",
                Data = new AuthDto() { },
                Status = 400
            };
        }

        var user = new ApplicationUser()
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var err in result.Errors)
            {
                errors += $"{err.Description},";
            }

            return new GenericResponseDto<AuthDto>()
            {
                Message = errors,
                Data = new AuthDto() { },
                Status = 400
            };
        }
        var newUser = await _userManager.FindByEmailAsync(user.Email);

        var jwtSecurityToken = await GenerateJwtToken(user);

        var authDto = new AuthDto()
        {
            Email = user.Email,
            UserName = user.UserName,
            //ExpiresOn = jwtSecurityToken.ValidTo,
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),


        };
        var newRefreshToken = GenerateRefreshToken();
        authDto.RefreshToken = newRefreshToken.Token;
        authDto.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
        newUser.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(newUser);


        return new GenericResponseDto<AuthDto>()
        {
            Status = 200,
            Message = "Your Account Registered Successfully",
            Data = authDto
        };


    }

    public async Task<GenericResponseDto<AuthDto>> Login(LoginDto loginDto)
    {


        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return new GenericResponseDto<AuthDto>()
            {
                Message = "Email or Password is incorrect!",
                Status = 400,
            };
        }
        var jwtSecurityToken = await GenerateJwtToken(user);




        var authDto = new AuthDto()
        {
            IsAuthenticated = true,
            Email = user.Email,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            UserName = user.UserName,
            LastLogin = DateTime.UtcNow,
            //ExpiresOn = jwtSecurityToken.ValidTo,

        };

        if (user.RefreshTokens.Any(t => t.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            authDto.RefreshToken = activeRefreshToken.Token;
            authDto.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
        }
        else
        {
            var newRefreshToken = GenerateRefreshToken();
            authDto.RefreshToken = newRefreshToken.Token;
            authDto.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
        }


        return new GenericResponseDto<AuthDto>()
        {
            Message = "Successfully Logged",
            Status = 200,
            Data = authDto
        };



    }


    public async Task<GenericResponseDto<AuthDto>> RefreshTokenAsync(string token)
    {
        var authDto = new AuthDto();
        var response = new GenericResponseDto<AuthDto>();

        var user = await _userManager.Users.Include(a => a.RefreshTokens).SingleOrDefaultAsync
            (u => u.RefreshTokens.Any(t => t.Token == token));



        if (user is null)
        {
            response.Message = "Invalid Token";
            response.Status = 400;
            return response;
        }

        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
        {
            response.Message = "Invalid Token";
            response.Status = 400;
            return response;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);

        var jwtToken = await GenerateJwtToken(user);

        authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authDto.Email = user.Email;
        authDto.UserName = user.UserName;
        authDto.RefreshToken = newRefreshToken.Token;
        authDto.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

        response.Message = "Success";
        response.Status = 200;
        response.Data = authDto;


        return response;
    }


    private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {

        var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
        new Claim(JwtRegisteredClaimNames.Email,user.Email)
        };

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Jwt.Key));
        var signingCredentails = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var JwtSecurityToken = new JwtSecurityToken(
        issuer: _Jwt.Issuer,
        audience: _Jwt.Audience,
        //expires: DateTime.Now.AddDays(_Jwt.DurationInDays),
        expires: DateTime.Now.AddMinutes(1),
        claims: claims,
        signingCredentials: signingCredentails
        );
        return JwtSecurityToken;
    }

    private RefreshToken GenerateRefreshToken()
    {

        var randomNumber = new byte[32];

        using var generator = new RNGCryptoServiceProvider();

        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow,
        };

    }


}
