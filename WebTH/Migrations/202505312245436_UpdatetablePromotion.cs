namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatetablePromotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Promotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Alias = c.String(nullable: false, maxLength: 150),
                        Description = c.String(),
                        DiscountPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tb_Product", "PromotionId", c => c.Int());
            CreateIndex("dbo.tb_Product", "PromotionId");
            AddForeignKey("dbo.tb_Product", "PromotionId", "dbo.tb_Promotion", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Product", "PromotionId", "dbo.tb_Promotion");
            DropIndex("dbo.tb_Product", new[] { "PromotionId" });
            DropColumn("dbo.tb_Product", "PromotionId");
            DropTable("dbo.tb_Promotion");
        }
    }
}
