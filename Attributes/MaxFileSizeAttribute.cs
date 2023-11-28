namespace GameZone.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSizeAllowed;

        public MaxFileSizeAttribute(int maxSizeAllowed)
        {
            _maxSizeAllowed = maxSizeAllowed;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var isAllowed = file.Length <= _maxSizeAllowed ;
                if (!isAllowed)
                {
                    return new ValidationResult($"Max file size is {_maxSizeAllowed} bytes");
                }
            }
            return ValidationResult.Success;
        }
    }
}
