namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentVn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "PaymentMethodVn", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "PaymentMethodVn");
        }
    }
}
