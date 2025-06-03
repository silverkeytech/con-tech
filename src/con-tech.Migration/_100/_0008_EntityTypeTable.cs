namespace con_tech.Migration._100;

[Migration(08)]
public class _0008_EntityTypeTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table(Tables.EntityType)
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
            .WithColumn("NameArabic").AsString(StringLength.TwoHundred).NotNullable()
            .WithColumn("NameEnglish").AsString(StringLength.TwoHundred).Nullable()
            .ObjectStatus()
            .ChangeInfo();
    }
}