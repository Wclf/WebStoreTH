namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPromotionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Product", "PromotionId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Product", "PromotionId");
        }
    }
}
