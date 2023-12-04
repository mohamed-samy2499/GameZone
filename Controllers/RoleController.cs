using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    [Authorize(Roles = "admin")]

    public class RoleController : Controller
    {
        #region Properties
        public RoleManager<IdentityRole> RoleManager { get; }
        public UserManager<IdentityUser> UserManager { get; }
        #endregion

        #region Constructor
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }
        #endregion

        #region Actions
        #region Index
        public IActionResult Index()
        {
            var Roles = RoleManager.Roles;
            return View(Roles);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {

            if (id == null)
                return NotFound();
            var Role = await RoleManager.FindByIdAsync(id);

            if (Role == null)
                return NotFound();
            var users = UserManager.Users;
            List<UserInRoleViewModel> UsersInRole = new List<UserInRoleViewModel>();
            foreach (var user in users)
            {
                var UserInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await UserManager.IsInRoleAsync(user, Role.Name))
                    UsersInRole.Add(UserInRole);
            }
            ViewData["UsersInRole"] = UsersInRole;

            return View(ViewName, Role);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(string id)
        {

            if (id == null)
                return NotFound();
            var Role = await RoleManager.FindByIdAsync(id);
            if (Role == null)
                return NotFound();
            return View("Delete", Role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, IdentityRole model)
        {
            if (id != model.Id)
                return NotFound();
            try
            {
                var role = await RoleManager.FindByIdAsync(id);

                var result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            catch
            {
                return View(model);
            }
            return View(model);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();
            var Role = await RoleManager.FindByIdAsync(id);
            if (Role == null)
                return NotFound();
            return View("Edit", Role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IdentityRole model)
        {
            if (model.Id != id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await RoleManager.FindByIdAsync(id);
                    role.Name = model.Name;

                    var result = await RoleManager.UpdateAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(model);
                }
            }
            return View(model);
        }
        #endregion
        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await RoleManager.CreateAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(role);
                }
            }
            return View(role);
        }
        #endregion
        #region Assign Roles
        public async Task<IActionResult> AddOrRemoveUsers(string RoleId)
        {
            if (RoleId == null)
                return NotFound();
            var role = await RoleManager.FindByIdAsync(RoleId);
            if (role == null)
                return NotFound();
            var users = UserManager.Users;
            List<UserInRoleViewModel> UsersInRole = new List<UserInRoleViewModel>();
            foreach (var user in users)
            {
                var UserInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await UserManager.IsInRoleAsync(user, role.Name))
                    UserInRole.IsSelected = true;
                else
                    UserInRole.IsSelected = false;
                UsersInRole.Add(UserInRole);

            }
            return View(UsersInRole);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleViewModel> model, string RoleId)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(RoleId);
                foreach (var user in model)
                {
                    var FullUser = await UserManager.FindByIdAsync(user.UserId);
                    if (!user.IsSelected && await UserManager.IsInRoleAsync(FullUser, role.Name))
                    {
                        await UserManager.RemoveFromRoleAsync(FullUser, role.Name);
                    }
                    else if (user.IsSelected && !await UserManager.IsInRoleAsync(FullUser, role.Name))
                    {
                        await UserManager.AddToRoleAsync(FullUser, role.Name);
                    }
                }
                return RedirectToAction("Edit", "Role", new { id = RoleId });
            }
            return View(model);
        }
        #endregion
        #endregion

    }
}
