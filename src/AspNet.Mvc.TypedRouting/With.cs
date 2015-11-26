namespace Microsoft.AspNet.Mvc
{
    /// <summary>
    /// Provides easy replacing of typed route values.
    /// </summary>
    public static class With
    {
        /// <summary>
        /// Indicates that a parameter should not be added to the route values of a typed route.
        /// </summary>
        /// <typeparam name="TParameter">Type of parameter.</typeparam>
        /// <returns><see cref="TParameter"/></returns>
        public static TParameter No<TParameter>()
        {
            return default(TParameter);
        }
    }
}
