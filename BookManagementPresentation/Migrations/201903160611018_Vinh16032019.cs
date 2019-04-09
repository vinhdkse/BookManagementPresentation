namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh16032019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MyUsers", "Serect", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MyUsers", "Serect");
        }
    }
}
