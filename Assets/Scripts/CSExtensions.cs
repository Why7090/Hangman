namespace ExtensionMethods
{
    public static class CSExtensions
    {
        public static int[] AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                return new int[1] { -1 };
            int[] indexes = new int[0];
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                int[] index_ = new int[1] { index };
                int[] combined = new int[indexes.Length + 1];
                indexes.CopyTo(combined, 0);
                index_.CopyTo(combined, indexes.Length);
                indexes = combined;
            }
        }
    }
}