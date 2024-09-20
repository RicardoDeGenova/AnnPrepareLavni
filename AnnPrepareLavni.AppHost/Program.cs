var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AnnPrepareLavni_ApiService>("annpreparelavni-apiservice");

builder.Build().Run();
