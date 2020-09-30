namespace Assets.Scripts.Core
{
    public class SaveLoader
    {
        public bool IsNeedToLoad { get; set; }
        private static SaveLoader _instance;

        public static SaveLoader Instance()
        {
            if (_instance == null)
                _instance = new SaveLoader();

            return _instance;
        }
    }
}