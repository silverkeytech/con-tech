namespace ConTech.Migration._100;

[Migration(05)]
public class _0005_ProjectViewTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table(Tables.ProjectView)
            .AutoId()
            .IntForeignKeyIndexed("ProjectId", Tables.Project, isNullable: false, isPK: false)
            .WithColumn("Name").AsString(StringLength.TwoHundred).NotNullable()
            .WithColumn("Description").AsString(StringLength.SevenHundredFifty).Nullable()
            .WithColumn("BackgroundPdf").AsCustom(SqlServerSpecificType.VarBinaryMax).Nullable()
            .ObjectStatus()
            .ChangeInfo();
    }
}