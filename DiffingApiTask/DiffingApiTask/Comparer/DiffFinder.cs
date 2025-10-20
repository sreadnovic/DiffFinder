public class DiffFinder : IDiffFinder
{
    public object GetDiff(string left, string right)
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

        if (offset != null)
        {
            diffs.Add(new Diff { offset = offset.Value, length = length });
        }

        return DiffResultFactory.Create2(diffs);
    }
}