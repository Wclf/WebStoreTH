namespace WebTH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatealiaspromoiton : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Promotion", "Alias", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Promotion", "Alias");
        }
    }
}
