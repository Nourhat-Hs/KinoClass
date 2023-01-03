using APIkino.Data;
using KinoClass.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Password = APIkino.Tools.Password;

namespace ApiSecurity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{


   private readonly IConfiguration _conf;
    private Context _context;

    public AuthenticationController(Context context, IConfiguration conf)
    {
        _context = context;
        _conf = conf;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register(UserDTO Request)
    {
        try
        {
            var dbUser = await _context.Users.Where(u => u.Username == Request.Username).FirstOrDefaultAsync();
            if (dbUser != null)
            {
                return BadRequest("username exsist already");
            }

            //hash the password
            Request.Password = Password.PasswordHashing(Request.Password);

            User user = new User
            {
                Username = Request.Username,
                Password = Request.Password,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to register {ex.Message}");
        }

    }







    //logginn 
    [HttpPost("logginn")]
    [AllowAnonymous]
    public async Task<ActionResult> logginn(UserDTO request)
    {
        try
        {
            string Innpassword = Password.PasswordHashing(request.Password);
            var dbUser = await _context.Users.Where(u => u.Username == request.Username && u.Password == Innpassword).FirstOrDefaultAsync();

            if (dbUser == null)
            {
                return BadRequest("username or password are incorect");
            }
            List<Claim> authClaim = new List<Claim>
            {
                new Claim("UserId",dbUser.Id.ToString()),
                new Claim(ClaimTypes.Name, dbUser.Username),
                new Claim("UserName", dbUser.Username)
            };


            //get our token 
            var token = this.GetToken(authClaim);

            //return our token 
            return Ok(new
            {
                token =new JwtSecurityTokenHandler().WriteToken(token),
                Exception= token.ValidTo
            });
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to login {ex.Message}");
        }
    }



    //create our token 
    private JwtSecurityToken GetToken(List<Claim>AuthClaim)
    {
        //get the signing key
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["JWT:Secret"]));


        //create our tokens
        var token = new JwtSecurityToken(
            issuer: _conf["JWT:ValidIssuer"],
            audience: _conf["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(30),
            claims: AuthClaim,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            ) ;
        return token;
        //now you can create your token in the log in
    }

}
