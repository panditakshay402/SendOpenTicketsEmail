using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using SendOpenTicketsEmail;
using System.Data.SqlClient;
using System.Data.Entity;


class Program
{
    
static void Main(string[] args)
{
    using (var context = new IMSDbContext())
    {
            var openTickets = context.Database
             .SqlQuery<OpenTicket>("EXEC [dbo].[sp_GetAllOpenTickets]").ToList();

            if (openTickets.Any())
        {
            Console.WriteLine("Open tickets found. Preparing to send email...");
            SendEmail(openTickets);
        }
        else
        {
            Console.WriteLine("No open tickets to send.");
        }
    }
}

    public static void SendEmail(IEnumerable<OpenTicket> openTickets)
    {
        var emailBody = new StringBuilder();
        emailBody.AppendLine("<h3>Open Tickets</h3>");
        emailBody.AppendLine("<table border='1' style='border-collapse: collapse; width: 100%;'>");
        emailBody.AppendLine("<tr><th>Clients</th><th>Subject</th><th>Raised Date</th><th>Resolution Date</th><th>Ticket No.</th><th>CCF No.</th><th>Status</th></tr>");

        foreach (var ticket in openTickets)
        {
            emailBody.AppendLine($"<tr>");
            emailBody.AppendLine($"<td>{ticket.Clientname}</td>");
            emailBody.AppendLine($"<td>{ticket.Subject}</td>");
            emailBody.AppendLine($"<td>{ticket.RaisedDate:dd/MMM/yyyy}</td>");
            emailBody.AppendLine($"<td>{ticket.ExpectedResolutionDate:dd/MMM/yyyy}</td>");
            emailBody.AppendLine($"<td>{ticket.TicketNo}</td>");
            emailBody.AppendLine($"<td>{ticket.CCFNO}</td>");
            emailBody.AppendLine($"<td>Open</td>");
            emailBody.AppendLine($"</tr>");
        }
        emailBody.AppendLine("</table>");

        var mailMessage = new MailMessage
        {
            From = new MailAddress("equiflow.equitec@gmail.com"),
            Subject = "Open Tickets Report",
            Body = emailBody.ToString(),
            IsBodyHtml = true
        };
        mailMessage.To.Add("heyaaru2000@gmail.com");

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new System.Net.NetworkCredential("equiflow.equitec@gmail.com", "eiiw nkki ezag gasb"),
            EnableSsl = true
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
