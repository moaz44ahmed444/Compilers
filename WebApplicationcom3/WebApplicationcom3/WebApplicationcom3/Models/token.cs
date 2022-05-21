namespace WebApplicationcom3.Models
{
    public class token
    {
        public token(tokenType type, int position, string text, object value)
        {
            Type = type;
            Position = position;
            Text = text;
            Value = value;
        }

        public tokenType Type { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
    }
}
