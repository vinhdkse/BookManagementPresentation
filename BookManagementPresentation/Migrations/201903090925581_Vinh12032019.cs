namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh12032019 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Carts", "CartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Carts", "CartDate", c => c.DateTime(nullable: false));
        }
    }
}
