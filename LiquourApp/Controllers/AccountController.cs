using LiquourApp.Models.Auth;
using LiquourApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LiquourApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.Login(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Error al iniciar sesión");
                return View(model);
            }

            // Crear claims para la autenticación por cookies
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, result.Email),
                new Claim("firstName", result.FirstName),
                new Claim("lastName", result.LastName),
                new Claim("token", result.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = result.Expiration
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verificar edad
            if (!_authService.IsAdult(model.DateOfBirth))
            {
                ModelState.AddModelError("DateOfBirth", "Debes ser mayor de 18 años para registrarte");
                return View(model);
            }

            var result = await _authService.Register(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Error al registrarse");
                return View(model);
            }

            // Crear claims para la autenticación por cookies
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, result.Email),
                new Claim("firstName", result.FirstName),
                new Claim("lastName", result.LastName),
                new Claim("token", result.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = result.Expiration
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult VerifyAge()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyAge(DateTime dateOfBirth)
        {
            var isAdult = _authService.IsAdult(dateOfBirth);

            if (!isAdult)
            {
                ViewData["Message"] = "Lo sentimos, debes ser mayor de 18 años para acceder a este sitio.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}