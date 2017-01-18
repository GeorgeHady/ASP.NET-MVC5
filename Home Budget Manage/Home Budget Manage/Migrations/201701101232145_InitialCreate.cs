namespace Home_Budget_Manage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounting Entries",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FK_FinancialAccounts_id = c.Int(nullable: false),
                        entryType = c.String(),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        details = c.String(),
                        date_byUser = c.DateTime(nullable: false),
                        User = c.String(),
                        date_inDatabse = c.String(),
                        TransferEntryCode = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Financial Accounts", t => t.FK_FinancialAccounts_id, cascadeDelete: true)
                .Index(t => t.FK_FinancialAccounts_id);
            
            CreateTable(
                "dbo.Financial Accounts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        accountName = c.String(nullable: false),
                        accountType = c.String(nullable: false),
                        otherDetails = c.String(),
                        registrationDate = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FK_Users_id = c.Int(nullable: false),
                        FK_FinancialAccounts_id = c.Int(nullable: false),
                        modificationPermission = c.Boolean(nullable: false),
                        useAccount = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Financial Accounts", t => t.FK_FinancialAccounts_id, cascadeDelete: true)
                .ForeignKey("dbo.Users Informatiom", t => t.FK_Users_id, cascadeDelete: true)
                .Index(t => t.FK_Users_id)
                .Index(t => t.FK_FinancialAccounts_id);
            
            CreateTable(
                "dbo.Users Informatiom",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        email = c.String(nullable: false),
                        userName = c.String(nullable: false),
                        password = c.String(nullable: false),
                        registrationDate = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "FK_Users_id", "dbo.Users Informatiom");
            DropForeignKey("dbo.Permissions", "FK_FinancialAccounts_id", "dbo.Financial Accounts");
            DropForeignKey("dbo.Accounting Entries", "FK_FinancialAccounts_id", "dbo.Financial Accounts");
            DropIndex("dbo.Permissions", new[] { "FK_FinancialAccounts_id" });
            DropIndex("dbo.Permissions", new[] { "FK_Users_id" });
            DropIndex("dbo.Accounting Entries", new[] { "FK_FinancialAccounts_id" });
            DropTable("dbo.Users Informatiom");
            DropTable("dbo.Permissions");
            DropTable("dbo.Financial Accounts");
            DropTable("dbo.Accounting Entries");
        }
    }
}
