using System.Text.Json.Serialization;

public abstract class DiffResultBase
{
    [JsonPropertyOrder(0)]
    public string diffResultType { get; set; }
}