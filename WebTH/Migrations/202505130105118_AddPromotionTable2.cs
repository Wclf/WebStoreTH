namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromotionTable2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Promotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Description = c.String(),
                        DiscountPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.tb_Product", "PromotionId");
            AddForeignKey("dbo.tb_Product", "PromotionId", "dbo.tb_Promotion", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Product", "PromotionId", "dbo.tb_Promotion");
            DropIndex("dbo.tb_Product", new[] { "PromotionId" });
            DropTable("dbo.tb_Promotion");
        }
    }
}
