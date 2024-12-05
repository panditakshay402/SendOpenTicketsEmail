using System;

namespace SendOpenTicketsEmail
{
    public class OpenCCF
    {
        public string ClientName { get; set; }  
        public string Subject { get; set; }
        public DateTime? RaisedDate { get; set; }
        public decimal? CCFNo { get; set; }
        public int TicketId { get; set; }
    }
}

