namespace iSukces.Parsers
{
    internal class StringTextSpanSource : ITextSpanSource
    {
        public StringTextSpanSource(string text)
        {
            Text = text;
        }

        public static TextSpan Make(string text)
        {
            if (text is null)
                text = string.Empty;
            var src = new StringTextSpanSource(text);
            return new TextSpan(src, 0, text.Length);
        }

        public string Text { get; }
    }
}