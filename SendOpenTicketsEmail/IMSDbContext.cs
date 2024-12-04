using System;
using System.Data.Entity;

public class IMSDbContext : DbContext
{
    public IMSDbContext() : base("name=IMSConnectionString") { }

    public DbSet<Ticket> Tickets { get; set; }
}

public class Ticket
{
    public int TicketId { get; set; }  
    public string Clientname { get; set; }  
    public string Subject { get; set; } 
    public DateTime? RaisedDate { get; set; } 
    public DateTime? ExpectedResolutionDate { get; set; } 
    public int TicketNo { get; set; }
    public decimal? CCFNO { get; set; } 
    public string Status { get; set; } 
}
