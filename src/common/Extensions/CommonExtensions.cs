using Microsoft.Extensions.Configuration;

namespace Extensions
{
    public static class CommonExtensions
    {
        public static TModel BindSection<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }
    }
}
