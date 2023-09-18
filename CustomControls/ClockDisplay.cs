using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quality_Control.CustomControls
{
    public class ClockDisplay : Control
    {
        private Line arrow ;
        public int angle = 0;
        static ClockDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClockDisplay), new FrameworkPropertyMetadata(typeof(ClockDisplay)));
        }
        public override void OnApplyTemplate()
        {
            arrow = Template.FindName("arrow", this) as Line;
            updateArrowAngle();
            base.OnApplyTemplate();
        }
        public void updateArrowAngle()
        {
            arrow.RenderTransform = new RotateTransform(angle,0.5,0.5);
        }
        public void setAngle(int ang)
        {
            angle = ang;
        }
    }
}
