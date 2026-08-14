using System.Collections.Generic; 
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

        public int bestTime;
        public int lastTime;
        public List<string> ownedItems = new List<string>();

        public int selectedShipIndex = 0;
        public int selectedEffectIndex = 0;

    }
}
