namespace SK.Framework;

public static class Tasks
{
    public async static Task<(T1, T2)> WhenAll<T1, T2>(Task<T1> t1, Task<T2> t2)
    {
        await Task.WhenAll(t1, t2);
        return (t1.Result, t2.Result);
    }

    public async static Task<(T1, T2, T3)> WhenAll<T1, T2, T3>(Task<T1> t1, Task<T2> t2, Task<T3> t3)
    {
        await Task.WhenAll(t1, t2, t3);
        return (t1.Result, t2.Result, t3.Result);
    }

    public async static Task<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4)
    {
        await Task.WhenAll(t1, t2, t3, t4);
        return (t1.Result, t2.Result, t3.Result, t4.Result);
    }
}
