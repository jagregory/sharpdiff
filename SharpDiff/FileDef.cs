namespace SharpDiff
{
    public class FileDef
    {
        public FileDef(char letter, string filename)
        {
            Letter = letter;
            FileName = filename;
        }

        public char Letter { get; private set; }
        public string FileName { get; private set; }
    }
}