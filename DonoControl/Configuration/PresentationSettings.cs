namespace DonoControl.Configuration
{
    public class PresentationSettings
    {
        public const string SectionName = "Presentation";
        public string FilePath { get; set; } = string.Empty;
        public int SlideIntervalMs { get; set; } = 5000;
    }
}
