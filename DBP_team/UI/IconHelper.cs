using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace DBP_team.UI
{
    public static class IconHelper
    {
        public static void ApplyAppIcon(Form form)
        {
            if (form == null) return;
            try
            {
                var obj = Properties.Resources.ResourceManager.GetObject("DBP");
                if (obj is Icon)
                {
                    form.Icon = (Icon)obj;
                }
                else if (obj is byte[] bytes)
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        form.Icon = new Icon(ms);
                    }
                }
            }
            catch { }
        }

        public static Icon GetAppIcon()
        {
            try
            {
                var obj = Properties.Resources.ResourceManager.GetObject("DBP");
                if (obj is Icon)
                    return (Icon)obj;
                if (obj is byte[] bytes)
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        return new Icon(ms);
                    }
                }
            }
            catch { }
            return SystemIcons.Application;
        }
    }
}
