// tao vieew cho Workspace 

namespace TASKFLOW_AI.Models._1_Workspace
{
    public class WorkspacePageViewModel
    {
        public int WorkspaceId { get; set; }

        public string WorkspaceName { get; set; } = string.Empty;

        public string WorkspaceRole { get; set; } = string.Empty;

        public string CurrentUserName { get; set; } = string.Empty;

        public List<WorkspaceListItemViewModel> Workspaces { get; set; }
            = new List<WorkspaceListItemViewModel>();

        public List<WorkspaceMemberViewModel> Members { get; set; }
            = new List<WorkspaceMemberViewModel>();
    }


    public class WorkspaceListItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }


    public class WorkspaceMemberViewModel
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}