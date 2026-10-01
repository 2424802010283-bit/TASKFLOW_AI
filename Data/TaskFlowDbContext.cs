using Microsoft.EntityFrameworkCore;
using TASKFLOW_AI.Models;
using TASKFLOW_AI.Models._1_Workspace;   // <-- Thêm dòng này
using TASKFLOW_AI.Models._2_TaskGraph;   // <-- Thêm dòng này
using TASKFLOW_AI.Models._3_Communication;
using TASKFLOW_AI.Models._4_AI_Integration;


namespace TASKFLOW_AI.Data
{
    public class TaskFlowDbContext  : DbContext
    {
        public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : base(options)
        {
        }
        //-- cụm 1: 1_Workspace.( quản trị cốt lõi) 
        public DbSet<User> Users { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<WorkspaceMember> WorkspaceMembers { get; set; }


        //-- cụm 2: 2_TaskGraph ( quản lý công việc & thuật toán) 
        public DbSet<Project> Projects { get; set; }
        public DbSet<TASKFLOW_AI.Models._2_TaskGraph.Task> Tasks { get; set; }
        public DbSet<TaskDependency> TaskDependencies { get; set; }
        public DbSet<TaskAssignee> TaskAssignees { get; set; }

        //-- cụm 3: 3_Communication ( giao tiếp % RAG)

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get    ; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Transcript> Transcripts { get; set; }


        //-- cụm 4: 4_AI_Integration (AI % tích hợp ngoài) 
        public DbSet<AIInsight> AIInsights { get; set; }
        public DbSet<ExternalIntegration> ExternalIntegrations { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }


        public DbSet<Contact> Contacts { get; set; }


        // HÀM QUAN TRỌNG: Cấu hình Khóa chính kép và Khóa ngoại phức tạp
        // HÀM QUAN TRỌNG: Cấu hình Khóa chính kép và Khóa ngoại phức tạp
        // HÀM QUAN TRỌNG: Cấu hình Khóa chính kép và Khóa ngoại phức tạp
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Khóa chính kép cho WorkspaceMember 
            modelBuilder.Entity<WorkspaceMember>()
                .HasKey(wm => new { wm.WorkspaceId, wm.UserId });

            modelBuilder.Entity<WorkspaceMember>()
                .HasOne(wm => wm.Workspace)
                .WithMany(w => w.Members) // Nếu lỗi ở đây thì đổi thành .WithMany()
                .HasForeignKey(wm => wm.WorkspaceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkspaceMember>()
                .HasOne(wm => wm.User)
                .WithMany()
                .HasForeignKey(wm => wm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. KHÓA CHÍNH KÉP CHO TASK ASSIGNEE (DÒNG NÀY SẼ CHỮA LỖI MÀN HÌNH ĐỎ CỦA BẠN)
            modelBuilder.Entity<TaskAssignee>()
                .HasKey(ta => new { ta.TaskId, ta.UserId });

            modelBuilder.Entity<TaskAssignee>()
                .HasOne(ta => ta.Task)
                .WithMany()
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignee>()
                .HasOne(ta => ta.User)
                .WithMany()
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Khóa chính kép cho Thuật toán Đồ thị Task 
            modelBuilder.Entity<TaskDependency>()
                .HasKey(td => new { td.PrecedingTaskId, td.SucceedingTaskId });

            modelBuilder.Entity<TaskDependency>()
                .HasOne(td => td.PrecedingTask)
                .WithMany()
                .HasForeignKey(td => td.PrecedingTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskDependency>()
                .HasOne(td => td.SucceedingTask)
                .WithMany()
                .HasForeignKey(td => td.SucceedingTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Bảo vệ chống lỗi Cascade Delete cho Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}











