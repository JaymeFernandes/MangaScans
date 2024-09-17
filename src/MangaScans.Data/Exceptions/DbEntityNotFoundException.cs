namespace MangaScans.Data.Exceptions;

public class DbEntityException : Exception
{
    public DbEntityException(string message) : base(message) { }
}