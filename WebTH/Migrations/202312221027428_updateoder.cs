namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateoder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Order", "Address", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Order", "Address", c => c.String(nullable: false));
        }
    }
}
