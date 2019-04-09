namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh12345678910111213 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DonDatHangs", "DistrictId", "dbo.Districts");
            DropIndex("dbo.DonDatHangs", new[] { "DistrictId" });
            AddColumn("dbo.DonDatHangs", "Address", c => c.String());
            DropColumn("dbo.DonDatHangs", "DistrictId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DonDatHangs", "DistrictId", c => c.Int(nullable: false));
            DropColumn("dbo.DonDatHangs", "Address");
            CreateIndex("dbo.DonDatHangs", "DistrictId");
            AddForeignKey("dbo.DonDatHangs", "DistrictId", "dbo.Districts", "Id", cascadeDelete: true);
        }
    }
}
