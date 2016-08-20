namespace AspNet.Mvc.TypedRouting.LinkGeneration
{
    using System.Collections.Generic;

    public interface IUniqueRouteKeysProvider
    {
        ISet<string> GetUniqueKeys();
    }
}
