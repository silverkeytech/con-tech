﻿using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;

namespace ConTech.Migration;

public record NameEnglishAndArabic(string nameEnglish, string nameArabic);
public static class Constants
{
    public const int CreatedByUserId = 1;
}

public static class StringLength
{
    public const int One = 1;
    public const int Two = 2;
    public const int Three = 3;
    public const int Four = 4;
    public const int Five = 5;
    public const int Twelve = 12;
    public const int Thirteen = 13;
    public const int Fourteen = 14;
    public const int Fifteen = 15;
    public const int Twenty = 20;
    public const int Thirty = 30;
    public const int Forty = 40;
    public const int Fifty = 50;
    public const int FiftyFive = 55;
    public const int OneHundred = 100;
    public const int TwoHundred = 200;
    public const int TwoHundredFiftyFive = 255;
    public const int FourHundredFifty = 450;
    public const int FiveHundred = 500;
    public const int FiveHundredFifty = 550;
    public const int SevenHundredFifty = 750;
    public const int Thousand = 1000;
    public const int SuperExtraLong = 30000;
    public const int Max = SuperExtraLong * 10;
}

public static class SqlServerSpecificType
{
    public const string Identity = "Int GENERATED BY DEFAULT AS IDENTITY";
    public const string VarBinaryMax = "varbinary(MAX)";

    public static string NVarChar(int? length = null)
    {
        if (length == null)
            return "NVARCHAR(MAX)";
        else
            return $"NVARCHAR({length})";
    }
}

internal static class SqlServerFunction
{
    // https://www.techonthenet.com/sql_server/functions/getutcdate.php
    public static readonly ServerFunction NowTimestampUTC = new("GETUTCDATE()");

    // https://docs.microsoft.com/en-us/sql/t-sql/functions/newid-transact-sql?view=sql-server-ver15
    public static readonly ServerFunction GenerateRandomUuidV4 = new("NEWID()");
}

internal class ServerFunction
{
    private string _functionName;

    public ServerFunction(string functionName)
    {
        _functionName = functionName;
    }

    public override string ToString()
    {
        return _functionName;
    }
}

public static class FluentMigratorExtensions
{
    public static ICreateTableColumnOptionOrWithColumnSyntax AutoSmallId(this ICreateTableWithColumnOrSchemaSyntax self) =>
        self.WithColumn("Id").AsInt16().PrimaryKey().Identity().NotNullable();

    public static ICreateTableColumnOptionOrWithColumnSyntax AutoId(this ICreateTableWithColumnOrSchemaSyntax self) =>
        self.WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable();

    public static ICreateTableColumnOptionOrWithColumnSyntax AutoBigId(this ICreateTableWithColumnOrSchemaSyntax self) =>
        self.WithColumn("Id").AsInt64().PrimaryKey().Identity().NotNullable();

    public static ICreateTableColumnOptionOrWithColumnSyntax SmallId(this ICreateTableWithColumnOrSchemaSyntax self) =>
        self.WithColumn("Id").AsInt16().PrimaryKey().NotNullable();

    public static ICreateTableColumnOptionOrWithColumnSyntax IntId(this ICreateTableWithColumnOrSchemaSyntax self) =>
        self.WithColumn("Id").AsInt32().PrimaryKey().NotNullable();

    public static ICreateTableColumnOptionOrWithColumnSyntax BigId(this ICreateTableWithColumnOrSchemaSyntax self) =>
        self.WithColumn("Id").AsInt64().PrimaryKey().NotNullable();

