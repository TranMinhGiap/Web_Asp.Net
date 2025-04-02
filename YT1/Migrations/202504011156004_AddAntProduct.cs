namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAntProduct : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Product", "Title", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.tb_Product", "ProductCode", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.tb_Product", "Description", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Product", "Description", c => c.String());
            AlterColumn("dbo.tb_Product", "ProductCode", c => c.String());
            AlterColumn("dbo.tb_Product", "Title", c => c.String());
        }
    }
}
