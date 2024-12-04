using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendOpenTicketsEmail
{
    public class OpenTicket
    {
        public int TicketId { get; set; }
        public int ClientId { get; set; }
        public string Clientname { get; set; }
        public string Subject { get; set; }
        public DateTime? RaisedDate { get; set; }
        public DateTime? ExpectedResolutionDate { get; set; }
        public int TicketNo { get; set; }  
        public decimal? CCFNO { get; set; }  
    }

}
