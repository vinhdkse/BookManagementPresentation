namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh19032019123456 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "DateSend", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "DateSend");
        }
    }
}
