using Umbraco.Web;

namespace Our.Umbraco.UaaS.Toolkit
{
    public static class UmbracoHelperExtensions
    {
        public static bool IsDevelopment(this UmbracoHelper helper)
        {
            return IsEnvironment(UmbracoEnvironmentType.Development);
        }

        public static bool IsStaging(this UmbracoHelper helper)
        {
            return IsEnvironment(UmbracoEnvironmentType.Staging);
        }

        public static bool IsLive(this UmbracoHelper helper)
        {
            return IsEnvironment(UmbracoEnvironmentType.Live);
        }

        private static bool IsEnvironment(UmbracoEnvironmentType environmentType)
        {
            var environment = EnvironmentHelper.GetUmbracoEnvironmentType();
            return environment.HasValue && environment == environmentType;
        }
    }
}