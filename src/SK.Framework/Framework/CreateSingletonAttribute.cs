namespace SK.Framework;

/// <summary>
/// Make a class to be automatically registered as singleton by DI
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CreateSingletonAttribute : Attribute
{
}
