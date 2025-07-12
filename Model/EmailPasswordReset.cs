using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EmailResetPassword
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string ResetCode { get; set; } = string.Empty;
        public DateTime ExpiredAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual SystemUser? User { get; set; }
    }

}
