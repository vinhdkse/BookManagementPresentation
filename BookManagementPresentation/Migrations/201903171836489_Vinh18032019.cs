namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh18032019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "isDelete", c => c.Boolean());
            AddColumn("dbo.Payments", "isDelete", c => c.Boolean());
            AddColumn("dbo.Categories", "isDelete", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "isDelete");
            DropColumn("dbo.Payments", "isDelete");
            DropColumn("dbo.Authors", "isDelete");
        }
    }
}
