namespace dezi.UiElements
{
    public abstract class UiElement
    {
        private int width;
        private int height;

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                int widthDelta = value - this.width;
                this.width = value;
                UpdateSubmoduleSizes(widthDelta, 0);
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                int heightDelta = value - this.height;
                this.height = value;
                UpdateSubmoduleSizes(0, heightDelta);
            }
        }

        public int CoordinateX { get; set; }

        public int CoordinateY { get; set; }

        public bool IsInFocus { get; set; }

        public abstract void UpdateSubmoduleSizes(int widthDelta, int heightDelta);
    }
}
