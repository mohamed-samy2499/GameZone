namespace GameZone.Settings
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email, IdentityUser user)
        {
            var Client = new SmtpClient("smtp.gmail.com", 587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("mailservice7539@gmail.com", "MailAgent2499");
            Client.Send("mailservice7539@gmail.com", user.Email, email.Title, email.Body);
        }
    }
}
