namespace PFK.Acts.SceneRegistry
{
    [System.Serializable]
    public class Entry
    {
        public string Title;
        public string ContractAddress;
        public string DirectoryPrefix;
        public string[] Scenes;

        public string GetScene(int index)
        {
            return 
                string.IsNullOrEmpty(DirectoryPrefix) ? 
                    Scenes[index] : 
                    $"{DirectoryPrefix}/{Scenes[index]}";
        }
    }
}