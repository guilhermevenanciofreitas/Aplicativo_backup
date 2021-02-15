using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skclusive.Core.Component;
using Skclusive.Markdown.Component;
using Skclusive.Material.Alert;
using Skclusive.Material.Component;
using Skclusive.Material.Layout;
using Skclusive.Material.Theme;
using System.Collections.Generic;

namespace Aplicativo.View.Helpers
{
    public interface IHelpViewConfig : ILayoutConfig
    {
    }

    public class HelpViewConfig : LayoutConfigBuilder<HelpViewConfig, IHelpViewConfig>
    {
        protected class DocsViewConfig : LayoutConfig, IHelpViewConfig
        {
        }

        public HelpViewConfig() : base(new DocsViewConfig())
        {
        }

        protected override IHelpViewConfig Config()
        {
            return (IHelpViewConfig)_config;
        }

        protected override HelpViewConfig Builder()
        {
            return this;
        }
    }

    public static class HelpViewViewExtension
    {
        public static void TryAddViewServices(this IServiceCollection services, IHelpViewConfig config)
        {
            services.TryAddMarkdownServices(config);

            services.TryAddLayoutServices(config);

            services.TryAddAlertServices(config);

            services.TryAddMaterialServices(config);

            services.TryAddScoped(sp => config);

            services.TryAddStyleTypeProvider<HelpViewStyleProvider>();

            services.TryAddStyleProducer<HelpViewStyleProducer>();
        }
    }

    public class HelpViewStyleProvider : StyleTypeProvider
    {
        public HelpViewStyleProvider() : base(priority: default, typeof(AppStyle))
        {
        }
    }

    public class HelpViewStyleProducer : IStyleProducer
    {
        public IDictionary<string, string> Variables(ThemeValue theme)
        {
            var isDark = theme.IsDark();

            return new Dictionary<string, string>
            {
                ["--theme-docs-palette-border-color"] = (isDark ? "rgba(255, 255, 255, 0.12)" : "rgba(0, 0, 0, 0.12)"),
            };
        }
    }

}