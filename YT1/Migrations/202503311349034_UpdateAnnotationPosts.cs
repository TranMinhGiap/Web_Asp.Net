namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAnnotationPosts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Posts", "Title", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.tb_Posts", "Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Posts", "Description", c => c.String());
            AlterColumn("dbo.tb_Posts", "Title", c => c.String());
        }
    }
}
