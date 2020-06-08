

namespace VoiceToCommand
{
    public interface IVoiceCommand
    {
        void Execute();

        bool CanExecute();
    }
}
