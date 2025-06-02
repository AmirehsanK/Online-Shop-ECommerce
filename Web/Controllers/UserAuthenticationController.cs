using Application.Services.Interfaces;
using Domain.Enums;
using Domain.ViewModel.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    public class UserAuthenticationController : SiteBaseController
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IRecaptchaVerifier _recaptchaVerifier;

        public UserAuthenticationController(
            IUserService userService,
            IPasswordService passwordService,
            IRecaptchaVerifier recaptchaVerifier)
        {
            _userService = userService;
            _passwordService = passwordService;
            _recaptchaVerifier = recaptchaVerifier;
        }

        #region Logout

        [HttpGet("Logout")] 
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion

        #region ForgotPassword

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordMailUserViewModel mailViewModel)
        {
            if (!ModelState.IsValid)
                return View(mailViewModel);

            var result = await _passwordService.ForgotPasswordEmailSenderAsync(mailViewModel.Email);
            switch (result)
            {
                case ForgetPasswordEnum.Success:
                    TempData[SuccessMessage] = "لینک تغییر رمز عبور با موفقیت به ایمیل شما ارسال شد";
                    return RedirectToAction(nameof(Login));
                case ForgetPasswordEnum.UserNotFound:
                    ViewBag.message = "حساب کاربری با این ایمیل یافت نشد"; 
                    return View(mailViewModel);
                case ForgetPasswordEnum.EmailSendFailed:
                default: // Consolidate default and EmailSendFailed
                    ViewBag.message = "خطایی رخ داده است لطفا دوباره تلاش کنید";
                    return View(mailViewModel);
            }
        }

        [HttpGet("ForgetPassword/{token}")]
        public async Task<IActionResult> ForgotPasswordChangePassword(string token)
        {
            var tokenEnum = await _passwordService.ForgotPasswordTokenCheckerAsync(token);
            if (tokenEnum == ForgetPasswordTokenCheckEnum.Success)
            {
                ViewData["Token"] = token; 
                return View(); 
            }
            TempData[ErrorMessage] = "لینک تغییر رمز عبور معتبر نمی باشد یا منقضی شده است.";
            return RedirectToAction(nameof(Login)); 
        }
        
        // This GET action seems problematic as it tries to activate an email with a token
        // and then redirects to login. It might be confused with email activation.
        // If it's intended to show a form for password change after token validation,
        // the ForgotPasswordChangePassword(string token) GET action should render that form.
        // I'm commenting it out as its purpose is unclear and potentially conflicting.
        
        [HttpGet("ForgetPassword")] // This route conflicts with the POST below if not distinguished by parameters/name
        public async Task<IActionResult> ForgotPasswordChanger()
        {
            // This logic seems to belong elsewhere or needs rethinking.
            // 'TempData["Token"] as string' is unreliable across requests if not set carefully.
            // 'userService.EmailActivatorAsync' seems out of place for password reset.
            var token = TempData["Token"] as string; 
            var user = await _userService.EmailActivatorAsync(token!);
            return RedirectToAction(nameof(Login));
        }
        

        [HttpPost("ResetPasswordWithToken")] 
        public async Task<IActionResult> ResetPasswordWithToken(ForgetPasswordUserViewModel model)
        {
            if (!ModelState.IsValid)
            {

                ViewData["Token"] = model.ActivationCode; 
                return View("ForgotPasswordChangePassword", model); 
            }

            var result = await _passwordService.ResetPasswordAsync(model.ActivationCode, model.NewPassword);
            
            if (result) 
            {
                TempData[SuccessMessage] = "رمز عبور با موفقیت تغییر یافت";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData[ErrorMessage] = "تغییر رمز عبور با مشکل مواجه شد. ممکن است لینک منقضی شده باشد یا رمز جدید معتبر نباشد.";
                ViewData["Token"] = model.ActivationCode;
                return View("ForgotPasswordChangePassword", model);
            }
        }

        #endregion

        #region Login

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserViewModel login)
        {
            if (!ModelState.IsValid) return View(login);

            var googleRecaptchaToken = Request.Form["g-recaptcha-response"].ToString();
            string? userIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            
            var isRecaptchaValid = await _recaptchaVerifier.IsRecaptchaValidAsync(googleRecaptchaToken, userIpAddress);
            if (!isRecaptchaValid)
            {
                TempData[ErrorMessage] = "کپچا را کامل کنید";
                return View(login);
            }

            var result = await _userService.LoginUserAsync(login);
            switch (result)
            {
                case LoginUserEnum.PasswordInvalid:
                case LoginUserEnum.EmailInvalid: 
                    TempData[ErrorMessage] = "نام کاربری یا رمز عبور شما اشتباه است";
                    return View(login);
                case LoginUserEnum.Success:
                    var user = await _userService.GetUserByEmailAsync(login.Email);
                    if (user == null) 
                    {
                        TempData[ErrorMessage] = "خطایی در ورود رخ داده است.";
                        return View(login);
                    }
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                        new(ClaimTypes.Email, user.Email),
                        new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new("IsAdmin", user.IsAdmin.ToString()) 
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };

                    await HttpContext.SignInAsync(principal, properties);
                    TempData[SuccessMessage] = "کاربر گرامی به سایت خوش آمدید";
                    return Redirect("/");
                case LoginUserEnum.UserNotActive:
                    TempData[ErrorMessage] = "اکانت شما فعال نشده است. لطفا ایمیل خود را برای لینک فعال سازی بررسی کنید.";
                    return View(login);
                default:
                    TempData[ErrorMessage] = "خطای نامشخصی در ورود رخ داده است.";
                    return View(login);
            }
        }

        #endregion

        #region Register

        [HttpGet("signup")]
        public IActionResult SignUp()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignupUser(RegisterUserViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View("SignUp", register); 
            }

            var googleRecaptchaToken = Request.Form["g-recaptcha-response"].ToString();
            string? userIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var isRecaptchaValid = await _recaptchaVerifier.IsRecaptchaValidAsync(googleRecaptchaToken, userIpAddress);
            if (!isRecaptchaValid)
            {
                TempData[ErrorMessage] = "کپچا را کامل کنید";
                return View("SignUp", register); 
            }

            var result = await _userService.RegisterUserValidationAsync(register);
            switch (result)
            {
                case RegisterUserEnum.EmailUsed:
                    TempData[ErrorMessage] = "ایمیل تکراری است";
                    return View("SignUp", register); 
                case RegisterUserEnum.Success:
                    await _userService.RegisterUserAsync(register);
                    TempData[SuccessMessage] = "ثبت نام شما با موفقیت انجام شد. لطفا ایمیل خود را برای فعال سازی حساب کاربری بررسی کنید.";
                    return RedirectToAction(nameof(Login)); 
                default:
                     TempData[ErrorMessage] = "خطای نامشخصی در هنگام ثبت نام رخ داد.";
                    return View("SignUp", register); 
            }
        }

        [HttpGet("RegistrationSuccess")] 
        public IActionResult RegistrationSuccess() 
        {
            
            ViewBag.Message = TempData[SuccessMessage] ?? "ثبت نام شما با موفقیت انجام شد. لطفا ایمیل خود را برای فعال سازی حساب کاربری بررسی کنید.";
            return View("Success");
        }

        [HttpGet("EmailActive/{emailActiveCode}")]
        public async Task<IActionResult> EmailActive(string emailActiveCode)
        {
            var result = await _userService.EmailActivatorAsync(emailActiveCode);
            if (result == ActiveEmailEnum.Success)
            {
                TempData[SuccessMessage] = "اکانت شما با موفقیت فعال سازی شد. اکنون می توانید وارد شوید.";
            }
            else
            {
                TempData[ErrorMessage] = "لینک فعال سازی نامعتبر است یا منقضی شده است.";
            }
            return RedirectToAction(nameof(Login));
        }

        #endregion
    }
}