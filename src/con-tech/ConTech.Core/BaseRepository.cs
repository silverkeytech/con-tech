namespace ConTech.Core;

public abstract class BaseRepository
{
    protected readonly DataAccessAdapter _adapter;
    protected readonly LinqMetaData _meta;
    protected IStringLocalizer<Global> _local;

    public BaseRepository(DataAccessAdapter adapter, IStringLocalizer<Global> local)
    {
        _adapter = adapter;
        _meta = new LinqMetaData(_adapter);
        _local = local;
    }
}