using ConTech.Migration;

namespace CMIS.Migration.Seed;

[Migration(3000)]
public class _3000_SeedEntityTypeTable : FluentMigrator.Migration
{
    record EntityType(int Id, NameEnglishAndArabic NameEnglishAndArabic);

    public override void Down()
    {
        Execute.Sql($"TRUNCATE TABLE {Tables.EntityType} RESTART IDENTITY CASCADE;");
    }

    public override void Up()
    {
        var types = new EntityType[]
        {
            new EntityType(100, new NameEnglishAndArabic("PolyLine", "PolyLine")),
            new EntityType(200, new NameEnglishAndArabic("LwPolyline","LwPolyline")),
            new EntityType(300, new NameEnglishAndArabic("Point","Point")),
            new EntityType(400, new NameEnglishAndArabic("MText","MText")),
            new EntityType(500, new NameEnglishAndArabic("Circle","Circle")),
            new EntityType(600, new NameEnglishAndArabic("Line","Line")),
        };

        foreach (var type in types)
        {
            Insert.IntoTable(Tables.EntityType).Row(new { type.Id, type.NameEnglishAndArabic.nameArabic, type.NameEnglishAndArabic.nameEnglish });
        }
    }
}