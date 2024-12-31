var builder = DistributedApplication.CreateBuilder(args);
// Register the cache
var redis = builder.AddRedis("redis", 6379).WithImage("library/redis", "7.2-alpine");

builder.AddProject<Projects.mvc>("web")
.WithReference(redis);

builder.Build().Run();
