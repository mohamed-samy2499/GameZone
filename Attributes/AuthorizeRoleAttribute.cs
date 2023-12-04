

namespace GameZone.Attributes
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public AuthorizeRoleAttribute(string role) : base()
        {
            Roles = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (!string.IsNullOrEmpty(Roles) && !user.IsInRole(Roles))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
