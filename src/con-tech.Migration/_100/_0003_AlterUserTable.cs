namespace ConTech.Migration._100;

[Migration(03)]
public class _0003_AlterUserTable : FluentMigrator.Migration
{
    public override void Down()
    {
        if (Schema.Table(Tables.User).Column("PersonId").Exists())
        {
            Delete.ForeignKey().FromTable(Tables.User).ForeignColumn("PersonId").ToTable(Tables.Person).PrimaryColumn("Id");
        }
    }

    public override void Up()
    {
        Alter.Table(Tables.User)
             .IntForeignKeyIndexed("PersonId", Tables.Person, isNullable: false, isPK: false)
             .AddColumn("ResetToken").AsGuid().Nullable()
             .AddColumn("ResetExpirationDate").AsDateTime().Nullable()
             .ObjectStatus()
             .ChangeInfo();
    }
}