namespace AspNet.Mvc.TypedRouting.Test.Setups
{
    using System.Linq;
    using Microsoft.Extensions.OptionsModel;

    public class TestOptionsManager<T> : OptionsManager<T>
        where T : class, new()
    {
        public TestOptionsManager()
            : base(Enumerable.Empty<IConfigureOptions<T>>())
        {
        }
    }
}
