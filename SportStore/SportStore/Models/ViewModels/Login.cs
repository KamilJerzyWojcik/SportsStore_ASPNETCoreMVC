using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models.ViewModels
{
	public class Login
	{
		[Required]
		public string Name { get; set; }

		[Required]
		[UIHint("password")] //typ password input z asp-for (template)
		public string Password { get; set; }

		public string ReturnUrl { get; set; } = "/";
	}
}
