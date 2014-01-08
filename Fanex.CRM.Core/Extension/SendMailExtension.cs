// --------------------------------------------------------------------------
// <copyright file="SendMailExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.MailDeliver.MailClient.API;
    using Fanex.MailDeliver.Model;

    /// <summary>
    /// Send Mail Extension.
    /// </summary>
    public class SendMailExtension
    {
        #region private variables
        private static bool _sendMailStatus = true;
        #endregion
        /**
         * Accounting - Send mail & notification
         **/
        public static void AccSendMail(MailSection.HostName hostName, string to, string cc, string mailTitle, string subject, string body)
        {
            AccSendMail(hostName, to, cc, mailTitle, subject, body, null);
        }

        public static bool AccSendMail(
            MailSection.HostName hostName,
            string to,
            string cc,
            string mailTitle,
            string subject,
            string body,
            MailAttachment mailAttachment)
        {
            AccSendMail(hostName, to, cc, string.Empty, mailTitle, subject, body, mailAttachment, null);
            return _sendMailStatus;
        }

        public static bool AccSendMail(
            MailSection.HostName hostName,
            string to,
            string cc,
            string bcc,
            string mailTitle,
            string subject,
            string body,
            MailAttachment mailAttachment)
        {
            AccSendMail(hostName, to, cc, bcc, mailTitle, subject, body, mailAttachment, null);
            return _sendMailStatus;
        }

        /// <summary>
        /// Send accounting mail.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="to">Destination Address.</param>
        /// <param name="cc">The CC address.</param>
        /// <param name="bcc">The BCC address.</param>
        /// <param name="mailTitle">The mail title.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body content.</param>
        /// <param name="mailAttachment">The mail attachment.</param>
        /// <param name="altView">The alt view.</param>
        public static void AccSendMail(
            MailSection.HostName hostName,
            string to,
            string cc,
            string bcc,
            string mailTitle,
            string subject,
            string body,
            MailAttachment mailAttachment,
            AlternateView altView)
        {
            var mailSend = new MailSend()
            {
                HostName = hostName,
                To = to,
                Cc = cc,
                Bcc = bcc,
                Title = mailTitle,
                Subject = subject,
                Body = body,
                Attachment = mailAttachment,
                AltView = altView
            };

            Task.Factory.StartNew(() =>
            {
                SendMail(mailSend);
            });
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="mailSend">The mail send.</param>
        /// <returns>Send mail result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static bool SendMail(MailSend mailSend)
        {
            try
            {
                using (var client = new MailDeliver())
                {
                    var mailItem = new Fanex.MailDeliver.Model.MailRecord
                    {
                        MailTo = mailSend.To,
                        MailCc = mailSend.Cc,
                        MailBcc = mailSend.Bcc,
                        Title = mailSend.Title,
                        Subject = mailSend.Subject,
                        Body = mailSend.Body
                    };
                    if (mailSend.Attachment != null && mailSend.Attachment.FileName != null)
                    {
                        mailItem.Attachments = new List<Fanex.MailDeliver.Model.MailAttachment>();
                        var attItem = new Fanex.MailDeliver.Model.MailAttachment
                        {
                            FileName = mailSend.Attachment.FileName,
                            Content = mailSend.Attachment.Content
                        };
                        if (mailSend.Attachment.ContentType.ToLower().IndexOf("excel") != -1)
                        {
                            attItem.ContentType = FileExtentions.MS_EXCEL;
                        }
                        else if (mailSend.Attachment.ContentType.ToLower().IndexOf("doc") != -1)
                        {
                            attItem.ContentType = FileExtentions.MS_DOC;
                        }
                        else
                        {
                            attItem.ContentType = FileExtentions.IMG_GIF;
                        }

                        mailItem.Attachments.Add(attItem);
                    }

                    if (mailSend.AltView != null)
                    {
                        mailItem.AlternateView = new List<MailLinkedResource>();
                        var linkedResource = mailSend.AltView.LinkedResources;
                        foreach (var linked in linkedResource)
                        {
                            var stream = linked.ContentStream;
                            byte[] buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                            var resource = new MailLinkedResource
                            {
                                FileName = string.Empty,
                                ImageType = FileExtentions.IMG_GIF,
                                ContentCID = linked.ContentId,
                                StreamContent = new MemoryStream(buffer)
                            };
                            mailItem.AlternateView.Add(resource);
                        }
                    }

                    // send mail
                    client.SendMail(mailItem);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return false;
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="mailSend">The mail send.</param>
        /// <param name="mailAttachment">The mail attachment.</param>
        /// <returns>Send mail result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static bool SendMail(MailSend mailSend, List<MailAttachment> mailAttachment)
        {
            try
            {
                using (var client = new MailDeliver())
                {
                    var mailItem = new Fanex.MailDeliver.Model.MailRecord
                    {
                        MailTo = mailSend.To,
                        MailCc = mailSend.Cc,
                        MailBcc = mailSend.Bcc,
                        Title = mailSend.Title,
                        Subject = mailSend.Subject,
                        Body = mailSend.Body
                    };
                    if (mailSend.Attachment != null && mailSend.Attachment.FileName != null)
                    {
                        mailItem.Attachments = new List<Fanex.MailDeliver.Model.MailAttachment>();

                        foreach (var item in mailAttachment)
                        {
                            var attItem = new Fanex.MailDeliver.Model.MailAttachment
                            {
                                FileName = item.FileName,
                                Content = item.Content
                            };
                            if (item.ContentType.ToLower().IndexOf("excel") != -1)
                            {
                                attItem.ContentType = FileExtentions.MS_EXCEL;
                            }
                            else if (item.ContentType.ToLower().IndexOf("doc") != -1)
                            {
                                attItem.ContentType = FileExtentions.MS_DOC;
                            }
                            else
                            {
                                attItem.ContentType = FileExtentions.IMG_GIF;
                            }

                            mailItem.Attachments.Add(attItem);
                        }
                    }

                    if (mailSend.AltView != null)
                    {
                        mailItem.AlternateView = new List<MailLinkedResource>();
                        var linkedResource = mailSend.AltView.LinkedResources;
                        foreach (var linked in linkedResource)
                        {
                            var stream = linked.ContentStream;
                            byte[] buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                            var resource = new MailLinkedResource
                            {
                                FileName = string.Empty,
                                ImageType = FileExtentions.IMG_GIF,
                                ContentCID = linked.ContentId,
                                StreamContent = new MemoryStream(buffer)
                            };
                            mailItem.AlternateView.Add(resource);
                        }
                    }

                    // send mail
                    client.SendMail(mailItem);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return false;
        }
    }
}
