namespace SK.Framework;

/// <summary>
/// Make a class to be automatically registered as in scope by DI
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CreateInScopeAttribute : Attribute
{
}
