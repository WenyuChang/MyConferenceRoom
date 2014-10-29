using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;

namespace SilverlightMuc
{
    /// <summary>
    /// Options Dialog
    /// </summary>
    public partial class Options : UserControl
    {
        // init key value pair with dummy values
        readonly KeyValuePair<string, int>[] chartData = new[] {
                    new KeyValuePair<string, int>("", 1),
                    new KeyValuePair<string, int>("", 1)
                    };
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// </summary>
        public Options()
        {
            InitializeComponent();
            chart.DataContext = chartData;
        }
        
        public void Init()
        {
            if (Storage.Settings.Nickname != null)
                txtNickname.Text = Storage.Settings.Nickname;

            CalcSpace();
        }

        /// <summary>
        /// Calcs the space.
        /// </summary>
        private void CalcSpace()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                long quota = isf.Quota;
                long availSpace = isf.AvailableFreeSpace;
                long usedSpace = isf.Quota - availSpace;

                long availSpaceKB = availSpace / 1024;
                long usedSpaceKB = usedSpace / 1024;
                //long quotaKB = quota / 1024;

                double availSpaceMB = availSpace / 1024f / 1024f;
                double usedSpaceMB = usedSpace / 1024f / 1024f;
                double quotaMB = quota / 1024f / 1024f;

                txtInfo.Text = string.Format("current quota: {0} MB", quotaMB.ToString("N2"));

                chartData[0] = new KeyValuePair<string, int>(usedSpaceMB.ToString("N1") + " MB in use", (int)usedSpaceKB);
                chartData[1] = new KeyValuePair<string, int>(availSpaceMB.ToString("N1") + " MB available", (int)availSpaceKB);
                chart.Refresh();
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdIncreaseQuota control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void cmdIncreaseQuota_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (numSpace.Value*1024*1024 > isf.Quota)
                    {

                        long incSpace = 1024*1024*(long) numSpace.Value;

                        bool b = isf.IncreaseQuotaTo(incSpace);
                        if (b)
                        {
                            // User approved increase. Perform your actions
                            CalcSpace();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtNickname.Text.Length > 0)
                Storage.Settings.Nickname = txtNickname.Text;

            Visibility = Visibility.Collapsed;
        }
    }
}