using System;
using System.Collections.ObjectModel;

namespace SilverlightMuc
{
    /// <summary>
    /// sorted observable ConferenceUser collection
    /// </summary>
    public class ConferenceUserCollection : ObservableCollection<ConferenceUser>
    {
        readonly object lockObj = new object();

        /// <summary>
        /// Adds the specified user.
        /// The user is added at the correct place (sorted)
        /// </summary>
        /// <param name="user">The user.</param>
        public new void Add(ConferenceUser user)
        {
            try
            {
                lock (lockObj)
                {
                    if (Count == 0)
                    {
                        base.Add(user);
                        return;
                    }
                    
                    for (var i = 0; i < Count; i++)
                    {
                        if (string.Compare(user.Nickname, this[i].Nickname) < 0)
                        {
                            InsertItem(i, user);
                            return;
                        }
                    }
                    InsertItem(Count, user);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
