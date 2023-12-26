namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updateadress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Address", c => c.String(nullable: false));
            DropColumn("dbo.tb_Order", "Adress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Order", "Adress", c => c.String(nullable: false));
            DropColumn("dbo.tb_Order", "Address");
        }
    }
}
