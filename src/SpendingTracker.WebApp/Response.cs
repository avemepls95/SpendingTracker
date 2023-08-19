namespace SpendingTracker.WebApp
{
    public class Response
    {
        public bool Ok => true;
    }

    public sealed class Response<T> : Response
    {
        public T Data { get; }

        public Response(T data)
        {
            Data = data;
        }
    }
}