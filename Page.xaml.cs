using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Matrix;
using Matrix.Xml;
using Matrix.Xmpp.Client;

namespace SilverlightMuc
{
    public partial class Page : UserControl
    {
        private readonly Dictionary<Jid, ConferenceTab> conferences = new Dictionary<Jid,ConferenceTab>();
        private static readonly XmppClient xmppClient = new XmppClient();
        private readonly WebClient webClient = new WebClient();
        private static MucManager mucManager;

        public Page()
        {
            webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;
            var uri = new System.Uri("config.xml", UriKind.Relative);
            webClient.DownloadStringAsync(uri);

            Loaded += Page_Loaded;
            xmppClient.OnLogin += xmppClient_OnLogin;
            xmppClient.OnError += xmppClient_OnError;
            // initialize the MucManager
            mucManager = new MucManager(xmppClient);
            mucManager.OnInvite += new EventHandler<MessageEventArgs>(mucManager_OnInvite);

            InitializeComponent();

            gridNickname.Visibility = String.IsNullOrEmpty(Storage.Settings.Nickname) ? Visibility.Visible : Visibility.Collapsed;

            if (cmdConnect.Visibility == Visibility.Visible)
                AddDebugTab();
        }

        /// <summary>
        /// Handles invite messages to a Muc room.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MessageEventArgs"/> instance containing the event data.</param>
        void mucManager_OnInvite(object sender, MessageEventArgs e)
        {
            // not used here in the example
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // the download of the xml file does not work on local host for me,
            // this is why we have a default xml here which is used for local debugging
            const string DEFAULT_RESULT = @"<rooms><room name='dev' jid='dev@conference.ag-software.de'/><room name='linuxcn' jid='linuxcn@conference.jabber.org'/><room name='jdev' jid='jdev@conference.jabber.org'/><room name='jabber' jid='jabber@conference.jabber.org'/></rooms>";

            string res = e.Error == null ? e.Result : DEFAULT_RESULT;
        
            var el = XmppXElement.LoadXml(res);
            if (el is Rooms)
            {
                var rooms = el as Rooms;
                foreach (var pair in rooms.GetRooms())
                {
                    var btn = new Button {Content = pair.Name, Tag = pair.Jid, Margin = new Thickness(3), Width = 60};
                    btn.Click += Button_Click;

                    ToolTipService.SetToolTip(btn, pair.Jid.ToString());

                    stackRooms.Children.Add(btn);
                }
            }
        }

        void xmppClient_OnError(object sender, Matrix.EventArgs e)
        {
            MessageBox.Show("OnError");
        }

        void xmppClient_OnLogin(object sender, Matrix.EventArgs e)
        {
            imgConnected.Source = new BitmapImage(new System.Uri("img/connected.png", UriKind.Relative));
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (cmdConnect.Visibility == Visibility.Collapsed)
                ConnectAnonymous();
        }

        public static XmppClient XmppClient
        {
            get { return xmppClient; }
        }

        /// <summary>
        /// Connects anonymous to a XMPP server.
        /// The server must be configured to support anonymous logins of couse.
        /// </summary>
        private void ConnectAnonymous()
        {
            xmppClient.XmppDomain = "ag-software.de";
            xmppClient.Hostname = "matrix.ag-software.de";
            xmppClient.Port = 5222;

            xmppClient.ProxyType = Matrix.Net.Proxy.ProxyType.HttpTunnel;
            xmppClient.ProxyPort = 4503;
            //xmppClient.ProxyHostname    = "localhost";
            xmppClient.ProxyHostname = "matrix.ag-software.de";

            xmppClient.AnonymousLogin   = true;

            xmppClient.Open();
        }

        private void AddDebugTab()
        {            
            var jid = new Jid("Debug");
            var itm = new ConferenceTab(jid);
            var head = new TabHeader("Debug");
            itm.Header = head;
            head.btnTabHeader.Click += cmdTabHeader_Click;

            itm.Content = new Debug(xmppClient);
            
            conferences.Add(jid, itm);

            tabRooms.Items.Add(itm);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!xmppClient.StreamActive)
                return;
            
            var btn = sender as Button;
            if (btn != null)
            {
                var jid = btn.Tag as Jid;

                AddConference(jid);
            }
        }

        private void AddConference(Jid jid)
        {
            if (!conferences.ContainsKey(jid))
            {
                var itm = new ConferenceTab(jid);

                var head = new TabHeader(jid);

                head.btnTabHeader.Click += cmdTabHeader_Click;

                itm.Header = head;
                itm.Content = new ConferenceRoom(xmppClient, jid);
                itm.IsSelected = true;
                tabRooms.Items.Add(itm);

                conferences.Add(jid, itm);
            }
        }

        void cmdTabHeader_Click(object sender, RoutedEventArgs e)
        {
            var btn  = sender as Button;
            if (btn != null)
            {
                var roomJid = btn.Tag as Jid;
            
                var element = VisualTreeHelper.GetParent(btn) as FrameworkElement;

                if (element is Grid)
                {
                    ConferenceTab tab = tabRooms.Items.OfType<ConferenceTab>().FirstOrDefault(t => t.Jid.Equals(roomJid));

                    if (tab.Content is ConferenceRoom)
                    {
                        var conf = tab.Content as ConferenceRoom;
                        conf.Exit();
                    }
                    tabRooms.Items.Remove(tab);

                    if (roomJid != null) conferences.Remove(roomJid);
                }
            }
        }

        private void cmdConnect_Click(object sender, RoutedEventArgs e)
        {
            ConnectAnonymous();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNickname.Text))
            {
                Storage.Settings.Nickname = txtNickname.Text;
                gridNickname.Visibility = Visibility.Collapsed;
            }
        }

        private void cmdMenu_Click(object sender, RoutedEventArgs e)
        {
            menu.IsOpen = true;
        }

        private void cmdMenu_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            menu.IsOpen = false;
        }

        private void StackPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            menu.IsOpen = false;
        }

        private void StackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            menu.IsOpen = true;
        }

        private void cmdOptions_Click(object sender, RoutedEventArgs e)
        {
            gridOptions.Init();
            gridOptions.Visibility = Visibility.Visible;
        }

      
    }
}