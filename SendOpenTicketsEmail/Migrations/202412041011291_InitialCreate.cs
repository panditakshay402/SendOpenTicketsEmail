namespace SendOpenTicketsEmail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        Clientname = c.String(),
                        Subject = c.String(),
                        RaisedDate = c.DateTime(nullable: false),
                        ExpectedResolutionDate = c.DateTime(),
                        TicketNo = c.String(),
                        CCFNO = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.TicketId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tickets");
        }
    }
}
