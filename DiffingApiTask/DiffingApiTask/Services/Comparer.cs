using System.Text.Json;

public static class Comparer
{
    public static string Compare(string left, string right)
    {
        byte[] leftBytes = Convert.FromBase64String(left);
        byte[] rightBytes = Convert.FromBase64String(right);

        var diffs = new List<Diff>();
        int? offset = null;
        int length = 0;

        for (int i = 0; i < Math.Min(leftBytes.Length, rightBytes.Length); i++)
        {
            if (leftBytes[i] != rightBytes[i])
            {
                if (offset == null)
                {
                    offset = i;
                    length = 1;
                }
                else
                {
                    length++;
                }
            }
            else if (offset != null)
            {
                diffs.Add(new Diff { offset = offset.Value, length = length });
                offset = null;
                length = 0;
            }
        }

        // Handle difference at the end
        if (offset != null)
        {
            diffs.Add(new Diff { offset = offset.Value, length = length });
        }

        if (diffs.Count == 0)
        {
            var res = new
            {
                diffResultType = "Equals"
            };

            return JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true });
        }
        else
        {
            var res = new
            {
                diffResultType = "ContentDoNotMatch",
                diffs = diffs
            };

            return JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    private class Diff
    {
        public int offset { get; set; }
        public int length { get; set; }
    }
}
