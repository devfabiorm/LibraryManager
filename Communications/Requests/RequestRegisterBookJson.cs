namespace LibraryManager.Communications.Requests;

public class RequestRegisterBookJson
{
    public string Author { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Amount { get; set; }
}
