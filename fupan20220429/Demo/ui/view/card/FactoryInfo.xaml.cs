using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using Demo.ui.view.dialog;
using log4net;
using Demo.ui.view.converter;
using Demo.ui.model;
using Demo.utilities;
using System.Windows.Data;

namespace Demo.ui.view.card
{

    public partial class FactoryInfo : UserControl
    {    
        public FactoryInfo()
        {
            InitializeComponent();
        }

        private void CCTV_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {  
           //richTextBox1.Clear();        
        }
    }
}
