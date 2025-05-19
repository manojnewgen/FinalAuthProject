using System.Web;

namespace FinalAuthProject.Middlewares
{
    public static class InputValidator
    {
        public static string SanitizeInput(string input)
        {
            // XSS koruması için HTML encoding
            return HttpUtility.HtmlEncode(input);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
