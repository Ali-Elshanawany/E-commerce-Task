using Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.IServices;

public interface IAuthService
{
    Task<GenericResponseDto<AuthDto>> RegisterAsync(RegisterDto registerDto);
    Task<GenericResponseDto<AuthDto>> Login(LoginDto loginDto);

    Task<GenericResponseDto<AuthDto>> RefreshTokenAsync(string refreshToken);
}
