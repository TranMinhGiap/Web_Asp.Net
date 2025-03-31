namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromDetail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_News", "Detail", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_News", "Detail", c => c.String(nullable: false));
        }
    }
}
