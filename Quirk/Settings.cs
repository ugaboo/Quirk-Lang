namespace Quirk {
    public static class Settings {
        public static string Tab = "    ";

        public static int TabSize {
            get => Tab.Length;
            set => Tab = new string(' ', value);
        }
    }
}