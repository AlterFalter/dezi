using System;
using System.Collections.Generic;

namespace dezi.UiElements.StackPanel
{
    public class StackPanel : UiElement
    {
        protected IList<StackPanelElement> uiElements;

        public StackPanel()
        {
            this.uiElements = new List<StackPanelElement>();
        }

        public void Remove(UiElement uiElement)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void Remove(int index)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void Add(UiElement uiElement, int index)
        {
            this.uiElements.Add(new StackPanelElement(uiElement, index));
        }

        protected override void UpdateSubmoduleSizes(int widthDelta, int heightDelta)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
