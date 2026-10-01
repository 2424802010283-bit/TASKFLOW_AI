using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TASKFLOW_AI.Data;
using TASKFLOW_AI.Models._1_Workspace;

namespace TASKFLOW_AI.Controllers
{
    public class WorkspaceController : Controller
    {
        private readonly TaskFlowDbContext _context;

        public WorkspaceController(TaskFlowDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // HIỂN THỊ DANH SÁCH WORKSPACE CỦA USER
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> MyWorkspaces()
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var workspaces = await _context.WorkspaceMembers
                .Where(wm => wm.UserId == currentUserId.Value)
                .Include(wm => wm.Workspace)
                .Select(wm => new
                {
                    id = wm.Workspace.WorkspaceId,
                    name = wm.Workspace.Name,
                    role = wm.Role
                })
                .ToListAsync();

            return Json(new
            {
                success = true,
                data = workspaces
            });
        }


        // ==========================================
        // TẠO WORKSPACE
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> Create(
    [FromForm] string name,
     List<int> memberIds)
        {
            try
            {
                var currentUserId = GetCurrentUserId();

                if (currentUserId == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Bạn chưa đăng nhập."
                    });
                }


                // ==========================================
                // KIỂM TRA TÊN
                // ==========================================

                if (string.IsNullOrWhiteSpace(name))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Vui lòng nhập tên nhóm."
                    });
                }


                // ==========================================
                // CHUẨN HÓA DANH SÁCH THÀNH VIÊN
                // ==========================================

                memberIds ??= new List<int>();


                // Loại ID trùng
                memberIds = memberIds
                    .Distinct()
                    .ToList();


                // ==========================================
                // KHÔNG CHO CHỌN CHÍNH MÌNH
                // ==========================================

                memberIds.Remove(currentUserId.Value);


                // ==========================================
                // NGƯỜI TẠO + CÁC THÀNH VIÊN ĐƯỢC CHỌN
                // ==========================================

                var allMemberIds = new List<int>
        {
            currentUserId.Value
        };

                allMemberIds.AddRange(memberIds);

                allMemberIds = allMemberIds
                    .Distinct()
                    .ToList();


                // ==========================================
                // PHẢI CÓ ÍT NHẤT 3 NGƯỜI
                // ==========================================

                if (allMemberIds.Count < 3)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Nhóm phải có ít nhất 3 thành viên, bao gồm bạn."
                    });
                }


                // ==========================================
                // KIỂM TRA USER CÓ TỒN TẠI
                // ==========================================

                var validUserIds = await _context.Users
                    .Where(u => allMemberIds.Contains(u.UserId))
                    .Select(u => u.UserId)
                    .ToListAsync();


                if (validUserIds.Count != allMemberIds.Count)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Một hoặc nhiều thành viên không tồn tại."
                    });
                }


                // ==========================================
                // TẠO WORKSPACE
                // ==========================================

                var workspace = new Workspace
                {
                    Name = name.Trim(),
                    OwnerId = currentUserId.Value,
                    CreatedAt = DateTime.Now
                };


                _context.Workspaces.Add(workspace);

                await _context.SaveChangesAsync();


                // ==========================================
                // THÊM THÀNH VIÊN
                // ==========================================

                foreach (var userId in allMemberIds)
                {
                    var member = new WorkspaceMember
                    {
                        WorkspaceId = workspace.WorkspaceId,

                        UserId = userId,

                        Role = userId == currentUserId.Value
                            ? "Owner"
                            : "Member",


                        JoinedAt = DateTime.Now
                    };

                    _context.WorkspaceMembers.Add(member);
                }


                await _context.SaveChangesAsync();


                // ==========================================
                // THÀNH CÔNG
                // ==========================================

                return Json(new
                {
                    success = true,

                    message = "Tạo nhóm thành công.",

                    workspaceId = workspace.WorkspaceId,

                    workspaceName = workspace.Name
                });
            }
            catch (Exception ex)
            {
                // In lỗi thật ra Console
                Console.WriteLine("====================================");
                Console.WriteLine("LỖI CREATE WORKSPACE");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("====================================");


                return StatusCode(500, new
                {
                    success = false,

                    message = "Lỗi server khi tạo nhóm.",

                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }


        // ==========================================
        // HELPER
        // ==========================================

        private int? GetCurrentUserId()
        {
            var value = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (int.TryParse(value, out int userId))
            {
                return userId;
            }

            return null;
        }
    }
}