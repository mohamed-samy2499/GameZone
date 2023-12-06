using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    [Authorize(Roles = "superadmin")]

    public class UserController : Controller
    {
        #region Properties
        public UserManager<IdentityUser> UserManager { get; }
        #endregion

        #region Constructor
        public UserController(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }
        #endregion

        #region Actions
        #region Index
        public IActionResult Index()
        {
            var Users = UserManager.Users;
            return View(Users);
        }
        #endregion
        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new IdentityUser
                    {

                        UserName = model.Name,
                        Email = model.Email
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return View(model);
                }
            }
            return View(model);
        }
        #endregion
        #region Details
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {

            if (id == null)
                return NotFound();
            var User = await UserManager.FindByIdAsync(id);
            if (User == null)
                return NotFound();
            return View(ViewName, User);
        }
        #endregion

        #region Delete
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {

            var user = await UserManager.FindByIdAsync(id);
            if(user == null)
                return NotFound();
            var isDeleted = await UserManager.DeleteAsync(user);
            if(isDeleted.Succeeded)
                return Ok();
            return BadRequest();

        }
      
        #endregion


        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IdentityUser model)
        {
            if (model.Id != id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserManager.FindByIdAsync(id);
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.NormalizedEmail = model.NormalizedEmail;
                    user.NormalizedUserName = model.NormalizedUserName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.SecurityStamp = model.SecurityStamp;
                    var result = await UserManager.UpdateAsync(user);
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
        #endregion

    }
}
