using AutoMapper;
using Demo.DAL.Models;
using DemoPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{


	[Authorize(Roles = "Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager , IMapper mapper)
        {
			_userManager = userManager;
            _mapper = mapper;
        }
		#region Index
		public async Task<IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var Users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FName = U.FName,
					LName = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result,


				}).ToListAsync();
				return View(Users);
			}

			else
			{
				var User = await _userManager.FindByEmailAsync(SearchValue);
				var MappedUser = new UserViewModel()
				{
					Id = User.Id,
					FName = User.FName,
					LName = User.LName,
					Email = User.Email,
					PhoneNumber = User.PhoneNumber,
					Roles = _userManager.GetRolesAsync(User).Result,
				};

				return View(new List<UserViewModel> { MappedUser });
			}
		}
        #endregion

        #region Details
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
		{
			if (id is null)
				return BadRequest();
			var User = await _userManager.FindByIdAsync(id);
			if (User is null)
				return NotFound();

			var MappedUser = _mapper.Map<ApplicationUser, UserViewModel>(User);
			return View(ViewName,MappedUser);


		}
		#endregion


		#region Edit
		public async Task<IActionResult> Edit(string id)
		{
			return await Details(id, "Edit");
		}


		[HttpPost]

		public async Task<IActionResult> Edit(UserViewModel model, [FromRoute] string id)
		{
			if (id != model.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					var User = await _userManager.FindByIdAsync(id);
					User.PhoneNumber = model.PhoneNumber;
					User.FName = model.FName;
					User.LName = model.LName;

					await _userManager.UpdateAsync(User);
					return RedirectToAction(nameof(Index));

				}
				catch (System.Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);

				}
			}
			return View(model);

		}
		#endregion

		#region Delete
		public async Task<IActionResult> Delete(string id)
		{
			return await Details(id, "Delete");
		}

		[HttpPost]
		public async Task<IActionResult> ConfirmDelete([FromRoute] string id)
		{
			try
			{
				var User = await _userManager.FindByIdAsync(id);
				await _userManager.DeleteAsync(User);
				return RedirectToAction(nameof(Index));
			}
			catch (System.Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return RedirectToAction("Error", "Home");
			}
		} 
		#endregion
	}
}
