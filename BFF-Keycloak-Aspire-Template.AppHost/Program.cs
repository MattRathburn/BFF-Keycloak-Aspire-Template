var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddKeycloak("keycloak", 8080);

var bff = builder.AddProject<Projects.BFF>("bff")
    .WithReference(keycloak)
    .WaitFor(keycloak);

var weatherapi = builder.AddProject<Projects.WeatherAPI>("weatherapi");

builder.AddNpmApp("angular", "../WebApp/Angular/angular-webapp")
    .WithReference(bff)
    .WithReference(weatherapi)
    .WaitFor(bff)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddNpmApp("react", "../WebApp/React/react-webapp")
    .WithReference(bff)
    .WithReference(weatherapi)
    .WaitFor(bff)
    .WithEnvironment("BROWSER", "none")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddNpmApp("vue", "../WebApp/Vue/vue-webapp")
    .WithReference(bff)
    .WithReference(weatherapi)
    .WaitFor(bff)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
