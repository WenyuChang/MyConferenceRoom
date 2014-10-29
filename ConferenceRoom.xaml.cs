using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;

using Matrix;
using Matrix.Xmpp.Client;
using Matrix.Xmpp.Delay;
using Matrix.Xmpp.Muc;
using Matrix.Xmpp.Muc.User;

using UItem = Matrix.Xmpp.Muc.User.Item;
using XUser = Matrix.Xmpp.Muc.User.X;



namespace SilverlightMuc
{
    public partial class ConferenceRoom : UserControl
    {
        readonly Jid         _roomJid        = null;
        readonly XmppClient  _XmppClient     = null;
        readonly MucManager  _MucManager;

        string      _Nickname = Storage.Settings.Nickname;
        bool        _shiftKey;
        bool        _connected      = false;

       
        const string CHANGE_NICK_COMMAND = "/nick ";
        const string ME_COMMAND = "/me ";

        public ConferenceRoom(XmppClient xmppClient, Jid roomJid)
        {
            InitializeComponent();

            _roomJid = roomJid;
            _XmppClient = xmppClient;
            
            _XmppClient.OnPresence  += new EventHandler<PresenceEventArgs>(_XmppClient_OnPresence);
            _XmppClient.OnMessage   += new EventHandler<MessageEventArgs> (_XmppClient_OnMessage);

            DisplayInfo("connecting...");

            _MucManager = new MucManager(_XmppClient);
            _MucManager.EnterRoom(roomJid, _Nickname);
        }

        #region << XmppClient events >>

        void _XmppClient_OnPresence(object sender, PresenceEventArgs e)
        {
            if (e.Presence.From.Equals(_roomJid, new BareJidComparer()))
            {
                var x = e.Presence.Element<XUser>();

                UItem item = null;
                if (x != null)
                    item = x.Element<UItem>();
                
                if (e.Presence.Type != Matrix.Xmpp.PresenceType.error)
                {
                    if (e.Presence.From.Resource == _Nickname)
                    {
                        // self presence
                        if (item != null)
                        {
                            conferenceRoster.Role   = item.Role;
                            conferenceRoster.Affiliation = item.Affiliation;
                        } 
                    }

                    if (!_connected)
                    {
                        _connected = true;
                        DisplayInfo("connected");
                    }
                    conferenceRoster.SetPresence(e.Presence, _XmppClient);

                    /*
                       <presence
                            from='darkcave@chat.shakespeare.lit/thirdwitch'
                            to='crone1@shakespeare.lit/desktop'
                            type='unavailable'>
                          <x xmlns='http://jabber.org/protocol/muc#user'>
                            <item affiliation='member'
                                  jid='hag66@shakespeare.lit/pda'
                                  nick='oldhag'
                                  role='participant'/>
                            <status code='303'/>
                          </x>
                        </presence>
                     */
                    
                    if (x != null)
                    {
                        if (x.HasStatus(StatusCode.NewNickname))
                        {
                            if (item != null)
                            {
                                string newNick = item.Nickname;
                                string oldNick = e.Presence.From.Resource;
                                DisplayInfo(string.Format("{0} is now known as {1}", oldNick, newNick));
                            }
                        }
                    }

                }
                else
                {
                    Error err = e.Presence.Error;
                    if (err != null)
                        DisplayError(err.Condition.ToString());
                }
            }
        }

