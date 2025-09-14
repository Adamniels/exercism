//
// using System.Collections.Immutable;
// using System.Reactive.Subjects;
//
// public class HangmanState
// {
//     public string MaskedWord { get; }
//     public ImmutableHashSet<char> GuessedChars { get; }
//     public int RemainingGuesses { get; }
//
//     public HangmanState(string maskedWord, ImmutableHashSet<char> guessedChars, int remainingGuesses)
//     {
//         MaskedWord = maskedWord;
//         GuessedChars = guessedChars;
//         RemainingGuesses = remainingGuesses;
//     }
// }
//
// public class TooManyGuessesException : Exception
// {
// }
//
// public class Hangman
// {
//     private readonly string _secretWord;
//     private readonly BehaviorSubject<HangmanState> _state;
//     private readonly Subject<char> _guesses;
//
//     private const int DefaultMaxGuesses = 9;
//
//     public IObservable<HangmanState> StateObservable { get => _state; }
//     public IObserver<char> GuessObserver { get => _guesses; }
//
//     public Hangman(string word)
//     {
//         if (string.IsNullOrWhiteSpace(word))
//             throw new ArgumentException("Secret word must be non-empty.", nameof(word));
//
//         _secretWord = word;
//         _guesses = new Subject<char>();
//
//         // Initialt state: maskerat ord, tomma gissningar, fulla försök
//         var initial = new HangmanState(
//             maskedWord: Mask(_secretWord, ImmutableHashSet<char>.Empty),
//             guessedChars: ImmutableHashSet<char>.Empty,
//             remainingGuesses: DefaultMaxGuesses
//         );
//
//         // BehaviorSubject gör att en subscriber får initialt state omedelbart
//         _state = new BehaviorSubject<HangmanState>(initial);
//
//         _guesses.Subscribe(
//              onNext: OnGuess,
//              onError: ex => _state.OnError(ex),
//              onCompleted: () => _state.OnCompleted()
//          );
//     }
//
//     private void OnGuess(char ch)
//     {
//         // 1) Normalisera och validera
//         if (!char.IsLetter(ch)) return; // ignorera icke-bokstäver
//         var guess = char.ToLowerInvariant(ch);
//
//         var prev = _state.Value;
//
//         // // 2) Dubbelgissning? Gör inget fel enligt våra regler
//         // if (prev.GuessedChars.Contains(guess))
//         //     return;
//
//         // 3) Ny uppsättning gissade tecken
//         var newGuessed = prev.GuessedChars.Add(guess);
//
//         // 4) Maskera om ordet baserat på nya gissningar
//         var newMasked = Mask(_secretWord, newGuessed);
//
//         // 5) Var gissningen korrekt?
//         var wasCorrect = IndexOfIgnoreCase(_secretWord, guess) >= 0;
//
//         // 6) Räkna ner försök endast vid fel
//         // var newRemaining = prev.RemainingGuesses - (wasCorrect ? 0 : 1);
//         var newRemaining = prev.RemainingGuesses - 1;
//         if (newRemaining < 0) newRemaining = 0;
//
//         // 7) Skicka ut nytt state
//         var next = new HangmanState(newMasked, newGuessed, newRemaining);
//         _state.OnNext(next);
//
//         // 8) Terminala fall
//         if (!newMasked.Contains('_'))
//         {
//             _state.OnCompleted(); // vinst
//         }
//         else if (newRemaining == 0)
//         {
//             _state.OnError(new TooManyGuessesException()); // förlust
//         }
//     }
//
//     // Helper function
//     private static string Mask(string secret, ImmutableHashSet<char> guessed)
//     {
//         var buf = new char[secret.Length];
//         for (int i = 0; i < secret.Length; i++)
//         {
//             var ch = secret[i];
//             if (char.IsLetter(ch))
//             {
//                 var lower = char.ToLowerInvariant(ch);
//                 buf[i] = guessed.Contains(lower) ? ch : '_';
//             }
//             else
//             {
//                 buf[i] = ch;
//             }
//         }
//         return new string(buf);
//     }
//     private static int IndexOfIgnoreCase(string s, char lowerGuess)
//     {
//         // lowerGuess är redan lowercase
//         for (int i = 0; i < s.Length; i++)
//         {
//             var c = s[i];
//             if (char.IsLetter(c) && char.ToLowerInvariant(c) == lowerGuess)
//                 return i;
//         }
//         return -1;
//     }
// }
