public interface IButtonInteractable
{
    public bool IsInteractable { get; set; }

    public void Initialize(bool isInteractable);
}
