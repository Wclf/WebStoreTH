namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletestartdatepromotion : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tb_Promotion", "StartDate");
            DropColumn("dbo.tb_Promotion", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Promotion", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_Promotion", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
