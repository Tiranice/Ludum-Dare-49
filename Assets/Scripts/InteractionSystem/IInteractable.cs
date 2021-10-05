namespace LudumDare49.Interaction
{
    ///<!--
    /// IInteractable.cs
    /// 
    /// Project:  Ludum Dare 49
    ///        
    /// Author :  Devon Wilson
    /// Company:  Black Pheonix Software
    /// Created:  Oct 01, 2021
    /// Updated:  Oct 01, 2021
    /// -->
    /// <summary>
    ///
    /// </summary>
    public interface IInteractable
    {
        event System.Action<IInteractable> OnInteract;

        void Interact();

        string InteractionType { get; }
        bool CanInteract { get; }
    }
}