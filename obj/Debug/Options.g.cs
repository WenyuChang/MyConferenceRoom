#pragma checksum "C:\Users\changwy\Documents\Visual Studio 2008\Projects\SilverlightMuc\Options.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F16A17839523471E52B23049FD4B75D6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SilverlightMuc {
    
    
    public partial class Options : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock txtTitle;
        
        internal System.Windows.Controls.TextBox txtNickname;
        
        internal System.Windows.Controls.TextBlock txtInfo;
        
        internal System.Windows.Controls.DataVisualization.Charting.Chart chart;
        
        internal System.Windows.Controls.NumericUpDown numSpace;
        
        internal System.Windows.Controls.Button cmdOK;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightMuc;component/Options.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtTitle = ((System.Windows.Controls.TextBlock)(this.FindName("txtTitle")));
            this.txtNickname = ((System.Windows.Controls.TextBox)(this.FindName("txtNickname")));
            this.txtInfo = ((System.Windows.Controls.TextBlock)(this.FindName("txtInfo")));
            this.chart = ((System.Windows.Controls.DataVisualization.Charting.Chart)(this.FindName("chart")));
            this.numSpace = ((System.Windows.Controls.NumericUpDown)(this.FindName("numSpace")));
            this.cmdOK = ((System.Windows.Controls.Button)(this.FindName("cmdOK")));
        }
    }
}
