namespace AspNet.Mvc.TypedRouting.Routing
{
    using System;

    public interface ITypedRouteBuilder
    {
        TypedRoute Get(string template, Action<TypedRoute> configuration);

        TypedRoute Post(string template, Action<TypedRoute> configuration);

        TypedRoute Put(string template, Action<TypedRoute> configuration);

        TypedRoute Delete(string template, Action<TypedRoute> configuration);

        TypedRoute Add(string template, Action<TypedRoute> configuration);
    }
}