    public static ICreateTableColumnOptionOrWithColumnSyntax AutoGuidId(this ICreateTableWithColumnOrSchemaSyntax self) =>
        self.WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid);

    public static ICreateTableColumnOptionOrWithColumnSyntax IntForeignKeyIndexed(this ICreateTableColumnOptionOrWithColumnSyntax self,
        string name, string foreignTable, bool isNullable, bool isPK)
    {
        ICreateTableColumnOptionOrWithColumnSyntax col;

        if (isNullable)
            col = self.WithColumn(name).AsInt32().Nullable().ForeignKey(foreignTable, "Id");
        else
            col = self.WithColumn(name).AsInt32().NotNullable().ForeignKey(foreignTable, "Id");

        if (isPK)
            col.PrimaryKey();
        else
            col.Indexed();

        return self;
    }

    public static ICreateTableColumnOptionOrWithColumnSyntax SmallIntForeignKeyIndexed(this ICreateTableColumnOptionOrWithColumnSyntax self,
        string name, string foreignTable, bool isNullable, bool isPK)
    {
        ICreateTableColumnOptionOrWithColumnSyntax col;

        if (isNullable)
            col = self.WithColumn(name).AsInt16().Nullable().ForeignKey(foreignTable, "Id");
        else
            col = self.WithColumn(name).AsInt16().NotNullable().ForeignKey(foreignTable, "Id");

        if (isPK)
            col.PrimaryKey();
        else
            col.Indexed();

        return self;
    }

    public static ICreateTableColumnOptionOrWithColumnSyntax BigIntForeignKeyIndexed(this ICreateTableColumnOptionOrWithColumnSyntax self,
        string name, string foreignTable, bool isNullable, bool isPK)
    {
        ICreateTableColumnOptionOrWithColumnSyntax col;

        if (isNullable)
            col = self.WithColumn(name).AsInt64().Nullable().ForeignKey(foreignTable, "Id");
        else
            col = self.WithColumn(name).AsInt64().NotNullable().ForeignKey(foreignTable, "Id");

        if (isPK)
            col.PrimaryKey();
        else
            col.Indexed();

        return self;
    }

    public static ICreateTableWithColumnOrSchemaSyntax IntForeignKeyIndexed(this ICreateTableWithColumnOrSchemaSyntax self,
      string name, string foreignTable, bool isNullable, bool isPK)
    {
        ICreateTableColumnOptionOrWithColumnSyntax col;

        if (isNullable)
            col = self.WithColumn(name).AsInt32().Nullable().ForeignKey(foreignTable, "Id").Indexed();
        else
            col = self.WithColumn(name).AsInt32().NotNullable().ForeignKey(foreignTable, "Id").Indexed();

        if (isPK)
            col.PrimaryKey();
        else
            col.Indexed();

        return self;
    }

    public static ICreateTableWithColumnOrSchemaSyntax SmallIntForeignKeyIndexed(this ICreateTableWithColumnOrSchemaSyntax self,
      string name, string foreignTable, bool isNullable, bool isPK)
    {
        ICreateTableColumnOptionOrWithColumnSyntax col;

        if (isNullable)
            col = self.WithColumn(name).AsInt16().Nullable().ForeignKey(foreignTable, "Id").Indexed();
        else
            col = self.WithColumn(name).AsInt16().NotNullable().ForeignKey(foreignTable, "Id").Indexed();

        if (isPK)
            col.PrimaryKey();
        else
            col.Indexed();

        return self;
    }

    public static IAlterTableAddColumnOrAlterColumnOrSchemaOrDescriptionSyntax IntForeignKeyIndexed(this IAlterTableAddColumnOrAlterColumnOrSchemaOrDescriptionSyntax self,
     string name, string foreignTable, bool isNullable, bool isPK)
    {
        IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax col;

        if (isNullable)
            col = self.AddColumn(name).AsInt32().Nullable().ForeignKey(foreignTable, "Id");
        else
            col = self.AddColumn(name).AsInt32().NotNullable().ForeignKey(foreignTable, "Id");

        if (isPK)
            col.PrimaryKey();
        else
            col.Indexed();

        return self;
    }

    public static IAlterTableAddColumnOrAlterColumnOrSchemaOrDescriptionSyntax BigForeignKeyIndexed(this IAlterTableAddColumnOrAlterColumnOrSchemaOrDescriptionSyntax self,
     string name, string foreignTable, bool isNullable, bool isPK)
    {
        IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax col;

        if (isNullable)
            col = self.AddColumn(name).AsInt64().Nullable().ForeignKey(foreignTable, "Id");
        else
            col = self.AddColumn(name).AsInt64().NotNullable().ForeignKey(foreignTable, "Id");

        if (isPK)
            col.PrimaryKey();
        else
            col.Indexed();

        return self;
    }

    public static IAlterTableAddColumnOrAlterColumnOrSchemaOrDescriptionSyntax SmallIntForeignKeyIndexed(this IAlterTableAddColumnOrAlterColumnOrSchemaOrDescriptionSyntax self,
     string name, string foreignTable, bool isNullable, bool isPK)
    {
        IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax col;

        if (isNullable)
            col = self.AddColumn(name).AsInt16().Nullable().ForeignKey(foreignTable, "Id");
        else
            col = self.AddColumn(name).AsInt16().NotNullable().ForeignKey(foreignTable, "Id");

        if (isPK)
            col.PrimaryKey();
        else
            col.Indexed();

        return self;
    }

    public static ICreateTableColumnOptionOrWithColumnSyntax ObjectStatus(this ICreateTableWithColumnOrSchemaSyntax self) =>
        self.WithColumn("ObjectStatus").AsInt32().NotNullable().WithDefaultValue(1).Indexed();

    public static ICreateTableColumnOptionOrWithColumnSyntax ObjectStatus(this ICreateTableColumnOptionOrWithColumnSyntax self) =>
        self.WithColumn("ObjectStatus").AsInt32().NotNullable().WithDefaultValue(1).Indexed();

    public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax ObjectStatus(this IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax self) =>
        self.AddColumn("ObjectStatus").AsInt32().NotNullable().WithDefaultValue(1).Indexed();

    public static ICreateTableColumnOptionOrWithColumnSyntax IsActive(this ICreateTableWithColumnOrSchemaSyntax self, bool defaultValue = true) =>
        self.WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(defaultValue).Indexed();

    public static ICreateTableColumnOptionOrWithColumnSyntax IsActive(this ICreateTableColumnOptionOrWithColumnSyntax self, bool defaultValue = true) =>
        self.WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(defaultValue).Indexed();

    public static ICreateTableColumnOptionOrWithColumnSyntax Bool(this ICreateTableColumnOptionOrWithColumnSyntax self,
       string name, bool? defaultValue)
    {
        if (defaultValue.HasValue)
            self.WithColumn(name).AsBoolean().WithDefaultValue(defaultValue).NotNullable();
        else
            self.WithColumn(name).AsBoolean().NotNullable();

        return self;
    }

    public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax Bool(this IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax self,
       string name, bool? defaultValue)
    {
        if (defaultValue.HasValue)
            self.AddColumn(name).AsBoolean().WithDefaultValue(defaultValue).NotNullable();
        else
            self.AddColumn(name).AsBoolean().NotNullable();

        return self;
    }

    public static void CreateInfo(this ICreateTableColumnOptionOrWithColumnSyntax self) =>
        self.WithColumn("DateCreatedUtc").AsDateTime().NotNullable().WithDefaultValue(SqlServerFunction.NowTimestampUTC)
        .WithColumn("CreatedByUserId").AsInt32().Nullable().ForeignKey(Tables.User, "Id").Indexed();

    public static ICreateTableColumnOptionOrWithColumnSyntax ChangeInfo(this ICreateTableColumnOptionOrWithColumnSyntax self)
    {
        self = self.WithColumn("DateCreatedUtc").AsDateTime().NotNullable().WithDefaultValue(SqlServerFunction.NowTimestampUTC)
            .WithColumn("CreatedByUserId").AsInt32().Nullable().ForeignKey(Tables.User, "Id").Indexed()
            .WithColumn("LastModifiedUtc").AsDateTime().Nullable()
            .WithColumn("LastModifiedByUserId").AsInt32().Nullable().ForeignKey(Tables.User, "Id").Indexed();

        return self;
    }

    public static ICreateTableColumnOptionOrWithColumnSyntax ChangeInfo(this ICreateTableWithColumnOrSchemaSyntax self)
    {
        var s = self.WithColumn("DateCreatedUtc").AsDateTime().NotNullable().WithDefaultValue(SqlServerFunction.NowTimestampUTC)
               .WithColumn("CreatedByUserId").AsInt32().Nullable().ForeignKey(Tables.User, "Id").Indexed()
               .WithColumn("LastModifiedUtc").AsDateTime().Nullable()
               .WithColumn("LastModifiedByUserId").AsInt32().Nullable().ForeignKey(Tables.User, "Id").Indexed();

        return s;
    }

    public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax ChangeInfo(this IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax self) =>
        self.AddColumn("DateCreatedUtc").AsDateTime().NotNullable().WithDefaultValue(SqlServerFunction.NowTimestampUTC)
            .AddColumn("CreatedByUserId").AsInt32().Nullable().ForeignKey(Tables.User, "Id").Indexed()
            .AddColumn("LastModifiedUtc").AsDateTime().Nullable()
            .AddColumn("LastModifiedByUserId").AsInt32().Nullable().ForeignKey(Tables.User, "Id").Indexed();
}