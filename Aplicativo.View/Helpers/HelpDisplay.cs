namespace Aplicativo.View.Helpers
{
    public static class HelpDisplay
    {

        public static Display Display { get; set; } = new Display(0, 0);

    }

    public enum LayoutSize
    {
        ExtraSmall = 0,
        Small = 576,
        Medium = 768,
        Large = 992,
        ExtraLarge = 1200,
    }

    public class Display
    {

        public Display(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public int Width { get; set; }

        public int Height { get; set; }

    }
}