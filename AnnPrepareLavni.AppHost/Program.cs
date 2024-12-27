var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AnnPrepareLavni_ApiService>("annpreparelavni-apiservice");

builder.AddProject<Projects.AnnPrepareLavni_FrontEnd>("annpreparelavni-frontend");

builder.Build().Run();
