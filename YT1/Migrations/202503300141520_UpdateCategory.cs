namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCategory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Category", "Title", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.tb_Category", "Desc", c => c.String(maxLength: 500));
            AlterColumn("dbo.tb_Category", "SeoTitle", c => c.String(maxLength: 500));
            AlterColumn("dbo.tb_Category", "SeoDesc", c => c.String(maxLength: 500));
            AlterColumn("dbo.tb_Category", "SeoKeyWord", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Category", "SeoKeyWord", c => c.String());
            AlterColumn("dbo.tb_Category", "SeoDesc", c => c.String());
            AlterColumn("dbo.tb_Category", "SeoTitle", c => c.String());
            AlterColumn("dbo.tb_Category", "Desc", c => c.String());
            AlterColumn("dbo.tb_Category", "Title", c => c.String());
        }
    }
}
