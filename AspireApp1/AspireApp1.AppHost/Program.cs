using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");
var rabbit = builder.AddRabbitMQ("rabbit");


builder.AddProject<DiagnosticsWebApi>("api")
    .WithReference(redis)
    .WithReference(rabbit)
    .WithReplicas(2);




builder.Build().Run();
