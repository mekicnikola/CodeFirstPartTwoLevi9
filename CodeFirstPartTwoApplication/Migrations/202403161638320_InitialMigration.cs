namespace CodeFirstPartTwoApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarId = c.Int(nullable: false, identity: true),
                        Color = c.String(),
                        Year = c.Int(nullable: false),
                        ChassisNumber = c.String(),
                        Brand = c.String(),
                        Model = c.String(),
                    })
                .PrimaryKey(t => t.CarId);
            
            CreateTable(
                "dbo.Engines",
                c => new
                    {
                        EngineId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Brand = c.String(),
                        SerialNumber = c.String(),
                        Type = c.String(),
                        EngineTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EngineId)
                .ForeignKey("dbo.EngineTypes", t => t.EngineTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Cars", t => t.EngineId)
                .Index(t => t.EngineId)
                .Index(t => t.EngineTypeId);
            
            CreateTable(
                "dbo.EngineTypes",
                c => new
                    {
                        EngineTypeId = c.Int(nullable: false, identity: true),
                        Model = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.EngineTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Engines", "EngineId", "dbo.Cars");
            DropForeignKey("dbo.Engines", "EngineTypeId", "dbo.EngineTypes");
            DropIndex("dbo.Engines", new[] { "EngineTypeId" });
            DropIndex("dbo.Engines", new[] { "EngineId" });
            DropTable("dbo.EngineTypes");
            DropTable("dbo.Engines");
            DropTable("dbo.Cars");
        }
    }
}
