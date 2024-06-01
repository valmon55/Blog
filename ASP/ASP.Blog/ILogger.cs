namespace ASP.Blog
{
    public interface ILogger
    {
        void WriteEvent(string eventMessage);
        void WriteError(string errorMessage);
    }
}
