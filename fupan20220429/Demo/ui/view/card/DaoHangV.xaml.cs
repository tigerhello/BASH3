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

    public partial class DaoHangV : UserControl
    {


        public TubeNavigatorViewModel mViewModel;

   
        public DaoHangV()
        {
            InitializeComponent();



            //string[] aa = Enum.GetNames(typeof(DSetKind));


            mViewModel = new TubeNavigatorViewModel();
            //mViewModel.DeStatus = new List<bool>();
            DataContext = mViewModel;

            //for(int i=0;i<13;i++)
            //{
            //    mViewModel.DeStatus.Add(true);
            //}
        }

    }

}
