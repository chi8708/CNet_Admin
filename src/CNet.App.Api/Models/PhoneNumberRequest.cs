namespace CNet.App.Api.Models;

public class PhoneNumberRequest
{
    public string Code { get; set; } = string.Empty;
    public string EncryptedData { get; set; } = string.Empty;
    public string Iv { get; set; } = string.Empty;
} 