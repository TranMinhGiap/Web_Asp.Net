namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Email", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "Email");
        }
    }
}
