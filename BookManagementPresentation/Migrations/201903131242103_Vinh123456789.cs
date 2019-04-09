namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh123456789 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DonDatHangs", "Total", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DonDatHangs", "Total");
        }
    }
}
