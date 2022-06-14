namespace dezi.UiElements
{
    public abstract class InteractiveUiElement : UiElement
    {
        public bool IsInFocus { get; set; }

        public InteractiveUiElement()
        {
            this.IsInFocus = false;
        }
    }
}
