namespace House_renting_system_Project.Middlewares
{
    public static class TimeForRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseTimer
            (this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TimeForRequestMiddleware>();
        }
    }
}
