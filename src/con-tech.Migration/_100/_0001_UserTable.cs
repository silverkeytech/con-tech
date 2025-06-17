namespace ConTech.Migration._100;

[Migration(1)]
public class _0001_UserTable : AutoReversingMigration
{
    public override void Up()
    {
        //DG: This is temporary
        Create.Table(Tables.User)
            .AutoId()
            .WithColumn("Password").AsString(StringLength.TwoHundred).NotNullable();
    }
}