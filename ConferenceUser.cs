using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using Matrix;
using Matrix.Xmpp.Muc;
using Uri=System.Uri;

namespace SilverlightMuc
{
    /// <summary>
    /// ContactListItem
    /// </summary>
    public class ConferenceUser : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        // Declare the PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;

        // NotifyPropertyChanged will raise the PropertyChanged event passing the
        // source property that is being updated.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)        
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));            
        }
        #endregion
        
        
        private readonly Jid    m_Jid;        
        private string          m_Status        = "";
        private StatusImage     m_StatusImage   = StatusImage.Online;
        private Role            m_Role          = Role.none;
        private string          m_PhotoHash;
        private BitmapImage     m_AvatarImage;


        /// <summary>
        /// Initializes a new instance of the <see cref="ConferenceUser"/> class.
        /// </summary>
        /// <param name="jid">The jid.</param>
        public ConferenceUser(Jid jid)
        {
            m_Jid = jid;

            var sr = Application.GetResourceStream(new Uri("SilverlightMuc;component/img/avatar.png", UriKind.Relative));
            var bmp = new BitmapImage();
            bmp.SetSource(sr.Stream);
            m_AvatarImage = bmp;
        }

        /// <summary>
        /// Gets the nickname.
        /// </summary>
        /// <value>The nickname.</value>
        public string Nickname
        {
            get { return m_Jid.Resource; }            
        }

        /// <summary>
        /// Gets the jid.
        /// </summary>
        /// <value>The jid.</value>
        public string Jid
        {
            get { return m_Jid; }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get { return m_Status; }
            set
            {
                m_Status = value;
                NotifyPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        public Role Role
        {
            get { return m_Role; }
            set
            {
                m_Role = value;
                NotifyPropertyChanged("Role");
            }
        }

        /// <summary>
        /// Gets or sets the photo hash.
        /// </summary>
        /// <value>The photo hash.</value>
        public string PhotoHash
        {
            get { return m_PhotoHash; }
            set { m_PhotoHash = value; }
        }

        public BitmapImage AvatarImage
        {
            get { return m_AvatarImage; }
            set
            {
                m_AvatarImage = value;
                NotifyPropertyChanged("AvatarImage");
            }
        }

        /// <summary>
        /// Gets or sets the status image.
        /// </summary>
        /// <value>The status image.</value>
        public StatusImage StatusImage
        {
            get { return m_StatusImage; }
            set 
            { 
                m_StatusImage = value;
                NotifyPropertyChanged("StatusImage");
            }
        }

        public string FontWeight
        {
            get { return Nickname == Storage.Settings.Nickname ? "Bold" : "Normal"; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ConferenceUser))
                return false;
            
            return ((ConferenceUser)obj).Jid.Equals(Jid);
        }
    }
}