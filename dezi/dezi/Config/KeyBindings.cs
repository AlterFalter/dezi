using dezi.Input;

namespace dezi.Config
{
    public class KeyBindings
    {
        // TODO: load settings
        // TODO: create properties

        public KeyboardInputs KeyboardInputs { get; }

        public KeyBindings()
        {
            this.KeyboardInputs = new KeyboardInputs(this);
        }
    }
}
