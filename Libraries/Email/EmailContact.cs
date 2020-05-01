using System.Net;
using System.Net.Mail;
using OnlineStore.Models;
using OnlineStore.Libraries.XML;

namespace OnlineStore.Libraries.Email
{
    public class EmailContact
    {
        public static void SendContactMessageByEmail(Contact newContact)
        {
            // SMPT -> Server that will send the message.
            SmtpClient emailSender = new SmtpClient("smtp.gmail.com", 587);
            emailSender.UseDefaultCredentials = false;

            string privateXmlPath = "./Private/PrivateData.xml";
            string email = XMLReader.GetDataFromXMLFile(privateXmlPath, "Contact_Email");
            string password = XMLReader.GetDataFromXMLFile(privateXmlPath, "Contact_Password");
            
            emailSender.Credentials = new NetworkCredential(email, password);
            emailSender.EnableSsl = true;

            string messageBody = 
                "<h2>OnlineStore Contact</h2>" +
                $"<b>Name:</b> {newContact.Name} <br/>" +
                $"<b>E-mail:</b> {newContact.Email} <br/>" +
                $"<b>Message:</b> {newContact.Text} <br/>" +
                "<br/> Email sent automatically by OnlineStore.";

            // MailMessage -> Creates the message.
            MailMessage message = new MailMessage();
            message.From = new MailAddress(email);
            message.To.Add(email);
            message.Subject = "OnlineStore Contact - E-mail: " + newContact.Email;
            message.Body = messageBody;
            message.IsBodyHtml = true;

            emailSender.Send(message);
        }
    }
}