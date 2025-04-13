namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAntOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Order", "CustomerName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.tb_Order", "Phone", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.tb_Order", "Address", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Order", "Address", c => c.String());
            AlterColumn("dbo.tb_Order", "Phone", c => c.String());
            AlterColumn("dbo.tb_Order", "CustomerName", c => c.String());
        }
    }
}
