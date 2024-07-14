﻿

namespace SurveyBasket.Infrastruction.Implementations.EmailSender;
internal class EmailService(IOptions<MailSettings> mailSettings) : IEmailSender
{
    private readonly MailSettings _mailSettings = mailSettings.Value;

    public  async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSettings.Mail),
            Subject = subject,
        };

        //User Email
        message.To.Add(MailboxAddress.Parse(email));


        //the template
        var builder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };

        message.Body = builder.ToMessageBody();



        using var smtp = new SmtpClient();

        //Transport Layer Security (TLS)
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

        await smtp.SendAsync(message);

        smtp.Disconnect(true);


    }
}
