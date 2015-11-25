namespace AspNet.Mvc.TypedRouting.Test
{
    using Microsoft.Extensions.OptionsModel;
    using System.Linq;

    public class TestOptionsManager<T> : OptionsManager<T>
        where T : class, new()
    {
        public TestOptionsManager()
            : base(Enumerable.Empty<IConfigureOptions<T>>())
        {
        }
    }
}
