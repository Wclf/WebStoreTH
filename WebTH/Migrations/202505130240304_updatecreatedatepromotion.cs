namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecreatedatepromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Promotion", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_Promotion", "ModifiedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Promotion", "ModifiedDate");
            DropColumn("dbo.tb_Promotion", "CreatedDate");
        }
    }
}
