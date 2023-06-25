namespace AJ.Generic.Tools
{   
    [System.Serializable]
    public class AJMessage
    {
        public int amount = 0;
        public float progress = 0f;
        public string message = "";
        public bool hasMessage = false;
        public object aj;
    }
}
