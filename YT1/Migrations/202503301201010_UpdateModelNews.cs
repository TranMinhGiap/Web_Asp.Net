namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelNews : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_News", "Title", c => c.String(nullable: false, maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_News", "Title", c => c.String());
        }
    }
}
