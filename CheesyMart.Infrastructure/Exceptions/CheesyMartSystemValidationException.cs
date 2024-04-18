namespace CheesyMart.Infrastructure.Exceptions;

[Serializable]
public class CheesyMartSystemValidationException : Exception
{
    public CheesyMartSystemValidationException()
    {
    }
    
    public CheesyMartSystemValidationException(string message) : base(message)
    {
    }
    
    public CheesyMartSystemValidationException(string message, Exception inner) : base(message, inner)
    {
    }
}