namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh120320192 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MyUsers", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MyUsers", "Email");
        }
    }
}