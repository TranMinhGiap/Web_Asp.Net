﻿namespace YT1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAntTbSubcribe : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Subcribe", "Email", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Subcribe", "Email", c => c.String());
        }
    }
}
