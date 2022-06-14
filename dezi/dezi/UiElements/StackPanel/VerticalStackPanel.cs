namespace dezi.UiElements.StackPanel
{
    /// <summary>
    /// Can be used to display UiElements above each other
    /// Start is on top and fills up on the bottom
    /// </summary>
    public class VerticalStackPanel : StackPanel
    {
        public void AddTop(UiElement uiElement)
        {
            this.Add(uiElement, 0);
        }

        public void AddBottom(UiElement uiElement)
        {
            this.Add(uiElement, this.uiElements.Count);
        }
    }
}
