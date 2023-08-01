using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tada.Shared
{
    public class AccountUser
    {

		public int Id { get; set; } = -1;

		public string Name { get; set; } = "";
		public string Email { get; set; } = "";
		public string Password { get; set; } = "";
		public string Token { get; set; } = "";
		public string Role { get; set; } = "";
		public int CreateUserId { get; set; } = 0;
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public int UpdateUserId { get; set; } = 0;
		public DateTime UpdateDate { get; set; } = DateTime.Now;

    }
}
