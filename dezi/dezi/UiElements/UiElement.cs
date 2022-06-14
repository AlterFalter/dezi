namespace dezi.UiElements
{
    public abstract class UiElement
    {
        private static int idCounter = 0;

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

        public int ID { get; private set; }

        public int CoordinateX { get; set; }

        public int CoordinateY { get; set; }

        public bool ShowElement { get; set; }

        protected UiElement()
        {
            this.ID = idCounter++;
            this.ShowElement = true;
        }

        /// <summary>
        /// Informs UiElement that size has changed and therefore the sizes of the submodules change too.
        /// When there are no submodules, let it empty.
        /// </summary>
        /// <param name="widthDelta"></param>
        /// <param name="heightDelta"></param>
        protected abstract void UpdateSubmoduleSizes(int widthDelta, int heightDelta);
    }
}
