using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tada.Shared
{
    public class UserSession
    {
        public int UserId { get; set; } = 0;
        public string UserName { get; set; } = "";
        public string Token { get; set; } = "";
        public string Role { get; set; } = "";

        public int ExpiresIn { get; set; } = 0;
        public DateTime ExpiryTimeStamp { get; set; } = DateTime.Now;
    }
}
