using System.Collections.Generic;
using System.Xml.Linq;

using Matrix.Xml;

namespace SilverlightMuc
{
    /// <summary>
    /// Rooms
    /// </summary>
    public class Rooms : XmppXElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rooms"/> class.
        /// </summary>
        public Rooms()
            : base(XNamespace.None, "rooms")
        {
        }

        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Room> GetRooms()
        {
            return Elements<Room>();
        }
    }
}
