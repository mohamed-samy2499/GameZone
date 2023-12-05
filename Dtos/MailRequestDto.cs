namespace GameZone.DTO
{
    public class MailRequestDto
    {
        [Required]
        public string ToEmail { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Body { get; set; } = string.Empty;
        public IList<IFormFile>? Attacments { get; set; } = default!;
    }
}
