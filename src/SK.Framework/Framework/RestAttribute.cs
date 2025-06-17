namespace SK.Framework;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
/// <summary>
/// A marker for classes related for REST payload. We will generate automatic typescript definition for all types with these markers
/// </summary>
public class RestAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RequestPayloadAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ReplyPayloadAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OptionalAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OrUndefined : Attribute
{
}
