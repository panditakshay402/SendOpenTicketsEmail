namespace SendOpenTicketsEmail.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeDateTimesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "RaisedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "RaisedDate", c => c.DateTime(nullable: false));
        }
    }
}
