using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Linq;

namespace LogMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            string logDirectory = @"C:\River\WinService\Logs";
            long maxFileSize = 10 * 1024 * 1024; // 10 MB

            // Klasördeki tüm .txt dosyalarını kontrol et
            var logFiles = Directory.GetFiles(logDirectory, "*.txt");

            foreach (var file in logFiles)
            {
                FileInfo fileInfo = new FileInfo(file);

                if (fileInfo.Length > maxFileSize)
                {
                    SendEmailNotification(fileInfo.FullName, fileInfo.Length);
                }
            }
            Environment.Exit(0);
        }

        static void SendEmailNotification(string fileName, long fileSize)
        {
            string smtpServer = "smtp.office365.com";
            string smtpUser = "...";
            string smtpPass = "...";
            string fromEmail = "...";
            string toEmail = "koray@anv.com.tr";
            string subject = "Agroglob River Log Dosyası Boyutu Uyarısı!";
            string body = $"Log dosyası {fileName} 10 MB'yi aştı.\nBoyut: {fileSize / (1024 * 1024)} MB";

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body
            };

            mail.To.Add(toEmail);

            SmtpClient smtpClient = new SmtpClient(smtpServer)
            {
                Port = 587, 
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            try
            {
                smtpClient.Send(mail);
                Console.WriteLine($"Email sent regarding file {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
