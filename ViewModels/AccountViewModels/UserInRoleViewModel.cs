namespace GameZone.ViewModels.AccountViewModels
{
    public class UserInRoleViewModel
    {
        [Required]
        public string UserId { get; set; } = default!;
        [Required]
        public string UserName { get; set; } = default!;

        public bool IsSelected { get; set; }
    }
}
