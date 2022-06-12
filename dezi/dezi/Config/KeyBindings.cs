using dezi.Input;

namespace dezi.Config
{
    public class KeyBindings
    {
        // TODO: load settings
        // TODO: create properties to load settings into

        public KeyboardInputs KeyboardInputs { get; }

        public KeyBindings()
        {
            this.KeyboardInputs = new KeyboardInputs(this);
        }

        public void Save()
        {
            // TODO
        }
    }
}
