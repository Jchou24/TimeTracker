using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.DAL;
using TimeTracker.DAL.DBModels.Auth;
using TimeTracker.Helper.Auth;
using TimeTracker.Helper.Extensions;
using TimeTracker.Models;

namespace TimeTracker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        protected readonly IUserAuthHandler<User, int> _authHandler;

        public AccountController(IUserAuthHandler<User, int> authHandler)
        {
            this._authHandler = authHandler;
        }

        [HttpGet]
        public bool IsLogin()
        { 
            return User.Identity.IsAuthenticated;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthenticationUser authenticationUser)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}

            var user = _authHandler.Find(x => x.Email == authenticationUser.Email).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("Fail", "Invalid account or password");
                return BadRequest(ModelState);
            }
            
            if (!SecurePasswordHasher.Verify(authenticationUser.Password, user.PasswordHash))
            {
                ModelState.AddModelError("Fail", "Invalid account or password");
                return BadRequest(ModelState);
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Guid.ToString()),
                    new Claim(ClaimTypes.Sid , user.Id.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                //ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(5),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                IssuedUtc = DateTimeOffset.UtcNow,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Regist([FromBody] RegistUser registUser)
        {
            User user = new User(registUser.Email, registUser.Password);

            switch (_authHandler.Create(user))
            {
                case DAL.Models.CreateAccountResult.Ok:
                    return Ok();
                case DAL.Models.CreateAccountResult.AccountExist:
                    ModelState.AddModelError("Fail", "Account exists!");
                    return BadRequest(ModelState);
                case DAL.Models.CreateAccountResult.Fail:
                default:
                    ModelState.AddModelError("Fail", "Invalid account or password!");
                    return BadRequest(ModelState);
            }            
        }

        [HttpGet]
        [Authorize]
        public UserInfo GetUserInfo()
        {
            int? id = User.GetUserId();
            if (id == null)
            {
                return null;
            }

            var user = _authHandler.FindById((int)id);
            if (user == null)
            {
                return null;
            }

            var userInfo = new UserInfo(user);
            return userInfo;
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateName([FromBody] UpdateUser updateUser)
        {
            int? id = User.GetUserId();
            if (id == null)
            {
                return BadRequest();
            }

            var user = _authHandler.FindById((int)id);
            if (user == null)
            {
                return BadRequest();
            }
            user.Name = updateUser.Name;
            _authHandler.Update(user);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdatePassword([FromBody] UpdatePassword updatePassword)
        {
            int? id = User.GetUserId();
            if (id == null)
            {
                return BadRequest();
            }

            var user = _authHandler.FindById((int)id);
            if (user == null)
            {
                return BadRequest();
            }

            if (!SecurePasswordHasher.Verify(updatePassword.CurrentPassword, user.PasswordHash))
            {
                ModelState.AddModelError("Fail", "Password is incorrect");
                return BadRequest(ModelState);
            }

            user.PasswordHash = SecurePasswordHasher.Hash(updatePassword.Password);
            _authHandler.Update(user);
            return Ok();
        }

        /// <summary>
        /// for refreshing session
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult KeepAlive()
        {
            return Ok();
        }
    }
}
