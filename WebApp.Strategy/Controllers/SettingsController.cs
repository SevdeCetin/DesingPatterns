using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SettingsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            Settings settings = new();
            if (User.Claims.Where(x => x.Type == Settings.claimDatabaseType).FirstOrDefault() != null)
            {
                settings.DatabaseType = (EDatabaseType)int.Parse(User.Claims.First(x => x.Type == Settings.claimDatabaseType).Value);


            }
            else
            {
                settings.DatabaseType = settings.GetDefaultDatabaseType;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeDatabase(int databaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var newClaim = new Claim(Settings.claimDatabaseType, databaseType.ToString());

            var claims = await _userManager.GetClaimsAsync(user);
            var hasDatabaseTyepClaim = claims.FirstOrDefault(x => x.Type == Settings.claimDatabaseType);
            if (hasDatabaseTyepClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, hasDatabaseTyepClaim, newClaim);

            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }

            await _signInManager.SignOutAsync();

            var authenticateResult = await HttpContext.AuthenticateAsync();

            await _signInManager.SignInAsync(user,authenticateResult.Properties);

            return RedirectToAction(nameof(Index));



        }
    }
}
