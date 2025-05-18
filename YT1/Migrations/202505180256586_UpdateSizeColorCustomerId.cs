namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSizeColorCustomerId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_OrderDetail", "Size", c => c.String());
            AddColumn("dbo.tb_OrderDetail", "Color", c => c.String());
            AddColumn("dbo.tb_Order", "CustomerId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "CustomerId");
            DropColumn("dbo.tb_OrderDetail", "Color");
            DropColumn("dbo.tb_OrderDetail", "Size");
        }
    }
}
