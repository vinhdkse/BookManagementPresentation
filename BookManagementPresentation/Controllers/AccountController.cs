using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BookManagementPresentation.Models;
using BookManagementPresentation.Model;
using System.IO;
using System.Data.Entity;

namespace BookManagementPresentation.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [Authorize]
        public ActionResult Information(string User)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MyUser my = db.MyUsers.SingleOrDefault(s => s.Email == User);
            return View(my);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Information([Bind(Include = "ID,Name,DateOfBirth,Gender,Email")]MyUser model, HttpPostedFileBase Img, FormCollection f)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string check = f["checkChange"].ToString();
            if (check.Equals("NO"))
            {
                model.Image = f["noChange"].ToString();
            }
            else
            {
                var allowedExtensions = new[] {
                ".Jpg", ".png", ".jpg", "jpeg",".JPG",".PNG",".JPEG"
                                              };
                var fileName = Path.GetFileName(Img.FileName);
                var ext = Path.GetExtension(Img.FileName);
                if (allowedExtensions.Contains(ext))
                {
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    model.Image = "img/" + fileName;
                    Img.SaveAs(path);

                }
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            Session["FullName"] = model.Name;
            Session["Image"] = model.Image;
            return RedirectToAction("Information", new { User = model.Email });

        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    ApplicationDbContext db = new ApplicationDbContext();
                    MyUser my = db.MyUsers.SingleOrDefault(s => s.Email == model.Email);
                    Session["FullName"] = my.Name;
                    Session["Image"] = my.Image;
                    var re = db.DonDatHangs.Where(s => s.CusUsername == my.Id&&s.isCanceled==false).Sum(s => s.Total);
                    
                    
                    if (re != null)
                    {
                        double total = double.Parse(re.Value.ToString());
                        if (total >= 20000000)
                        {
                            Session["Vip"] = "Yes";
                        }
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    ViewBag.Invalid = "Wrong many time, If you forgor your password, Please click Forgot Password. Try again after 5 minutes";
                    return View(model);
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ViewBag.Invalid = "Invalid Email or password";
                    return View(model);
            }
        }
        [Authorize]
        public ActionResult ChangePassword(string Id, string Email)
        {
            ViewData["Id"] = Id;
            ViewData["Email"] = Email;
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(FormCollection f)
        {
            string id = f["Id"];
            string oldpass = f["OldPassword"];
            string newpass = f["Password"];
            var check = UserManager.ChangePassword(id, oldpass, newpass);
            if (check.Succeeded)
            {
                return RedirectToAction("/Home/Index");
            }
            ViewBag.Invalid = "Invalid password";
            ViewData["Id"] = f["Id"];
            ViewData["Email"] = f["Email"];
            return View();

        }


        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user.Id, "Member");
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                MyUser myUser = new MyUser();
                myUser.Email = model.Email;
                myUser.Id = user.Id;
                myUser.Name = model.Name;
                myUser.Serect = model.Serect;
                myUser.Image = "img/placeholders/avatars/avatar10.jpg";
                ApplicationDbContext dbContext = new ApplicationDbContext();
                dbContext.MyUsers.Add(myUser);
                dbContext.SaveChanges();
                Session["FullName"] = myUser.Name;
                Session["Image"] = myUser.Image;
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
               // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "This Email is existed! Please choose another one";

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }


        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.MyUsers.SingleOrDefault(s => s.Email == model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed

                //return View("ForgotPasswordConfirmation");
                ViewBag.Error = "Tài khoản của bạn không tồn tại. Vui lòng đăng kí";
                return View();

            }

            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            if (model.Choosen.Equals("1"))
            {
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code, check = "", Email = user.Email, error = "" }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            else
            {
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                return RedirectToAction("ResetPassword", new { userId = user.Id, code = code, Email = user.Email, check = "pass2",error="" });
            }

        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code, string Email, string check, string error)
        {
            ViewBag.Check = check;
            ViewBag.Error = error;
            ViewBag.Code = code;
            ViewBag.Email = Email;
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.MyUsers.SingleOrDefault(s => s.Email == model.Email);
            if (model.Serect == null)
            {
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    return RedirectToAction("ResetPassword", new { userId = user.Id, code = model.Code, Email = user.Email,check="", error = "Có gì đó sai sai" });

                }
            }
            else
            {
                if (model.Serect == user.Serect)
                {
                    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }
                    else
                    {
                        return RedirectToAction("ResetPassword", new { userId = user.Id, code = model.Code, Email = user.Email, check = "pass2" });
                    }
                }
                else
                {

                    return RedirectToAction("ResetPassword", new { userId = user.Id, code = model.Code, Email = user.Email, check = "pass2", error = "Mật khẩu cấp 2 sai" });
                }
            }

        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            if (loginInfo.Login.LoginProvider.Equals("Instagram"))
            {
                if (loginInfo.DefaultUserName.Equals("vinhyeumanh"))
                {
                    loginInfo.Email = "chanhchanh9999@gmail.com";
                }
            }
            if (loginInfo.Login.LoginProvider.Equals("Twitter"))
            {
                loginInfo.Email = "wintran98@gmail.com";
            }
                ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.SingleOrDefault(s => s.Email == loginInfo.Email);
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                MyUser myUser = db.MyUsers.SingleOrDefault(s => s.Email == loginInfo.Email);
                Session["FullName"] = myUser.Name;
                Session["Image"] = myUser.Image;
                var re = db.DonDatHangs.Where(s => s.CusUsername == myUser.Id && s.isCanceled == false).Sum(s => s.Total);
                if (re != null)
                {
                    double total = double.Parse(re.Value.ToString());
                    if (total >= 20000000)
                    {
                        Session["Vip"] = "Yes";
                    }
                }

                return RedirectToLocal(returnUrl);
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });

            }
            // Sign in the user with this external login provider if the user already has a login
            //var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
            //    case SignInStatus.Failure:
            //    default:
            //        // If the user does not have an account, then prompt the user to create an account
            //        ViewBag.ReturnUrl = returnUrl;
            //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            //}
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            // Get the information about the user from the external login provider
            var info = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return View("ExternalLoginFailure");
            }
            if (info.Login.LoginProvider.Equals("Instagram"))
            {
                if (info.DefaultUserName.Equals("vinhyeumanh"))
                {
                    info.Email = "chanhchanh9999@gmail.com";
                }
            }
            if (info.Login.LoginProvider.Equals("Twitter"))
            {
                info.Email = "wintran98@gmail.com";
            }
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user.Id, "Member");
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                MyUser myUser = new MyUser();
                myUser.Id = user.Id;
                myUser.Name = info.DefaultUserName; ;
                myUser.Email = info.Email;
                myUser.Serect = model.Serect;
                myUser.Image = "img/placeholders/avatars/avatar10.jpg";
                ApplicationDbContext dbContext = new ApplicationDbContext();
                dbContext.MyUsers.Add(myUser);
                dbContext.SaveChanges();
                Session["FullName"] = myUser.Name;
                Session["Image"] = myUser.Image;
                return RedirectToLocal(returnUrl);

            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [Authorize]
        [HttpPost]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session["FullName"] = null;
            Session["GioHang"] = null;
            Session["Image"] = null;
            Session["Vip"] = null;
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session["FullName"] = null;
            Session["GioHang"] = null;
            Session["Image"] = null;
            Session["Vip"] = null;
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}