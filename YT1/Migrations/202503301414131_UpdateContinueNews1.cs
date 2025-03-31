namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContinueNews1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_News", "Description", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.tb_News", "Detail", c => c.String(nullable: true));
            AlterColumn("dbo.tb_News", "Images", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_News", "Images", c => c.String());
            AlterColumn("dbo.tb_News", "Detail", c => c.String());
            AlterColumn("dbo.tb_News", "Description", c => c.String());
        }
    }
}