        void _XmppClient_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.From.Equals(_roomJid, new BareJidComparer()))
            {
                if(e.Message.Subject != null)
                    NewSubject(e.Message);
                else if(e.Message.Body != null)
                    IncomingMessage(e.Message);               
            }
        }

        #endregion

        /// <summary>
        /// Display a incoming message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void IncomingMessage(Message msg)
        {
            DateTime date = DateTime.Now;

            if (msg.XDelay != null)
            {
                date = msg.XDelay.Stamp;
            }
            else if (msg.Delay != null)
            {
                date = msg.Delay.Stamp;
            }
            
            if (txtIncoming.Inlines.Count > 0)
                txtIncoming.Inlines.Add(new LineBreak());
            
            string sdate = date.ToLongTimeString();           

            var run1 = new Run() { Text = sdate + " ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Blue) };
            txtIncoming.Inlines.Add(run1);

            
            if (msg.Body.StartsWith(ME_COMMAND))
            {
                var text = msg.Body.Replace(ME_COMMAND, "");
                var run2 = new Run() { Text = "* " + msg.From.Resource + ": " + text, FontStyle = FontStyles.Italic, Foreground = new SolidColorBrush(Colors.Red) };
                
                txtIncoming.Inlines.Add(run2);
            }
            else
            {
                var run2 = new Run() { Text = msg.From.Resource + ": ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Blue) };
                var run3 = new Run() { Text = msg.Body };

                txtIncoming.Inlines.Add(run2);
                txtIncoming.Inlines.Add(run3);
            }
            
            scrollIn.ScrollToVerticalOffset(double.MaxValue);
        }

        public void DisplayError(string err)
        {
            string sdate = DateTime.Now.ToLongTimeString();

            Run run1 = new Run() { Text = sdate + " ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Red) };
            Run run2 = new Run() { Text = "Error: ",   FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Red) };
            Run run3 = new Run() { Text = err };

            if (txtIncoming.Inlines.Count > 0)
                txtIncoming.Inlines.Add(new LineBreak());

            txtIncoming.Inlines.Add(run1);
            txtIncoming.Inlines.Add(run2);
            txtIncoming.Inlines.Add(run3);

            scrollIn.ScrollToVerticalOffset(double.MaxValue);
        }

        public void DisplayInfo(string info)
        {
            string sdate = DateTime.Now.ToLongTimeString();

            Run run1 = new Run() { Text = sdate + " ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Green) };
            Run run2 = new Run() { Text = "Info: ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Green) };
            Run run3 = new Run() { Text = info };

            if (txtIncoming.Inlines.Count > 0)
                txtIncoming.Inlines.Add(new LineBreak());

            txtIncoming.Inlines.Add(run1);
            txtIncoming.Inlines.Add(run2);
            txtIncoming.Inlines.Add(run3);

            scrollIn.ScrollToVerticalOffset(double.MaxValue);
        }

        /// <summary>
        /// Sends the chat.
        /// </summary>
        private void SendChat()
        {
            // only send when there is text
            if (string.IsNullOrEmpty(txtOut.Text))
                return;

            // change Nickname
            if (txtOut.Text.StartsWith(CHANGE_NICK_COMMAND))
            {
                ChangeNickname();
            }
            else
            {
                var msg = new Message {To = _roomJid, Type = Matrix.Xmpp.MessageType.groupchat, Body = txtOut.Text};

                _XmppClient.Send(msg);
            }            
            
            txtOut.Text = "";
        }        

        private void ChangeNickname()
        {
            _Nickname = txtOut.Text.Replace(CHANGE_NICK_COMMAND, "");
            _MucManager.ChangeNickname(_roomJid, _Nickname);
            Storage.Settings.Nickname = _Nickname;
        }

        /// <summary>
        /// new Subject was set
        /// </summary>
        /// <param name="msg">The message object which contains the subject.</param>
        public void NewSubject(Message msg)
        {
            txtSubject.Text = msg.Subject;
        }       

        /// <summary>
        /// Handles the KeyUp event of the txtOut control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void txtOut_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Shift)
                _shiftKey = false;

            if (_shiftKey && e.Key == Key.Enter)
            {
                txtOut.Text = txtOut.Text + Environment.NewLine;
                txtOut.SelectionStart = txtOut.Text.Length;
                scrollOut.ScrollToVerticalOffset(double.MaxValue);
            }

            if (!_shiftKey && e.Key == Key.Enter)
                SendChat();
        }

        /// <summary>
        /// Handles the KeyDown event of the txtOut control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void txtOut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Shift)
                _shiftKey = true;
        }

        /// <summary>
        /// Handles the Click event of the cmdSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void cmdSend_Click(object sender, RoutedEventArgs e)
        {
            SendChat();
        }

        /// <summary>
        /// Exit the room
        /// </summary>
        public void Exit()
        {
            if (_connected)
                _MucManager.ExitRoom(_roomJid, _Nickname);
        }
    }
}
