namespace SendOpenTicketsEmail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTicketModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "CCFNO", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "CCFNO", c => c.String());
        }
    }
}
