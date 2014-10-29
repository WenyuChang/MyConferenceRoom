using System.Windows;
using System.Windows.Controls;
using Matrix;

namespace SilverlightMuc
{
    public class ConferenceTab : TabItem
    {
        public ConferenceTab(Jid jid)
        {
            m_Jid = jid;
        }

        private Jid m_Jid;

        public Jid Jid
        {
            get { return m_Jid; }
        }

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();

        //    ContentControl cc = (ContentControl)this.GetTemplateChild("HeaderTopSelected");
        //    cc.HorizontalContentAlignment = HorizontalAlignment.Right;
        //    cc = (ContentControl)this.GetTemplateChild("HeaderTopUnselected");
        //    cc.HorizontalContentAlignment = HorizontalAlignment.Right;
        //}

    }
}
