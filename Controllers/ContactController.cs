        using Microsoft.AspNetCore.Mvc;
        using Microsoft.EntityFrameworkCore;
        using System.Security.Claims;
        using TASKFLOW_AI.Data;
        using TASKFLOW_AI.Models._1_Workspace;

        namespace TASKFLOW_AI.Controllers
        {
            public class ContactController : Controller
            {
                private readonly TaskFlowDbContext _context;

                public ContactController(TaskFlowDbContext context)
                {
                    _context = context;
                }

       // TÌM BẰNG GMAIL VÀ TÊN CÓ THỂ TÌM BẰNG IN HOA IN ĐẬM KHÔNG BỎ DẤU VẪN RA ( CHỈ SỬA KHI WUAS CẦN BÌNH THƯỜNG KHONG ĐỤNG)

        [HttpGet]
        public async Task<IActionResult> SearchUser(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new { success = true, data = new List<object>() });
            }

            // Chuẩn hóa từ khóa tìm kiếm (bỏ khoảng trắng thừa và viết thường)
            query = query.Trim().ToLower();

            // Lấy ID của người đang đăng nhập
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int currentUserId = 0;

            if (!string.IsNullOrEmpty(userIdClaim))
            {
                currentUserId = int.Parse(userIdClaim);
            }

            // Truy vấn Database: Tìm theo Email hoặc Tên (bỏ qua bản thân mình)
            var users = await _context.Users
                .Where(u =>
                    u.UserId != currentUserId &&
                    (u.Email.ToLower().Contains(query) || u.FullName.ToLower().Contains(query))
                )
                .Select(u => new
                {
                    id = u.UserId,
                    fullName = u.FullName,
                    email = u.Email
                })
                .Take(10) // Lấy tối đa 10 người để giao diện không bị giật
                .ToListAsync();

            return Json(new
            {
                success = true,
                data = users
            });
        }

        // ==============================
        // THÊM NGƯỜI DÙNG VÀO DANH BẠ
        // ==============================
        [HttpPost]
                public async Task<IActionResult> AddContact(int friendId)
                {
                    var userIdClaim = User.FindFirstValue(
                        ClaimTypes.NameIdentifier
                    );

                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Bạn chưa đăng nhập."
                        });
                    }

                    var currentUserId = int.Parse(userIdClaim);


                    // Không cho tự thêm chính mình
                    if (currentUserId == friendId)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Bạn không thể tự thêm chính mình."
                        });
                    }


                    // Kiểm tra người được thêm có tồn tại không
                    var friendExists = await _context.Users
                        .AnyAsync(u => u.UserId == friendId);

                    if (!friendExists)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Không tìm thấy người dùng."
                        });
                    }


                    // Kiểm tra đã tồn tại trong danh bạ chưa
                    var exists = await _context.Contacts
                        .AnyAsync(c =>
                            c.UserId == currentUserId &&
                            c.FriendId == friendId);

                    if (exists)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Người này đã có trong danh bạ."
                        });
                    }


                    // Tạo contact
                    var contact = new Contact
                    {
                        UserId = currentUserId,
                        FriendId = friendId,
                        CreatedAt = DateTime.Now
                    };

                    _context.Contacts.Add(contact);

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        success = true,
                        message = "Thêm liên hệ thành công."
                    });
                }
            }
        }