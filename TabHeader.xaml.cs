using System.Windows.Controls;
using Matrix;

namespace SilverlightMuc
{
    public partial class TabHeader : UserControl
    {
        public TabHeader(Jid jid)
        {
            InitializeComponent();

            Jid = jid;
            
            txtHeader.Text     = Jid.Bare;
            btnTabHeader.Tag   = Jid;
        }

        public Jid Jid {get; set;}
               
    }
}