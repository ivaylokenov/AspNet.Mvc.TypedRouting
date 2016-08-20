namespace AspNet.Mvc.TypedRouting.LinkGeneration
{
    using System.Collections.Generic;

    public class UniqueRouteKeysProvider : IUniqueRouteKeysProvider
    {
        private readonly ISet<string> uniqueRouteKeys;

        public UniqueRouteKeysProvider()
        {
            uniqueRouteKeys = new HashSet<string>();
        }

        internal void AddKey(string key)
        {
            uniqueRouteKeys.Add(key);
        }

        public ISet<string> GetUniqueKeys()
        {
            return uniqueRouteKeys;
        }
    }
}
