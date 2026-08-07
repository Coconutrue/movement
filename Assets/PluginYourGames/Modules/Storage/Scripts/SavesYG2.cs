
namespace YG
{
    [System.Serializable]
    public partial class SavesYG
    {
        public int idSave;
        public bool isFirstGame = true;
        public string language = "ru";

        public int money; 
        public float lastX;
        public float lastY;
        public float lastZ;
    }
}
