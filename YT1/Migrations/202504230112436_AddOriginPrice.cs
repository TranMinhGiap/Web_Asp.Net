﻿namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOriginPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Product", "OriginPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Product", "OriginPrice");
        }
    }
}
