using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Subjects;

public class HangmanState
{
    public string MaskedWord { get; }
    public ImmutableHashSet<char> GuessedChars { get; }
    public int RemainingGuesses { get; }

    public HangmanState(string maskedWord, ImmutableHashSet<char> guessedChars, int remainingGuesses)
    {
        MaskedWord = maskedWord;
        GuessedChars = guessedChars;
        RemainingGuesses = remainingGuesses;
    }
}

public class TooManyGuessesException : Exception
{
}

public class Hangman
{
    private readonly string _secretWord;
    private readonly BehaviorSubject<HangmanState> _state;
    private readonly HashSet<char> _guessedChars = new HashSet<char>();
    private int _remainingGuesses = 9;

    public IObservable<HangmanState> StateObservable => _state;
    public IObserver<char> GuessObserver { get; }

    public Hangman(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            throw new ArgumentException("Secret word must be non-empty.", nameof(word));

        _secretWord = word;

        // Initiera state
        _state = new BehaviorSubject<HangmanState>(
                new HangmanState(
                    Mask(_secretWord, _guessedChars),
                    _guessedChars.ToImmutableHashSet(),
                    _remainingGuesses));


        // Skapa en observer som hanterar gissningar
        GuessObserver = Observer.Create<char>(OnGuess);
    }

    private void OnGuess(char ch)
    {
        if (_remainingGuesses <= 0)
        {
            _state.OnError(new TooManyGuessesException());
            return;
        }

        if (!char.IsLetter(ch))
            return;

        var guess = char.ToLowerInvariant(ch);

        bool added = _guessedChars.Add(guess);
        bool contained = WordContains(_secretWord, guess);

        if (!added || !contained)
        {
            _remainingGuesses--;
        }

        // Har vi vunnit?
        if (AllLettersGuessed())
        {
            _state.OnCompleted();
            return;
        }

        if (_remainingGuesses < 0)
        {
            _state.OnError(new TooManyGuessesException());
            return;
        }

        _state.OnNext(new HangmanState(
                    Mask(_secretWord, _guessedChars),
                    _guessedChars.ToImmutableHashSet(),
                    _remainingGuesses));
    }


    // Helper functions
    private static bool WordContains(string word, char guess)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (char.ToLowerInvariant(word[i]) == guess)
                return true;
        }
        return false;
    }

    private bool AllLettersGuessed()
    {
        for (int i = 0; i < _secretWord.Length; i++)
        {
            char c = _secretWord[i];
            if (char.IsLetter(c))
            {
                if (!_guessedChars.Contains(char.ToLowerInvariant(c)))
                    return false;
            }
        }
        return true;
    }

    private static string Mask(string secret, HashSet<char> guessed)
    {
        var buf = new char[secret.Length];
        for (int i = 0; i < secret.Length; i++)
        {
            var ch = secret[i];
            if (char.IsLetter(ch))
            {
                var lower = char.ToLowerInvariant(ch);
                buf[i] = guessed.Contains(lower) ? ch : '_';
            }
            else
            {
                buf[i] = ch;
            }
        }
        return new string(buf);
    }
}
