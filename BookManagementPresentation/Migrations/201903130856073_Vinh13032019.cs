namespace BookManagementPresentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vinh13032019 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CityId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProvinceId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Provinces", t => t.ProvinceId, cascadeDelete: true)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DonDatHangs", "DistrictId", c => c.Int(nullable: true));
            AddColumn("dbo.DonDatHangs", "PaymentId", c => c.Int(nullable: true));
            CreateIndex("dbo.DonDatHangs", "DistrictId");
            CreateIndex("dbo.DonDatHangs", "PaymentId");
            AddForeignKey("dbo.DonDatHangs", "DistrictId", "dbo.Districts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DonDatHangs", "PaymentId", "dbo.Payments", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DonDatHangs", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.DonDatHangs", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Cities", "ProvinceId", "dbo.Provinces");
            DropForeignKey("dbo.Districts", "CityId", "dbo.Cities");
            DropIndex("dbo.Cities", new[] { "ProvinceId" });
            DropIndex("dbo.Districts", new[] { "CityId" });
            DropIndex("dbo.DonDatHangs", new[] { "PaymentId" });
            DropIndex("dbo.DonDatHangs", new[] { "DistrictId" });
            DropColumn("dbo.DonDatHangs", "PaymentId");
            DropColumn("dbo.DonDatHangs", "DistrictId");
            DropTable("dbo.Provinces");
            DropTable("dbo.Cities");
            DropTable("dbo.Districts");
        }
    }
}
