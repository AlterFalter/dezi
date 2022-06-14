namespace dezi.UiElements.StackPanel
{
    /// <summary>
    /// Can be used to show UiElements next to each other
    /// (start left and filling up to the right)
    /// </summary>
    public class HorizontalStackPanel : StackPanel
    {
        public void AddLeft(UiElement uiElement)
        {
            this.Add(uiElement, 0);
        }

        public void AddRight(UiElement uiElement)
        {
            this.Add(uiElement, this.uiElements.Count);
        }
    }
}
