using AutoMapper;
using Demo.DAL.Models;
using DemoPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{

    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, 
            IMapper mapper, UserManager <ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }
        #region Index
        public async Task<IActionResult> Index(string SearchValue)
        {

            if (string.IsNullOrEmpty(SearchValue))
            {
                var Roles = await _roleManager.Roles.ToListAsync();
                var MappedRole = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(Roles);
                return View(MappedRole);
            }
            else
            {
                var Role = await _roleManager.FindByNameAsync(SearchValue);
                var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);
                return View(new List<RoleViewModel> { MappedRole });
            }
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var MappedRole = _mapper.Map<RoleViewModel, IdentityRole>(model);
                await _roleManager.CreateAsync(MappedRole);
                return RedirectToAction("Index");

            }
            return View(model);
        }


        #endregion

        #region Details
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role is null)
                return NotFound();

            var MappedUser = _mapper.Map<IdentityRole, RoleViewModel>(Role);
            return View(ViewName,MappedUser);


        }
        #endregion


        #region Edit
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }


        [HttpPost]

        public async Task<IActionResult> Edit(RoleViewModel model, [FromRoute] string id)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await _roleManager.FindByIdAsync(id);
                   Role.Name = model.RoleName;

                    await _roleManager.UpdateAsync(Role);
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
        public async Task<IActionResult> ConfirmDelete( string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region AddOrRemoveUsers

        public async Task< IActionResult> AddOrRemoveUsers(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is  null) 
                return NotFound();
            var UsersInRole = new List<UserInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole =new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,

                };

                if (await _userManager.IsInRoleAsync(user , role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;
                UsersInRole.Add(userInRole);
            }
            return View(UsersInRole);

        }
        [HttpPost]
        public async Task< IActionResult> AddOrRemoveUsers ([FromRoute] string id , List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }
                }
                return RedirectToAction("Index" , "User");
            }
            return View(users);

        }        
        
        #endregion

    }
}
