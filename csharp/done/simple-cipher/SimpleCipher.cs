public class SimpleCipher
{
    // No magical numbers
    private const int AlphabetSize = 26;
    private const char LowerA = 'a';

    public SimpleCipher()
    {
        // Generate a random key of length 100
        Random rand = new Random();
        var result = new char[100];
        int randvalue;

        for (int i = 0; i < 100; i++)
        {
            randvalue = rand.Next(0, AlphabetSize);
            result[i] = (char)(randvalue + LowerA);
        }

        this.Key = new string(result);
    }

    public SimpleCipher(string key) => Key = key;

    public string Key { get; }


    public string Encode(string plaintext)
    {
        // Allocate buffer, else a new string will be allocated for each str += i in the for loop
        var result = new char[plaintext.Length];

        for (int i = 0; i < plaintext.Length; i++)
        {
            // Find correct number of steps to shift with
            int shiftSteps = Key[i % Key.Length] - LowerA;

            // Shift letter and add to encoded
            result[i] = shiftChar(plaintext[i], shiftSteps);
        }

        return new string(result);
    }

    public string Decode(string ciphertext)
    {
        var result = new char[ciphertext.Length];

        for (int i = 0; i < ciphertext.Length; i++)
        {
            // Find correct number of steps to shift with
            int shiftSteps = Key[i % Key.Length] - LowerA;

            // Shift letter and add to encoded
            result[i] = shiftChar(ciphertext[i], -shiftSteps);
        }

        return new string(result);
    }

    // Helper function
    private char shiftChar(char c, int shift)
    {
        int pos = c - LowerA;
        int newPos = (pos + shift + AlphabetSize) % AlphabetSize;

        return (char)('a' + newPos);

    }
}
