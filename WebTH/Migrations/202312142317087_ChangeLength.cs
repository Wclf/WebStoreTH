namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Posts", "Alias", c => c.String(maxLength: 150));
            AlterColumn("dbo.tb_Posts", "Image", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Posts", "SeoTitle", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Posts", "SepDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.tb_Posts", "SeoKeywords", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Product", "Title", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.tb_Product", "Alias", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Product", "ProductCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.tb_Product", "Image", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Product", "SeoTitle", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Product", "SepDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.tb_Product", "SeoKeywords", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Product", "SeoKeywords", c => c.String());
            AlterColumn("dbo.tb_Product", "SepDescription", c => c.String());
            AlterColumn("dbo.tb_Product", "SeoTitle", c => c.String());
            AlterColumn("dbo.tb_Product", "Image", c => c.String());
            AlterColumn("dbo.tb_Product", "ProductCode", c => c.String());
            AlterColumn("dbo.tb_Product", "Alias", c => c.String());
            AlterColumn("dbo.tb_Product", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.tb_Posts", "SeoKeywords", c => c.String());
            AlterColumn("dbo.tb_Posts", "SepDescription", c => c.String());
            AlterColumn("dbo.tb_Posts", "SeoTitle", c => c.String());
            AlterColumn("dbo.tb_Posts", "Image", c => c.String());
            AlterColumn("dbo.tb_Posts", "Alias", c => c.String());
        }
    }
}
