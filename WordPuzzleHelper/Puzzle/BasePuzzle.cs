namespace WordPuzzleHelper.Puzzle
{
    public abstract class BasePuzzle
    {
        protected readonly KnownWords KnownWords;

        protected BasePuzzle(KnownWords knownWords)
        {
            KnownWords = knownWords;
        }
    }
}
