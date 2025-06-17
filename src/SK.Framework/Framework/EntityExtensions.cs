using SD.LLBLGen.Pro.ORMSupportClasses;

namespace SK.Framework;

public static class EntityExtensions
{
    /// <summary>
    /// Use this instead of AddNew()
    /// https://www.llblgen.com/tinyforum/Thread/27459/1
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TEntity AddNewEntity<TEntity>(this EntityCollectionBase2<TEntity> collection) where TEntity : EntityBase2, IEntity2
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        var item = collection.EntityFactoryToUse == null ? Activator.CreateInstance<TEntity>() : (TEntity)collection.EntityFactoryToUse.Create();
        collection.Add(item);
        return item;
    }
}
