namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh123abcd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DonDatHangs", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DonDatHangs", "Phone");
        }
    }
}
