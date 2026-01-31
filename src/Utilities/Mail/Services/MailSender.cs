using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Smtp;

namespace Mail.Services;

public class MailSender
{
    private readonly SmtpSender _smtpSender;
    private string _from = "";

    public MailSender(string host, int port, string from, string user, string password, bool enableSsl = true)
    {
        _smtpSender = new SmtpSender(() => new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(user, password),
            EnableSsl = enableSsl,
            UseDefaultCredentials = false
        });

        _from = from;

        FluentEmail.Core.Email.DefaultSender = _smtpSender;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string htmlBody)
    {
        var email = await Email
            .From(_from)
            .To(to)
            .Subject(subject)
            .Body(htmlBody, isHtml: true)
            .SendAsync();

        return email.Successful;
    }

}