namespace WordBearers
{
    public enum CursorType
    {
        Default,
        Hovering,
        Magnifier
    }
    
    interface  IClickable
    {
        public CursorType GetCursor();
    }
}
