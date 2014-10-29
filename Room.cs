using System.Xml.Linq;

using Matrix;
using Matrix.Xml;

namespace SilverlightMuc
{
    /// <summary>
    /// Settings file
    /// </summary>
    public class Room : XmppXElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        public Room()
            : base(XNamespace.None, "room")
        {
        }

        /// <summary>
        /// Gets the name of this element.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An <see cref="T:System.Xml.Linq.XName"/> that contains the name of this element.
        /// </returns>
        public new string Name
        {
            get { return GetAttribute("name"); }
        }

        /// <summary>
        /// Gets the jid.
        /// </summary>
        /// <value>The jid.</value>
        public Jid Jid
        {
            get { return GetAttributeJid("jid"); }
        }
    }
}