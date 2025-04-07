namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFKProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_ProductImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Image = c.String(),
                        isDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_ProductImages", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_ProductImages", new[] { "ProductId" });
            DropTable("dbo.tb_ProductImages");
        }
    }
}
