#pragma checksum "C:\Users\changwy\Documents\Visual Studio 2008\Projects\SilverlightMuc\Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4BD24EDB75D7ECEB130F22A793648920"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SilverlightMuc;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
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
    
    
    public partial class Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button cmdMenu;
        
        internal System.Windows.Controls.Primitives.Popup menu;
        
        internal System.Windows.Controls.Button cmdOption;
        
        internal System.Windows.Controls.TabControl tabRooms;
        
        internal System.Windows.Controls.Image imgConnected;
        
        internal System.Windows.Controls.StackPanel stackRooms;
        
        internal System.Windows.Controls.Button cmdConnect;
        
        internal SilverlightMuc.Options gridOptions;
        
        internal System.Windows.Controls.Grid gridNickname;
        
        internal System.Windows.Controls.TextBlock txtTitle;
        
        internal System.Windows.Controls.TextBlock lblText;
        
        internal System.Windows.Controls.TextBox txtNickname;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightMuc;component/Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.cmdMenu = ((System.Windows.Controls.Button)(this.FindName("cmdMenu")));
            this.menu = ((System.Windows.Controls.Primitives.Popup)(this.FindName("menu")));
            this.cmdOption = ((System.Windows.Controls.Button)(this.FindName("cmdOption")));
            this.tabRooms = ((System.Windows.Controls.TabControl)(this.FindName("tabRooms")));
            this.imgConnected = ((System.Windows.Controls.Image)(this.FindName("imgConnected")));
            this.stackRooms = ((System.Windows.Controls.StackPanel)(this.FindName("stackRooms")));
            this.cmdConnect = ((System.Windows.Controls.Button)(this.FindName("cmdConnect")));
            this.gridOptions = ((SilverlightMuc.Options)(this.FindName("gridOptions")));
            this.gridNickname = ((System.Windows.Controls.Grid)(this.FindName("gridNickname")));
            this.txtTitle = ((System.Windows.Controls.TextBlock)(this.FindName("txtTitle")));
            this.lblText = ((System.Windows.Controls.TextBlock)(this.FindName("lblText")));
            this.txtNickname = ((System.Windows.Controls.TextBox)(this.FindName("txtNickname")));
            this.cmdOK = ((System.Windows.Controls.Button)(this.FindName("cmdOK")));
        }
    }
}
