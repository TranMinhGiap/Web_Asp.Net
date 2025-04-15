namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStatistical : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Statisticals", newName: "tb_ThongKe");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.tb_ThongKe", newName: "Statisticals");
        }
    }
}
