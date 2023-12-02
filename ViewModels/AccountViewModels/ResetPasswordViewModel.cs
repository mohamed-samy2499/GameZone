namespace GameZone.ViewModels.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Minimum length of password is 4 chars")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Minimum length of password is 4 chars")]
        [Compare("Password", ErrorMessage = "Confirm Password doesn't match The Password")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
        public string Token { get; set; }

    }
}
