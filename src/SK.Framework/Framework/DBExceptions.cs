using SD.LLBLGen.Pro.ORMSupportClasses;

namespace SK.Framework;

public class DBException : Exception
{
    public DBException(string message) : base(message)
    {
    }
}

/// <summary>
///  All DB query exception related
/// </summary>
public class QueryException : DBException
{
    public QueryException(string message) : base(message)
    {
    }
}

/// <summary>
///  When an item that is supposed to exist doesn't exist.
/// </summary>
public class ExpectedItemNotFoundException : QueryException
{
    public ExpectedItemNotFoundException(string message) : base(message)
    {
    }
}

public class DeleteOperationException : DBException
{
    public DeleteOperationException(string message) : base(message)
    {
    }
}

public class SoftDeleteOperationException : DeleteOperationException
{
    public SoftDeleteOperationException(string message) : base(message)
    {
    }
}

public enum DbOperation
{
    Create,

    Update,

    Delete
}

public class SaveOperationException : DBException
{
    public DbOperation Operation { get; set; }

    public SaveOperationException(string message, DbOperation operation) : base(message)
    {
        operation = Operation;
    }
}

public static class IEntityExtensions
{
    public static DbOperation SaveOperationType(this IEntity2 self) => (self.IsNew) ? DbOperation.Create : DbOperation.Update;

    public static DbOperation DeleteOperationType(this IEntity2 self) => DbOperation.Delete;
}