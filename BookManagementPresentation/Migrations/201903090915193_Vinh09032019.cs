namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh09032019 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Authors", "Description", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Authors", "Description", c => c.String());
        }
    }
}
