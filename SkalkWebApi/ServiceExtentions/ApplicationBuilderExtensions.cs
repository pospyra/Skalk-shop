using Microsoft.EntityFrameworkCore;

namespace SkalkWebApi.ServiceExtentions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSkalkCoreontext(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            using var context = scope?.ServiceProvider.GetRequiredService<SkalkContext>();
            context?.Database.Migrate();
        }
    }
}
