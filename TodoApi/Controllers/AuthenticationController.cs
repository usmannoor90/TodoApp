using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthenticationController(IConfiguration config)
    {
        _config = config;
    }

    public record AuthenticationData(string? UserName, string? Password);
    public record UserData(int Id, string FirstName, string LastName, string UserName);

    [HttpPost("token")]
    [AllowAnonymous]
    public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
    {
        var user = ValidateCredentials(data);
        if (user == null)
        {
            return Unauthorized();
        }
        try
        {
            var token = GenerateToken(user);
            if (token == null)
            {
                return NotFound("User is not found.");
            }
            return Ok(token);
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
            return ex.Message;
        }


    }

    private string GenerateToken(UserData user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey")!));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();

        claims.Add(new(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.UserName));
        claims.Add(new(JwtRegisteredClaimNames.GivenName, user.FirstName));
        claims.Add(new(JwtRegisteredClaimNames.FamilyName, user.LastName));

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer")!,
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(1),
            signingCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    private UserData? ValidateCredentials(AuthenticationData data)
    {
        // this is not a production code - replace it with the auth system.
        var SomePasswrod = "test123";
        var SomeUserName = "usman";
        if (CompareValue(data.Password!, SomePasswrod) && CompareValue(data.UserName!, SomeUserName))
        {

            return new UserData(1, "Usman", "Noor", "Usman Noor");
        }

        if (CompareValue(data.Password!, "SomePasswrod") && CompareValue(data.UserName!, "SomeUserName"))
        {

            return new UserData(1, "Usman", "Noor", "Usman Noor");
        }

        return null;

    }

    private bool CompareValue(string actual, string expected)
    {
        if (actual != null)
        {
            if (actual == expected)
            {
                return true;
            }

        }
        return false;
    }
}
