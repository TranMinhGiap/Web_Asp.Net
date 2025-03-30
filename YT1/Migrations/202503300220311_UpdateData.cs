namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Category", "Alias", c => c.String(maxLength: 500));
            AddColumn("dbo.tb_News", "Alias", c => c.String());
            AddColumn("dbo.tb_Posts", "Alias", c => c.String());
            AddColumn("dbo.tb_Product", "Alias", c => c.String());
            AlterColumn("dbo.tb_Category", "Desc", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Category", "Desc", c => c.String(maxLength: 500));
            DropColumn("dbo.tb_Product", "Alias");
            DropColumn("dbo.tb_Posts", "Alias");
            DropColumn("dbo.tb_News", "Alias");
            DropColumn("dbo.tb_Category", "Alias");
        }
    }
}
