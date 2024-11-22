using System.Threading.Tasks;
using Azure;

namespace PulseFit.Management.Web.Helpers
{
    public interface IMailHelper
    {
        // Method to send an email. It takes three parameters:
        // 'to' - the recipient's email address,
        // 'subject' - the subject of the email,
        // 'body' - the body of the message.
        Response SendEmail(string to, string subject, string body);


        string LoadAndProcessEmailTemplate(string templatePath, Dictionary<string, string> placeholders);
    }
}
