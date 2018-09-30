namespace NoteApp.Services
{
    public interface IFilter
    {
        bool IsHideFinished();

        void SetHideFinished(bool hideFinished);
    }
}
