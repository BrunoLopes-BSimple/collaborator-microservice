public class InstanceInfo
{
    public static readonly string InstanceId = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Guid.NewGuid().ToString();
}