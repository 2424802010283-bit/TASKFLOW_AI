using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TASKFLOW_AI.Models._1_Workspace
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        public int UserId { get; set; }
        public int FriendId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual User Owner { get; set; }

        [ForeignKey("FriendId")]
        public virtual User Friend { get; set; }
    }
}