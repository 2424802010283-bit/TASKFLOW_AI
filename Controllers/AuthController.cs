using Microsoft.AspNetCore.Mvc;
using TASKFLOW_AI.Data;
using TASKFLOW_AI.Models._1_Workspace;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace TASKFLOW_AI.Controllers
{
    public class AuthController : Controller
    {
        private readonly TaskFlowDbContext _context;

        public AuthController(TaskFlowDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. LUỒNG ĐĂNG KÝ
        // ==========================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Vui lòng nhập email.";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                user = new User
                {
                    FullName = email.Split('@')[0],
                    Email = email,
                    PasswordHash = ComputeSha256Hash("MatKhauTam123!"),
                    CreatedAt = DateTime.Now,
                    WorkloadCapacity = 40
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            await SignInUserAsync(user);
            return RedirectToAction("Workspace", "Home");
        }

        // ==========================================
        // 2. LUỒNG ĐĂNG NHẬP (Bổ sung ràng buộc)
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Vui lòng nhập email.";
                return View();
            }

            // KIỂM TRA RÀNG BUỘC: Phải có tài khoản trước mới được vào
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                // Khóa luồng nếu chưa đăng ký
                ViewBag.Error = "Tài khoản chưa tồn tại. Vui lòng tạo tài khoản trước.";
                return View();
            }

            // Nếu đã đăng ký -> Cho phép đăng nhập và lưu phiên
            await SignInUserAsync(user);
            return RedirectToAction("Workspace", "Home");
        }

        // ==========================================
        // CÁC HÀM HỖ TRỢ NGHIỆP VỤ
        // ==========================================
        private async Task SignInUserAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}