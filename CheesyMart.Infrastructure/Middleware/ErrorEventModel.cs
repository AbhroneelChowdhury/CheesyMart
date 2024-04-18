namespace CheesyMart.Infrastructure.Middleware;

public class ErrorEventModel
{
    public string EventId { get; set; }
    
    public string Message { get; set; }
    
    public string StatusCode { get; set; }
}