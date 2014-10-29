namespace SilverlightMuc
{
    /// <summary>
    /// enum for more user friendly status states
    /// </summary>
    public enum StatusImage
    {        
        /// <summary>
        /// user is online
        /// </summary>
        Online,
        
        /// <summary>
        /// user is online and free for chat
        /// </summary>
        Chat,
        
        /// <summary>
        /// user is online, but away
        /// </summary>
        Away,
        
        /// <summary>
        /// user is online, but extended away
        /// </summary>
        Xa,
        
        /// <summary>
        /// user is online in do not disturb mode
        /// </summary>
        Dnd,
    }
}