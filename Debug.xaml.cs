using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using Matrix;
using Matrix.Xmpp.Client;

namespace SilverlightMuc
{
    public partial class Debug : UserControl
    {
        enum eDirection
        {
            Incoming,
            Outgoing
        }

        readonly XmppClient _xmppClient;

        #region << Costructor >>
        public Debug(XmppClient client)
        {
            InitializeComponent();

            _xmppClient = client;
            _xmppClient.OnReceiveXml += _xmppClient_OnReceiveXml;
            _xmppClient.OnSendXml += _xmppClient_OnSendXml;
        }
        #endregion

        #region << XmppClient events >>
        /// <summary>
        /// Handles the OnSendXml event of the XmppClient.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextEventArgs"/> instance containing the event data.</param>
        void _xmppClient_OnSendXml(object sender, TextEventArgs e)
        {
            // no Invoke required, does the library for us
            AddDebug(e.Text, eDirection.Outgoing);
        }

        /// <summary>
        /// Handles the OnReceiveXml event of the XmppClient.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextEventArgs"/> instance containing the event data.</param>
        void _xmppClient_OnReceiveXml(object sender, TextEventArgs e)
        {
            // no Invoke required, does the library for us
            AddDebug(e.Text, eDirection.Incoming);
        }
        #endregion

        #region << private members >>
        /// <summary>
        /// Display the debug data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="dir">The dir.</param>
        private void AddDebug(string xml, eDirection dir)
        {
            string sdate = DateTime.Now.ToLongTimeString();
            
            Run run1;
            if (dir == eDirection.Incoming)
                run1 = new Run { Text = sdate + " RECV: ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Blue) };
            else
                run1 = new Run { Text = sdate + " SEND: ", FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Red) };
            
            var run2 = new Run { Text = xml };
                        
            txtDebug.Inlines.Add(run1);
            txtDebug.Inlines.Add(run2);
            txtDebug.Inlines.Add(new LineBreak());

            scrollIn.ScrollToVerticalOffset(double.MaxValue);
        }
        #endregion
    }
}