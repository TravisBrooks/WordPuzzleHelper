namespace WordPuzzleHelper.Puzzle
{
    public abstract class BasePuzzle
    {
        protected readonly KnownWords _knownWords;

        protected BasePuzzle(KnownWords knownWords)
        {
            _knownWords = knownWords;
        }
    }
}
