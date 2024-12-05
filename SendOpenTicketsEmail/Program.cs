using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace SendOpenTicketsEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["IMSConnectionString"].ConnectionString;
            var filteredCCFs = GetFilteredCCFs(connectionString);

            if (filteredCCFs.Any())
            {
                Console.WriteLine("Filtered open CCFs found. Preparing to send email...");
                SendEmail(filteredCCFs);
            }
            else
            {
                Console.WriteLine("No open CCFs to send.");
            }
        }

        public static List<OpenCCF> GetFilteredCCFs(string connectionString)
        {
            var openCCFs = new List<OpenCCF>();

            string query = "SELECT c.ClientId, cl.Name AS ClientName, c.Subject, c.RaisedDate, c.CCFNo, c.TicketId " +
                           "FROM CCFs c " +
                           "INNER JOIN Clients cl ON c.ClientId = cl.Id " +
                           "WHERE c.Status = 'Open' AND c.Hide = 'N' " +
                           "ORDER BY c.RaisedDate DESC";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ccf = new OpenCCF
                            {
                                ClientName = reader["ClientName"].ToString(),
                                Subject = reader["Subject"].ToString(),
                                RaisedDate = reader["RaisedDate"] as DateTime?,
                                CCFNo = reader["CCFNo"] as decimal?,
                                TicketId = (int)reader["TicketId"]
                            };
                            openCCFs.Add(ccf);
                        }
                    }
                }
            }

            return openCCFs;
        }

        public static void SendEmail(IEnumerable<OpenCCF> openCCFs)
        {
            var emailBody = new StringBuilder();

            emailBody.AppendLine("<h3>Dear Team,</h3>");
            emailBody.AppendLine("<p>Please find below the list of open CCFs as of the latest update. These tickets are currently being reviewed and need attention. Kindly review and take the necessary actions.</p>");

            // Report heading and table structure
            emailBody.AppendLine("<h4>Weekly Open Tickets Report</h4>");
            emailBody.AppendLine("<table border='1' style='border-collapse: collapse; width: 100%; border: 1px solid #ddd;'>");

            // Table header with background color and padding for alignment
            emailBody.AppendLine("<tr style='background-color: #f4f4f4; color: #333;'>");
            emailBody.AppendLine("<th style='text-align: left; padding: 8px; width: 20%;'>Client Name</th>");
            emailBody.AppendLine("<th style='text-align: left; padding: 8px; width: 25%;'>Subject</th>");
            emailBody.AppendLine("<th style='text-align: center; padding: 8px; width: 10%;'>Raised Date</th>");
            emailBody.AppendLine("<th style='text-align: center; padding: 8px; width: 9%;'>CCF No.</th>");
            emailBody.AppendLine("<th style='text-align: center; padding: 8px; width: 9%;'>Ticket Id</th>");
            emailBody.AppendLine("<th style='text-align: center; padding: 8px; width: 8%;'>Status</th>");
            emailBody.AppendLine("</tr>");

            // Loop to add each CCF's details in the table
            foreach (var ccf in openCCFs)
            {
                emailBody.AppendLine("<tr style='background-color: #fff; color: #333;'>");
                emailBody.AppendLine($"<td style='text-align: left; padding: 8px; width: 20%;'>{ccf.ClientName}</td>");
                emailBody.AppendLine($"<td style='text-align: left; padding: 8px; width: 25%;'>{ccf.Subject}</td>");
                emailBody.AppendLine($"<td style='text-align: center; padding: 8px; width: 10%;'>{ccf.RaisedDate:dd/MMM/yyyy}</td>");
                emailBody.AppendLine($"<td style='text-align: center; padding: 8px; width: 9%;'>{(ccf.CCFNo.HasValue ? ccf.CCFNo.Value.ToString("F0") : "N/A")}</td>");
                emailBody.AppendLine($"<td style='text-align: center; padding: 8px; width: 9%;'>{ccf.TicketId}</td>");
                emailBody.AppendLine("<td style='text-align: center; padding: 8px; width: 8%;'>Open</td>");
                emailBody.AppendLine("</tr>");
            }

            // Close the table
            emailBody.AppendLine("</table>");

            // Closing message
            emailBody.AppendLine("<p>Should you need further details or have any questions regarding these open tickets, please do not hesitate to reach out.</p>");
            emailBody.AppendLine("<p>Best Regards,<br>Your Support Team</p>");

            // Retrieve SMTP settings from app.config
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"];
            var smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
            var smtpUsername = ConfigurationManager.AppSettings["SMTPUsername"];
            var smtpPassword = ConfigurationManager.AppSettings["SMTPPassword"];
            var emailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            var emailTo = ConfigurationManager.AppSettings["EmailTo"];

            // Create and configure the email message
            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailFrom),
                Subject = "Weekly Open Tickets Report",  // Updated email subject
                Body = emailBody.ToString(),
                IsBodyHtml = true
            };
            mailMessage.To.Add(emailTo);

            // SMTP client setup
            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SMTPSsl"])
            };

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
