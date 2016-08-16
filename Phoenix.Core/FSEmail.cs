using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace Phoenix.Core
{
    public class FSEmail
    {
        public string Subject { get; set; }
        public MailPriority Priority { get; set; }
        public bool IsHTML { get; set; }
        public string Body { get; set; }
        private string _attachments = "";
        private readonly SmtpClient _client = new SmtpClient(FSConfig.AppSettings("SMTPServer"));

        private readonly string _defaultSender;
        private readonly string _defaultSenderAlias;

        public string Attachments
        {
            get { return _attachments; }
            set { _attachments = value; }
        }

        private bool HasAttachments()
        {
            return !string.IsNullOrEmpty(_attachments);
        }

        public FSEmail(string subject, MailPriority priority, string body)
        {
            IsHTML = true;
            Subject = subject;
            Priority = priority;
            Body = body;

            _defaultSender = "FSAlert" + FSConfig.AppSettings("EnvironmentShortName") + "@" + FSConfig.AppSettings("CodeServer");
            _defaultSenderAlias = "FSAlert" + FSConfig.AppSettings("EnvironmentShortName");

            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public void Send(string sendTo)
        {
            Send(new List<string> { sendTo }, _defaultSender, _defaultSenderAlias);
        }

        public void Send(IEnumerable sendTo)
        {
            Send(sendTo, _defaultSender, _defaultSenderAlias);
        }

        public void Send(string sendTo, string sendFromAlias)
        {
            Send(new List<string> { sendTo }, _defaultSender, sendFromAlias);
        }

        public void Send(IEnumerable sendTo, string sendFromAlias)
        {
            Send(sendTo, _defaultSender, sendFromAlias);
        }

        public void Send(IEnumerable sendTo, string sendFrom, string sendFromAlias)
        {
            var email = CreateMail();

            foreach (var mailAdd in from string address in sendTo where IsAddress(address) select new MailAddress(address))
            {
                email.To.Add(mailAdd);
            }
            email.From = new MailAddress(sendFrom, sendFromAlias);

            if (email.To.Count > 0) _client.Send(email);

            email.Attachments.Dispose();
        }

        public void SendInlineImage(string sendTo, string sendFrom)
        {
            SendInlineImage(new List<string> { sendTo }, sendFrom, sendFrom);
        }

        public void SendInlineImage(string sendTo, string sendFrom, string sendFromAlias)
        {
            SendInlineImage(new List<string> { sendTo }, sendFrom, sendFromAlias);
        }

        public void SendInlineImage(IEnumerable<string> sendTo, string sendFrom)
        {
            SendInlineImage(sendTo, sendFrom, sendFrom);
        }

        public void SendInlineImage(IEnumerable<string> sendTo, string sendFrom, string sendFromAlias)
        {
            var email = CreateMail(false);

            foreach (var mailAdd in from string address in sendTo where IsAddress(address) select new MailAddress(address))
            {
                email.To.Add(mailAdd);
            }
            email.From = new MailAddress(sendFrom, sendFromAlias);
            email.Sender = email.From;

            email.AlternateViews.Add(AddInlineImage());

            if (email.To.Count > 0) _client.Send(email);

            email.Attachments.Dispose();
        }

        private AlternateView AddInlineImage()
        {
            var bitmap = new Bitmap(CoreResources.FirstRandLogo_SmallWhite);
            var logo = new MemoryStream();
            bitmap.Save(logo, ImageFormat.Png);
            logo.Position = 0;

            var inlineImage = new LinkedResource(logo, System.Net.Mime.MediaTypeNames.Image.Jpeg)
            {
                ContentId = "companyLogo"
            };

            var alternateView = AlternateView.CreateAlternateViewFromString(Body, null, System.Net.Mime.MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inlineImage);

            return alternateView;
        }

        private static bool IsAddress(string address)
        {
            var hasAt = address.IndexOf("@", StringComparison.Ordinal);
            var hasNotSecondAt = address.IndexOf("@", hasAt + 1, StringComparison.Ordinal);
            var hasDot = address.IndexOf(".", hasAt + 1, StringComparison.Ordinal);

            return hasAt > -1 & hasNotSecondAt == -1 & hasDot > -1;
        }

        private MailMessage CreateMail(bool removeLogo = true)
        {
            if (removeLogo) Body = Body.Replace(@"cid:companyLogo", @"");

            var email = new MailMessage
            {
                Subject = this.Subject,
                Body = this.Body,
                Priority = this.Priority,
                IsBodyHtml = this.IsHTML
            };

            if (HasAttachments())
                email.Attachments.Add(new Attachment(Attachments, System.Net.Mime.MediaTypeNames.Application.Octet));

            email.From = new MailAddress(_defaultSender, _defaultSenderAlias);
            email.Sender = email.From;

            return email;
        }
    }
}