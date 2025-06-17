namespace ConTech.Migration._100;

[Migration(04)]
public class _0004_ProjectTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table(Tables.Project)
            .AutoId()
            .WithColumn("Name").AsString(StringLength.TwoHundred).NotNullable()
            .WithColumn("Description").AsString(StringLength.SevenHundredFifty).Nullable()
            .ObjectStatus()
            .ChangeInfo();
    }
}