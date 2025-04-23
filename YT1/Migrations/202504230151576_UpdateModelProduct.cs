namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Product", "Size", c => c.String());
            AddColumn("dbo.tb_Product", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Product", "Color");
            DropColumn("dbo.tb_Product", "Size");
        }
    }
}
