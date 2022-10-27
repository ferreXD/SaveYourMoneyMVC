using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaveYourMoneyMVC.Entities;
using SaveYourMoneyMVC.Models.Account;
using SaveYourMoneyMVC.Models.Error;
using System.Net;
using Const = SaveYourMoneyMVC.Common.Constants.Contants;
using System.Security.Claims;

namespace SaveYourMoneyMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Gastos");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email, Password, RememberMe")] LoginModel model)
        {
            var dbEntity = await _userManager.FindByEmailAsync(model.Email);

            if (dbEntity == null)
            {
                string defaultLanguage = System.Threading.Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

                string title = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.UserNotFound_Es : Properties.Resources.UserNotFound_En;
                string message = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.UserNotFound_Message_Es : Properties.Resources.UserNotFound_Message_En;

                return RedirectToAction("Index", "Error", new ErrorViewModel() { StatusCode = HttpStatusCode.BadRequest, Title = title, Message = message, Url = "//Account/Login" });
            }

            var currentClaims = await _userManager.GetClaimsAsync(dbEntity);

            if(!currentClaims.Any(claim => claim.Type.Equals("Language"))){
                Claim languageClaim = new Claim("Language", dbEntity.Language);
                await _userManager.AddClaimAsync(dbEntity, languageClaim);
            }

            var result = await _signInManager.PasswordSignInAsync(dbEntity, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                string defaultLanguage = System.Threading.Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

                string title = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.InvalidPassword_Es : Properties.Resources.InvalidPassword_En;
                string message = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.InvalidPassword_Message_Es : Properties.Resources.InvalidPassword_Message_En;

                return RedirectToAction("Index", "Error", new ErrorViewModel() { StatusCode = HttpStatusCode.BadRequest, Title = title, Message = message, Url = "//Account/Login"});
            }

            return RedirectToAction("Index", "Gastos");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name, Email, Password, VerifiedPassword, Language, RememberMe")] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if(await _userManager.FindByEmailAsync(model.Email) != null)
            {
                string defaultLanguage = System.Threading.Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

                string title = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.RepeatedEmail_Es : Properties.Resources.RepeatedEmail_En;
                string message = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.RepeatedEmail_Message_Es : Properties.Resources.RepeatedEmail_Message_En;

                return RedirectToAction("Index", "Error", new ErrorViewModel() { StatusCode = HttpStatusCode.BadRequest, Title = title, Message = message, Url = "//Account/Register" });
            }

            var identityUser = new User { UserName = model.Name, Email = model.Email, Language = model.Language };
            var created = await _userManager.CreateAsync(identityUser, model.Password);

            if (!created.Succeeded)
            {

                string userLanguage = identityUser.Language;
                string title = String.Empty;
                string message = String.Empty;

                if (created.Errors.FirstOrDefault().Code.Equals("DuplicateUserName"))
                {
                    title = userLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.RepeatedUsername_Es : Properties.Resources.RepeatedUsername_En;
                    message = userLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.RepeatedUsername_Message_Es : Properties.Resources.RepeatedUsername_Message_En;

                    return RedirectToAction("Index", "Error", new ErrorViewModel() { StatusCode = HttpStatusCode.BadRequest, Title = title, Message = message, Url = "//Account/Register" });
                }
                else
                {
                    title = userLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.InternalServerError_Es : Properties.Resources.InternalServerError_En;
                    message = userLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.InternalServerError_Es : Properties.Resources.InternalServerError_Message_En;

                    return RedirectToAction("Index", "Error", new ErrorViewModel() { StatusCode = HttpStatusCode.InternalServerError, Title = title, Message = message, Url = "//Account/Register" });
                }
            }

            Claim languageClaim = new Claim("Language", identityUser.Language);
            await _userManager.AddClaimAsync(identityUser, languageClaim);

            var signedIn = await _signInManager.PasswordSignInAsync(identityUser, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);

            if (!signedIn.Succeeded)
            {
                string userLanguage = identityUser.Language;

                string title = userLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.InternalServerError_Es : Properties.Resources.InternalServerError_En;
                string message = userLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.InternalServerError_Es : Properties.Resources.InternalServerError_Message_En;

                return RedirectToAction("Index", "Error", new ErrorViewModel() { StatusCode = HttpStatusCode.InternalServerError, Title = title, Message = message, Url = "//Account/Register" });
            }
            
            return RedirectToAction("Index", "Gastos");
        }

        [HttpPost("ChangeLang")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeLang(string lang)
        {
            var dbEntity = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (dbEntity == null)
            {
                string defaultLanguage = System.Threading.Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

                string title = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.UserNotFound_Es : Properties.Resources.UserNotFound_En;
                string message = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.UserNotFound_Message_Es : Properties.Resources.UserNotFound_Message_En;

                return RedirectToAction("Index", "Error", new ErrorViewModel() { StatusCode = HttpStatusCode.BadRequest, Title = title, Message = message, Url = "//Account/Login" });
            }

            dbEntity.Language = lang;

            await _userManager.UpdateAsync(dbEntity);
            await AddUpdateClaim(dbEntity, "Language", lang);

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost("Logout/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string id)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            await _signInManager.SignOutAsync();

            var dbEntity = await _userManager.FindByNameAsync(id);

            if (dbEntity == null)
            {
                string defaultLanguage = System.Threading.Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

                string title = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.UserNotFound_Es : Properties.Resources.UserNotFound_En;
                string message = defaultLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.UserNotFound_Message_Es : Properties.Resources.UserNotFound_Message_En;

                return RedirectToAction("Index", "Error", new ErrorViewModel() { StatusCode = HttpStatusCode.BadRequest, Title = title, Message = message, Url = "//Account/Login" });
            }

            var result = await _userManager.DeleteAsync(dbEntity);

            if (!result.Succeeded)
            {
                string userLanguage = dbEntity.Language;

                string title = userLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.InternalServerError_Es : Properties.Resources.InternalServerError_En;
                string message = userLanguage.Equals(Const.LANGUAGE_ES) ? Properties.Resources.InternalServerError_Es : Properties.Resources.InternalServerError_Message_En;

                return RedirectToAction("Index", "Error", new ErrorViewModel() { StatusCode = HttpStatusCode.InternalServerError, Title = title, Message = message, Url = "//Account/Register" });
            }

            return RedirectToAction("Register", "Account");
        }

        private async Task<ClaimsPrincipal> AddUpdateClaim(User user, string key, string value)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            Claim newClaim = new Claim(key, value);
            Claim prevClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type.Equals("Language"));

            await _userManager.ReplaceClaimAsync(user, prevClaim, newClaim);

            var updatedClaims = await _userManager.GetClaimsAsync(user);

            var appIdentity = new ClaimsIdentity(
                updatedClaims,
                identity.AuthenticationType
            );

            return new ClaimsPrincipal(appIdentity);
        }
    }
}
