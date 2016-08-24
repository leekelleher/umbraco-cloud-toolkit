using System;
using System.IO;
using Umbraco.Web;

namespace Our.Umbraco.UaaS.Toolkit
{
    public static class EnvironmentHelper
    {
        public static string GetUmbracoEnvironment()
        {
            var environmentType = GetUmbracoEnvironmentType();

            if (environmentType.HasValue)
            {
                return environmentType.Value.ToString();
            }

            // TODO: [MB] Could do with a way to figure out local without a dependency on System.Web?
            // http://stackoverflow.com/a/19010304/12787
            if (UmbracoContext.Current != null && UmbracoContext.Current.HttpContext.Request.IsLocal)
            {
                return "Local";
            }

            return "Elsewhere";
        }

        /// <remarks>
        /// Code supplied by @sitereactor. As Courier's `FileSystem` class is marked as `internal`.
        /// </remarks>
        internal static UmbracoEnvironmentType? GetUmbracoEnvironmentType()
        {
            var absolutePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\environment"));
            var enumType = typeof(UmbracoEnvironmentType);

            foreach (var type in Enum.GetNames(enumType))
            {
                var path = Path.Combine(absolutePath, type);

                if (File.Exists(path))
                {
                    return (UmbracoEnvironmentType)Enum.Parse(enumType, type);
                }
            }

            return null;
        }
    }
}