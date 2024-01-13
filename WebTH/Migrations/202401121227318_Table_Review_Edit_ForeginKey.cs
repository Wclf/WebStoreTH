namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table_Review_Edit_ForeginKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tb_Review", "ProductId");
            AddForeignKey("dbo.tb_Review", "ProductId", "dbo.tb_Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Review", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_Review", new[] { "ProductId" });
        }
    }
}
