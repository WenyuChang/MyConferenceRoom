using Matrix.Xml;

namespace SilverlightMuc
{
    /// <summary>
    /// Settings file
    /// </summary>
    public class Settings : XmppXElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
            : base("ag-software", "Settings")
        {
        }

        /// <summary>
        /// Gets or sets the nickname.
        /// </summary>
        /// <value>The nickname.</value>
        public string Nickname
        {
            get { return GetTag("Nickname"); }
            set { SetTag("Nickname", value); }
        }
    }
}