using Wolverine;
using Wolverine.Http;
using WolverineMiddlewareApply;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(options =>
{
    // Setting up the outbox on all locally handled
    // background tasks
    options.Policies.AutoApplyTransactions();
    options.Policies.UseDurableLocalQueues();
    options.Policies.UseDurableOutboxOnAllSendingEndpoints();
    
    // options.UseFluentValidation();
});

var app = builder.Build();

app.MapWolverineEndpoints(options =>
{
    options.AddMiddleware(typeof(UserIdMiddleWare));
    options.ConfigureEndpoints(e =>
    {
        // e.RequireAuthorization(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    });
    
    // options.UseFluentValidationProblemDetailMiddleware();
});

app.Run();