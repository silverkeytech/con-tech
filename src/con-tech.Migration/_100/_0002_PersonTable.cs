namespace ConTech.Migration._100;

[Migration(02)]
public class _0002_PersonTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table(Tables.Person)
            .AutoId()
            .WithColumn("FirstName").AsString(StringLength.TwoHundred).NotNullable()
            .WithColumn("LastName").AsString(StringLength.TwoHundred).NotNullable()
            .WithColumn("Email").AsString(StringLength.FiveHundred).Nullable()
            .WithColumn("Title").AsString(StringLength.Forty).Nullable()
            .WithColumn("Phone").AsString(StringLength.Forty).Nullable()
            .WithColumn("Verified").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("VerificationCode").AsString(4).Nullable()
            .ObjectStatus()
            .ChangeInfo();
    }
}