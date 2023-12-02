namespace GameZone.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = default!;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "password is required")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Minimum length of password is 4 chars")]
        public string Password { get; set; } = default!;
        [Required(ErrorMessage = "confirm password is required")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Minimum length of password is 4 chars")]
        [Compare("Password", ErrorMessage = "Confirm Password doesn't match the password")]
        public string ConfirmPassword { get; set; } = default!;
        public bool IsAgree { get; set; }



    }
}

