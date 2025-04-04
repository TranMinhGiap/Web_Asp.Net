﻿namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAntPrdCategory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_ProductCategory", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.tb_ProductCategory", "Description", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_ProductCategory", "Description", c => c.String());
            AlterColumn("dbo.tb_ProductCategory", "Title", c => c.String());
        }
    }
}
