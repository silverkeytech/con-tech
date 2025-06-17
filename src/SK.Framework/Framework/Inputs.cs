using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Collections;

namespace SK.Framework;

public interface IToEntityCollection<T> where T : IEntityCollection2
{
	T ToEntityCollection();
}

public interface IToEntityCollectionWithParam<out T, D> where T : IEntityCollection2
{
	T ToEntityCollection(D p);
}
public interface IToEntity<T> where T : IEntity2
{
    T ToEntity();
}

public interface IToEntity2<T, D> where T : IEntity2
                                  where D : IEntity2
{
    (T, D) ToEntity();
}

public interface IToEntity3<T, D, X> where T : IEntity2
                                  where D : IEntity2
                                  where X : IEntity2
{
    (T, D, X) ToEntity();
}

public interface IToEntity4<T, D, X, Y> where T : IEntity2
                                  where D : IEntity2
                                  where X : IEntity2
                                  where Y : IEntity2
{
    (T, D, X, Y) ToEntity();
}

public interface IToEntityWithParam<out T, D> where T : IEntity2
{
    T ToEntity(D p);
}


public interface IToEntityWithParam2<out T, D, X> where T : IEntity2
{
    T ToEntity(D d, X x);
}

public interface IToEntityWithParam3<out T, D, X, Y> where T : IEntity2
{
    T ToEntity(D d, X x, Y y);
}

public interface IToEntityWithParam4<out T, D, X, Y, Z> where T : IEntity2
{
    T ToEntity(D d, X x, Y y, Z z);
}

public interface IToEntity2WithParam<T, D, E> where T : IEntity2
                                        where D : IEntity2
{
    (T, D) ToEntity(E e);
}