using System.Text.Json.Serialization;

public class ContentDoNotMatchResult : DiffResultBase
{
    [JsonPropertyOrder(1)]
    public List<Diff> diffs { get; set; }
    public ContentDoNotMatchResult()
    {
        diffResultType = "ContentDoNotMatch";
    }
    public ContentDoNotMatchResult(List<Diff> diffs)
    {
        diffResultType = "ContentDoNotMatch";
        this.diffs = diffs;
    }
}