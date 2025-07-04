﻿namespace ConTech.Migration._100;

[Migration(07)]
public class _0007_LevelChildTable : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table(Tables.LevelChild)
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("LevelId").AsGuid().NotNullable().ForeignKey(Tables.ViewLevel, "Id")
            .WithColumn("ParentId").AsGuid().NotNullable()
            .WithColumn("Name").AsString(StringLength.TwoHundred).NotNullable()
            .WithColumn("Description").AsString(StringLength.SevenHundredFifty).Nullable()
            .WithColumn("EntityList").AsString(StringLength.Max).Nullable()
            .ObjectStatus()
            .ChangeInfo();
    }
}