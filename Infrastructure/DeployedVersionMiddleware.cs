using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AnimeciBackend
{
    public class DeployedVersionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _dv;
        public DeployedVersionMiddleware(RequestDelegate next, string dv)
        {
            _next = next;
            _dv = dv;
        }
        
        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add("X-DeployedVersion", new[] { _dv });
                return Task.CompletedTask;
            }, context);

            await _next(context);
        }
    }
}