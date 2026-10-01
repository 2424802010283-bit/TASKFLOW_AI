using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using TASKFLOW_AI.Data;
using TASKFLOW_AI.Models;
using TASKFLOW_AI.Models._1_Workspace;

namespace TASKFLOW_AI.Controllers
{
    public class HomeController : Controller
    {
        private readonly TaskFlowDbContext _context;

        public HomeController(TaskFlowDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier
                });
        }


        // =====================================================
        // WORKSPACE
        // =====================================================

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Workspace(
            int? workspaceId)
        {
            // =================================================
            // 1. LẤY USER ĐANG ĐĂNG NHẬP
            // =================================================

            var claim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(
                claim,
                out int currentUserId))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }


            // =================================================
            // 2. LẤY DANH SÁCH WORKSPACE CỦA USER
            // =================================================

            var workspaceMemberships =
                await _context.WorkspaceMembers

                    .Where(wm =>
                        wm.UserId ==
                        currentUserId)

                    .Include(wm =>
                        wm.Workspace)

                    .OrderBy(wm =>
                        wm.Workspace.Name)

                    .ToListAsync();


            // User chưa thuộc workspace nào
            if (workspaceMemberships.Count == 0)
            {
                return RedirectToAction(
                    "Create",
                    "Workspace");
            }


            // =================================================
            // 3. XÁC ĐỊNH WORKSPACE ĐANG MỞ
            // =================================================

            var currentMembership =
                workspaceId.HasValue

                    ? workspaceMemberships
                        .FirstOrDefault(
                            wm =>
                                wm.WorkspaceId ==
                                workspaceId.Value)

                    : workspaceMemberships.First();


            // User cố mở workspace không thuộc mình
            if (currentMembership == null)
            {
                return RedirectToAction(
                    "Workspace",
                    new
                    {
                        workspaceId =
                            workspaceMemberships
                                .First()
                                .WorkspaceId
                    });
            }


            // =================================================
            // 4. LẤY MEMBER CỦA WORKSPACE
            // =================================================

            var members =
                await _context.WorkspaceMembers

                    .Where(wm =>
                        wm.WorkspaceId ==
                        currentMembership.WorkspaceId)

                    .Include(wm =>
                        wm.User)

                    .OrderBy(wm =>
                        wm.Role)

                    .ThenBy(wm =>
                        wm.User.FullName)

                    .ToListAsync();


            // =================================================
            // 5. TẠO VIEW MODEL
            // =================================================

            var model =
                new WorkspacePageViewModel
                {
                    WorkspaceId =
                        currentMembership
                            .WorkspaceId,

                    WorkspaceName =
                        currentMembership
                            .Workspace
                            .Name,

                    WorkspaceRole =
                        currentMembership
                            .Role,

                    CurrentUserName =
                        User.Identity?.Name
                        ?? "User"
                };


            // =================================================
            // 6. WORKSPACE LIST
            // =================================================

            foreach (
                var membership
                in workspaceMemberships)
            {
                model.Workspaces.Add(
                    new WorkspaceListItemViewModel
                    {
                        Id =
                            membership
                                .WorkspaceId,

                        Name =
                            membership
                                .Workspace
                                .Name,

                        Role =
                            membership.Role
                    });
            }


            // =================================================
            // 7. MEMBER LIST
            // =================================================

            foreach (
                var member
                in members)
            {
                model.Members.Add(
                    new WorkspaceMemberViewModel
                    {
                        UserId =
                            member.UserId,

                        FullName =
                            member.User.FullName,

                        Email =
                            member.User.Email,

                        Role =
                            member.Role
                    });
            }


            return View(model);
        }
    }
}