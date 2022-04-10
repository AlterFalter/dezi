namespace dezi.Helper
{
    public static class StringHelper
    {
        public static string PutInNewStringAtIndex(string mainString, int startIndex, string newSubstring)
        {
            return mainString.Substring(0, startIndex)
                + newSubstring
                + mainString.Substring(startIndex + newSubstring.Length);
        }
    }
}
