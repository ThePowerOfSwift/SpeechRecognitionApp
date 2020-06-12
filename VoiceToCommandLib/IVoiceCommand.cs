
namespace VoiceToCommandLib
{
    public interface IVoiceCommand
    {
        void Execute();

        bool CanExecute();
    }
}
