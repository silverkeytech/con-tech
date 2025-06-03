namespace con_tech.Migration._100;

[Migration(09)]
public class _0009_LevelEntityTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table(Tables.LevelEntity)
            .AutoId()
            .WithColumn("LevelId").AsGuid().NotNullable().ForeignKey(Tables.ViewLevel, "Id")
            .IntForeignKeyIndexed("EntityTypeId", Tables.EntityType, isNullable: false, isPK: false)
            .WithColumn("EntityName").AsString(StringLength.TwoHundred).NotNullable()
            .WithColumn("UniqueId").AsString(StringLength.SevenHundredFifty).Nullable()
            .WithColumn("ColorCode").AsString(StringLength.Twenty).Nullable()
            .WithColumn("Visible").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Frozen").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("ColorIndex").AsInt32().NotNullable()
            .WithColumn("Vertices").AsString(StringLength.Thousand).Nullable()
            .ObjectStatus()
            .ChangeInfo();
    }
}