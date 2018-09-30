namespace NoteApp.Services
{
    public class Style : IStyle
    {
        private static string BRIGHT = "bright";
        private static string DARK = "dark";

        private string style = BRIGHT;

        public void change()
        {
            style = getOpposite();
            
        }

        public string getCurrent()
        {
            return style;
        }

        public string getNext()
        {
            return getOpposite();
        }

        private string getOpposite()
        {
            if (style.Equals(BRIGHT))
            {
                return DARK;
            }
            else
            {
                return BRIGHT;
            }
        }
    }
}
