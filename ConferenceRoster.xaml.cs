using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Matrix.Xmpp;
using Matrix.Xmpp.Client;
using Matrix.Xmpp.Muc;
using Matrix.Xmpp.Vcard;
using Matrix.Xmpp.Vcard.Update;
using Item  = Matrix.Xmpp.Muc.User.Item;
using X     = Matrix.Xmpp.Muc.User.X;

namespace SilverlightMuc
{
    public partial class ConferenceRoster : UserControl
    {
        private readonly object lockObj = new object();
        internal ConferenceUserCollection colConferenceUser = new ConferenceUserCollection();

        public ConferenceRoster()
        {
            InitializeComponent();

            listRoster.ItemsSource = colConferenceUser;
        }

        public Role Role { get; set;}
        public Affiliation Affiliation { get; set; }

        /// <summary>
        /// Sets the presence.
        /// </summary>
        /// <param name="pres">The pres.</param>
        /// <param name="xmppClient">The XMPP client.</param>
        public void SetPresence(Presence pres, XmppClient xmppClient)
        {
            lock (lockObj)
            {
                ConferenceUser cu = colConferenceUser.FirstOrDefault(item => item.Jid.Equals(pres.From));

                if (pres.Type == PresenceType.unavailable)
                {
                    // Contact left Room
                    colConferenceUser.Remove(cu);
                }

                if (cu == null)
                {
                    cu = new ConferenceUser(pres.From);
                    colConferenceUser.Add(cu);
                }

                string status = pres.Status;
                if (status != null)
                    cu.Status = status;
                else
                    cu.Status = "";

                // Contact changed Presence
                switch (pres.Show)
                {
                    case Show.NONE:
                        cu.StatusImage = StatusImage.Online;
                        break;
                    case Show.chat:
                        cu.StatusImage = StatusImage.Chat;
                        break;
                    case Show.away:
                        cu.StatusImage = StatusImage.Away;
                        break;
                    case Show.xa:
                        cu.StatusImage = StatusImage.Xa;
                        break;
                    case Show.dnd:
                        cu.StatusImage = StatusImage.Dnd;
                        break;
                }

                X x = pres.MucUser;
                if (x != null)
                {
                    Item item = x.Item;
                    if (item != null)
                        cu.Role = item.Role;
                }

                var xUpdate = pres.Element<Matrix.Xmpp.Vcard.Update.X>();
                if (xUpdate != null)
                {
                    if (!string.IsNullOrEmpty(xUpdate.Photo) && cu.PhotoHash != xUpdate.Photo)
                    {
                        try
                        {
                            cu.PhotoHash = xUpdate.Photo;
                            if (Storage.AvatarExists(xUpdate.Photo))
                            {
                                byte[] b = Storage.LoadAvatar(cu.PhotoHash);
                                var ms = new MemoryStream(b, 0, b.Length);
                                var bmp = new BitmapImage();
                                bmp.SetSource(ms);
                                cu.AvatarImage = bmp;
                            }
                            else
                            {
                                // Request the vcard for avatar image
                                var viq = new VcardIq {To = pres.From, Type = IqType.get};
                                xmppClient.IqGrabber.SendIq(viq, OnVcardResult, cu);
                            }
                        }
                        catch (Exception ex)
                        {

                            System.Diagnostics.Debug.WriteLine("EXCEPTION ON AVATAR");
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        }

                    }
                }
            }
        }

        void OnVcardResult(object sender, IqEventArgs e)
        {
            if (e.Iq.Type == IqType.result)
            {
                if (e.Iq.Query is Vcard)
                {
                    var vcard = e.Iq.Query as Vcard;

                    var photo = vcard.Photo;
                    if (photo != null)
                    {
                        byte[] b = photo.Binval;
                        if (b != null)
                        {
                            var cu = e.State as ConferenceUser;
                            Storage.SaveAvatar(cu.PhotoHash, b);

                            var ms = new MemoryStream(b, 0, b.Length);
                            var bmp = new BitmapImage();
                            bmp.SetSource(ms);
                            
                            cu.AvatarImage = bmp;
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            colConferenceUser.Clear();
        }

        private void listRoster_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (Role != Role.moderator)
            //    return;
            
            //var button = (sender as FrameworkElement).FindName("btnKick") as UIElement;
            //button.Visibility = Visibility.Visible;

            //button = (sender as FrameworkElement).FindName("btnBan") as UIElement;
            //button.Visibility = Visibility.Visible;

            
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            //var button = (sender as FrameworkElement).FindName("btnKick") as UIElement;
            //button.Visibility = Visibility.Collapsed;

            //button = (sender as FrameworkElement).FindName("btnBan") as UIElement;
            //button.Visibility = Visibility.Collapsed;
        }

        
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            /*
             * there is a but in the listview, the Items never stretch to the max. with
             * So we have to use this code
            */
            var fe = sender as FrameworkElement;
            var cp = VisualTreeHelper.GetParent(fe) as ContentPresenter;
            if (cp != null)
                cp.HorizontalAlignment = HorizontalAlignment.Stretch;
        }
     
    }
}
