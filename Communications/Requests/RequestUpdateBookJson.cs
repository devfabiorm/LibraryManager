namespace LibraryManager.Communications.Requests;

public class RequestUpdateBookJson
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int Amount { get; set; }
}
