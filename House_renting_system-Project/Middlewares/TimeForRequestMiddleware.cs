using House_renting_system_Project.Data.Data;
using System.Diagnostics;

namespace House_renting_system_Project.Middlewares
{
    public class TimeForRequestMiddleware
    {
        private RequestDelegate next;
        public TimeForRequestMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, HouseRentingDbContext ctx)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Start");
            await this.next(httpContext);
            stopwatch.Stop();
            Console.WriteLine($"Milliseconds finale time: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
