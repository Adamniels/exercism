public static class RotationalCipher
{
    public static string Rotate(string text, int shiftKey)
    {
        string rotatedWord = "";
        foreach (char c in text)
        {
            if (c >= 'a' && c <= 'z')
            {
                rotatedWord += (char)('a' + (c - 'a' + shiftKey) % 26);
            }
            else if (c >= 'A' && c <= 'Z')
            {
                rotatedWord += (char)('A' + (c - 'A' + shiftKey) % 26);
            }
            else
            {
                rotatedWord += c;
            }
        }
        return rotatedWord;
    }
}
