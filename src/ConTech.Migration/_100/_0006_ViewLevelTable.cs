namespace ConTech.Migration._100;

[Migration(06)]
public class _0006_ViewLevelTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table(Tables.ViewLevel)
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .IntForeignKeyIndexed("ViewId", Tables.ProjectView, isNullable: false, isPK: false)
            .WithColumn("Name").AsString(StringLength.TwoHundred).NotNullable()
            .WithColumn("Description").AsString(StringLength.SevenHundredFifty).Nullable()
            .WithColumn("DxfData").AsString(StringLength.Max).Nullable()
            .WithColumn("EntityData").AsString(StringLength.Max).Nullable()
            .WithColumn("DxfFile").AsCustom(SqlServerSpecificType.VarBinaryMax).Nullable()
            .WithColumn("excelFile").AsCustom(SqlServerSpecificType.VarBinaryMax).Nullable()
            .WithColumn("Scale").AsFloat().Nullable()
            .WithColumn("TransitionX").AsFloat().Nullable()
            .WithColumn("TransitionY").AsFloat().Nullable()
            .ObjectStatus()
            .ChangeInfo();
    }
}