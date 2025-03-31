namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequireImagesNews : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_News", "Images", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_News", "Images", c => c.String(nullable: false));
        }
    }
}
