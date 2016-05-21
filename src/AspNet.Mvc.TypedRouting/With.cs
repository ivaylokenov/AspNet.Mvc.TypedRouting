namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Provides easy replacing of typed route values.
    /// </summary>
    public static class With
    {
        /// <summary>
        /// Indicates that a parameter should not be added to the route values of a generated link.
        /// </summary>
        /// <typeparam name="TParameter">Type of parameter.</typeparam>
        /// <returns>Default value of the parameter.</returns>
        public static TParameter No<TParameter>()
        {
            return default(TParameter);
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
    public static class With
    {
        /// <summary>
        /// Indicates that a parameter can be of any value in a typed route configuration.
        /// </summary>
        /// <typeparam name="TParameter">Type of parameter.</typeparam>
        /// <returns>Default value of the parameter.</returns>
        public static TParameter Any<TParameter>()
        {
            return default(TParameter);
        }
    }
}