namespace Microsoft.AspNet.Mvc
{
    public static class With
    {
        public static TParameter No<TParameter>()
        {
            return default(TParameter);
        }
    }
}
