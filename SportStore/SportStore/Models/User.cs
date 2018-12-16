using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models
{
	public class User : IdentityUser<int>
	{
		public User(string userName) : base(userName)
		{
		}
		public User()
		{
		}
	}
}
