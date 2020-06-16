
namespace VoiceToCommand.Core
{
    public interface IVoiceCommand
    {
        void Execute();

        bool CanExecute();
    }
}

