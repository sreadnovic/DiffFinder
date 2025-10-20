using System.Text.Json;

public static class DiffResultFactory
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

    public static string Create(List<Diff> diffs)
    {
        if (diffs.Count == 0)
            return JsonSerializer.Serialize(new EqualsResult(), options);
        else
            return JsonSerializer.Serialize(new ContentDoNotMatchResult(diffs), options);
    }
    
    public static object Create2(List<Diff> diffs)
    {
        if (diffs.Count == 0)
            return new EqualsResult();
        else
            return new ContentDoNotMatchResult(diffs); 
    }
}