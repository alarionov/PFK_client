namespace PFK
{
    public enum CursorType
    {
        Default,
        Hand,
        Magnifier
    }
    
    interface  IClickable
    {
        public CursorType GetCursor();
    }
}
