using Demo.service;
using Demo.ui.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Demo.model.vo;
using System.Collections.ObjectModel;

namespace Demo.ui.view.dialog
{
    /// <summary>
    /// Interaction logic for JigEditDlg.xaml
    /// </summary>
    public partial class RecipeEditConfDlg : Window
    {
        private bool initWindows;
        private ObservableCollection<TubeRecipeModifyViewModel> mItemModels;
        private bool mApply;

        public RecipeEditConfDlg()
        {
            InitializeComponent();

            initWindows = false;
            
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (!initWindows)
            {
                mItemModels = new ObservableCollection<TubeRecipeModifyViewModel>();
                DataContext = mItemModels;
                StartUpdateItemTask();
                mApply = false;
                initWindows = true;
            }
        }

        public bool Apply
        {
            get { return mApply; }
            set { mApply = true; }
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            mApply = false;
            this.Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            mApply = true;
            this.Close();
        }

        private void StartUpdateItemTask() {
            //new Thread(() =>
            //{
            //    UpdateItems();
            //}).Start();

            Action act = () =>
            {
                UpdateItems();
                //foreach (var item in GetData())
                    //mItemModels.Add(item);
            };
            //act.BeginInvoke(null, null);
            Dispatcher.CurrentDispatcher.BeginInvoke(act, DispatcherPriority.ApplicationIdle);
        }

        private IEnumerable<TubeRecipeModifyViewModel> GetData()
        {
            List<TubeRecipeProcItemViewModel> recipeItemModels = TubeViewModelFactory.Instance.GetRecipeItemViewModel("TubeRecipePage");
            int itemIndex = 0;
            for (int i = 0; i < recipeItemModels.Count; ++i)
            {


                if (recipeItemModels[i].StepTypeValue != recipeItemModels[i].StepTypeValueEdit)
                {
                    TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                    recipeModifyViewModel.StepID = "Step" + (i + 1);
                    recipeModifyViewModel.Description = "Step Type";
                    recipeModifyViewModel.OldValue = "" + recipeItemModels[i].StepTypeValue;
                    recipeModifyViewModel.NewValue = "" + recipeItemModels[i].StepTypeValueEdit;
                    yield return recipeModifyViewModel;
                };

                if (recipeItemModels[i].StepTimeValue != recipeItemModels[i].StepTimeValueEdit)
                {
                    TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                    recipeModifyViewModel.StepID = "Step" + (i + 1);
                    recipeModifyViewModel.Description = "Step Time";
                    recipeModifyViewModel.OldValue = "" + recipeItemModels[i].StepTimeValue;
                    recipeModifyViewModel.NewValue = "" + recipeItemModels[i].StepTimeValueEdit;
                    yield return recipeModifyViewModel;
                }

                for (int j = 0; j < 8; ++j)
                {
                    if (recipeItemModels[i].MfcValues[j] != recipeItemModels[i].MfcValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "MFC" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].MfcValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].MfcValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                }
                for (int j = 0; j < 2; ++j)
                {
                    if (recipeItemModels[i].AnaValueEdits[j].EndsWith("r"))
                    {
                        if (int.Parse(recipeItemModels[i].AnaValueEdits[j].Substring(0, recipeItemModels[i].AnaValueEdits[j].Length - 1)) != recipeItemModels[i].AnaValues[j])
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "ANA" + (j + 1);
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaValues[j];
                            recipeModifyViewModel.NewValue = "" + int.Parse(recipeItemModels[i].AnaValueEdits[j].Substring(0, recipeItemModels[i].AnaValueEdits[j].Length - 1));
                            yield return recipeModifyViewModel;
                        }

                        if (recipeItemModels[i].RampValues[8 + j] != true)
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "ANA" + (j + 1) + " Ramp";
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[8 + j];
                            recipeModifyViewModel.NewValue = "True";
                            yield return recipeModifyViewModel;
                        }
                    }
                    else
                    {
                        if (int.Parse(recipeItemModels[i].AnaValueEdits[j]) != recipeItemModels[i].AnaValues[j])
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "ANA" + (j + 1);
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaValues[j];
                            recipeModifyViewModel.NewValue = recipeItemModels[i].AnaValueEdits[j];
                            yield return recipeModifyViewModel;
                        }
                        if (recipeItemModels[i].RampValues[8 + j] == true)
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "ANA" + (j + 1) + " Ramp";
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[8 + j];
                            recipeModifyViewModel.NewValue = "False";
                            yield return recipeModifyViewModel;
                        }
                    }
                }
                for (int j = 0; j < 6; ++j)
                {
                    if (recipeItemModels[i].TemperValueEdits[j].EndsWith("r"))
                    {
                        if (int.Parse(recipeItemModels[i].TemperValueEdits[j].Substring(0, recipeItemModels[i].TemperValueEdits[j].Length - 1)) != recipeItemModels[i].TemperValues[j])
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "TEMPER" + (j + 1);
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperValues[j];
                            recipeModifyViewModel.NewValue = "" + int.Parse(recipeItemModels[i].TemperValueEdits[j].Substring(0, recipeItemModels[i].TemperValueEdits[j].Length - 1));
                            yield return recipeModifyViewModel;
                        }

                        if (recipeItemModels[i].RampValues[21 + j] != true)
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Ramp";
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[21 + j];
                            recipeModifyViewModel.NewValue = "True";
                            yield return recipeModifyViewModel;
                        }
                    }
                    else
                    {
                        if (int.Parse(recipeItemModels[i].TemperValueEdits[j]) != recipeItemModels[i].TemperValues[j])
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "TEMPER" + (j + 1);
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperValues[j];
                            recipeModifyViewModel.NewValue = recipeItemModels[i].TemperValueEdits[j];
                            yield return recipeModifyViewModel;
                        }

                        if (recipeItemModels[i].RampValues[21 + j] == true)
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Ramp";
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[21 + j];
                            recipeModifyViewModel.NewValue = "False";
                            yield return recipeModifyViewModel;
                        }
                    }
                }
                for (int j = 0; j < 32; ++j)
                {
                    if (recipeItemModels[i].DoValues[j] != recipeItemModels[i].DoValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Do" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].DoValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].DoValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                    if (recipeItemModels[i].EvValues[j] != recipeItemModels[i].EvValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Ev" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].EvValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].EvValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                    /*if (recipeItemModels[i].RampValues[j] != recipeItemModels[i].RampValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Ramp" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].RampValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }*/
                    if (recipeItemModels[i].DiValues[j] != recipeItemModels[i].DiValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Di" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].DiValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].DiValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                }
                for (int j = 0; j < 2; ++j)
                {
                    if (recipeItemModels[i].PadValues[j] != recipeItemModels[i].PadValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Paddle" + ((j == 0) ? "Position" : "Speed");
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].PadValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].PadValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                }
                for (int j = 0; j < 1; ++j)
                {
                    if (recipeItemModels[i].AbortStepIdxs[j] != recipeItemModels[i].AbortStepIdxEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Abort Step";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AbortStepIdxs[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AbortStepIdxEdits[j];
                        yield return recipeModifyViewModel;
                    }
                }
                for (int j = 0; j < 1; ++j)
                {
                    if (recipeItemModels[i].DelayValues[j] != recipeItemModels[i].DelayValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Delay Time";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].DelayValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].DelayValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                }
                for (int j = 0; j < 2; ++j)
                {
                    if (recipeItemModels[i].AnaAbortValues[j] != recipeItemModels[i].AnaAbortValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "ANA" + (j + 1) + " Abort";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaAbortValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AnaAbortValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                    if (recipeItemModels[i].AnaHoldValues[j] != recipeItemModels[i].AnaHoldValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "ANA" + (j + 1) + " Hold";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaHoldValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AnaHoldValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                    if (recipeItemModels[i].AnaNextValues[j] != recipeItemModels[i].AnaNextValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "ANA" + (j + 1) + " Next";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaNextValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AnaNextValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                    if (recipeItemModels[i].AnaAlarmValues[j] != recipeItemModels[i].AnaAlarmValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "ANA" + (j + 1) + " Alarm";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaAlarmValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AnaAlarmValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                }
                for (int j = 0; j < 6; ++j)
                {
                    if (recipeItemModels[i].TemperAbortValues[j] != recipeItemModels[i].TemperAbortValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Abort";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperAbortValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].TemperAbortValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                    if (recipeItemModels[i].TemperHoldValues[j] != recipeItemModels[i].TemperHoldValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Hold";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperHoldValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].TemperHoldValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                    if (recipeItemModels[i].TemperNextValues[j] != recipeItemModels[i].TemperNextValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Next";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperNextValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].TemperNextValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                    if (recipeItemModels[i].TemperAlarmValues[j] != recipeItemModels[i].TemperAlarmValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Alarm";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperAlarmValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].TemperAlarmValueEdits[j];
                        yield return recipeModifyViewModel;
                    }
                }
            }
        }

        private void UpdateItems() {
            List<TubeRecipeProcItemViewModel> recipeItemModels = TubeViewModelFactory.Instance.GetRecipeItemViewModel("TubeRecipePage");
            int itemIndex = 0;
            for (int i = 0; i < recipeItemModels.Count; ++i)
            {

                
                if (recipeItemModels[i].StepTypeValue != recipeItemModels[i].StepTypeValueEdit) {
                    TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                    recipeModifyViewModel.StepID = "Step" + (i + 1);
                    recipeModifyViewModel.Description = "Step Type";
                    recipeModifyViewModel.OldValue = "" + recipeItemModels[i].StepTypeValue;
                    recipeModifyViewModel.NewValue = "" + recipeItemModels[i].StepTypeValueEdit;
                    mItemModels.Add(recipeModifyViewModel);
                };

                if (recipeItemModels[i].StepTimeValue != recipeItemModels[i].StepTimeValueEdit) {
                    TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                    recipeModifyViewModel.StepID = "Step" + (i + 1);
                    recipeModifyViewModel.Description = "Step Time";
                    recipeModifyViewModel.OldValue = "" + recipeItemModels[i].StepTimeValue;
                    recipeModifyViewModel.NewValue = "" + recipeItemModels[i].StepTimeValueEdit;
                    mItemModels.Add(recipeModifyViewModel);
                }

                for (int j = 0; j < 8; ++j)
                {
                    if (recipeItemModels[i].MfcValues[j] != recipeItemModels[i].MfcValueEdits[j]) {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "MFC" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].MfcValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].MfcValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                }
                for (int j = 0; j < 2; ++j)
                {
                    if (recipeItemModels[i].AnaValueEdits[j].EndsWith("r"))
                    {
                        if(int.Parse(recipeItemModels[i].AnaValueEdits[j].Substring(0, recipeItemModels[i].AnaValueEdits[j].Length - 1)) != recipeItemModels[i].AnaValues[j]){
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "ANA" + (j + 1);
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaValues[j];
                            recipeModifyViewModel.NewValue = "" + int.Parse(recipeItemModels[i].AnaValueEdits[j].Substring(0, recipeItemModels[i].AnaValueEdits[j].Length - 1));
                            mItemModels.Add(recipeModifyViewModel);
                        }

                        if (recipeItemModels[i].RampValues[8 + j] != true) {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "ANA" + (j + 1) + " Ramp";
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[8 + j];
                            recipeModifyViewModel.NewValue = "True";
                            mItemModels.Add(recipeModifyViewModel);
                        }
                    }
                    else
                    {
                        if (int.Parse(recipeItemModels[i].AnaValueEdits[j]) != recipeItemModels[i].AnaValues[j])
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "ANA" + (j + 1);
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaValues[j];
                            recipeModifyViewModel.NewValue = recipeItemModels[i].AnaValueEdits[j];
                            mItemModels.Add(recipeModifyViewModel);
                        }
                        if (recipeItemModels[i].RampValues[8 + j] == true)
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "ANA" + (j + 1) + " Ramp";
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[8 + j];
                            recipeModifyViewModel.NewValue = "False";
                            mItemModels.Add(recipeModifyViewModel);
                        }
                    }
                }
                for (int j = 0; j < 6; ++j)
                {
                    if (recipeItemModels[i].TemperValueEdits[j].EndsWith("r"))
                    {
                        if (int.Parse(recipeItemModels[i].TemperValueEdits[j].Substring(0, recipeItemModels[i].TemperValueEdits[j].Length - 1)) != recipeItemModels[i].TemperValues[j])
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "TEMPER" + (j + 1);
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperValues[j];
                            recipeModifyViewModel.NewValue = "" + int.Parse(recipeItemModels[i].TemperValueEdits[j].Substring(0, recipeItemModels[i].TemperValueEdits[j].Length - 1));
                            mItemModels.Add(recipeModifyViewModel);
                        }

                        if (recipeItemModels[i].RampValues[21 + j] != true)
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Ramp";
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[21 + j];
                            recipeModifyViewModel.NewValue = "True";
                            mItemModels.Add(recipeModifyViewModel);
                        }
                    }
                    else
                    {
                        if (int.Parse(recipeItemModels[i].TemperValueEdits[j]) != recipeItemModels[i].TemperValues[j])
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "TEMPER" + (j + 1);
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperValues[j];
                            recipeModifyViewModel.NewValue = recipeItemModels[i].TemperValueEdits[j];
                            mItemModels.Add(recipeModifyViewModel);
                        }

                        if (recipeItemModels[i].RampValues[21 + j] == true)
                        {
                            TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                            recipeModifyViewModel.StepID = "Step" + (i + 1);
                            recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Ramp";
                            recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[21 + j];
                            recipeModifyViewModel.NewValue = "False";
                            mItemModels.Add(recipeModifyViewModel);
                        }
                    }
                }
                for (int j = 0; j < 32; ++j)
                {
                    if (recipeItemModels[i].DoValues[j] != recipeItemModels[i].DoValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Do" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].DoValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].DoValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                    if (recipeItemModels[i].EvValues[j] != recipeItemModels[i].EvValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Ev" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].EvValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].EvValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                    /*if (recipeItemModels[i].RampValues[j] != recipeItemModels[i].RampValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Ramp" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].RampValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].RampValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }*/
                    if (recipeItemModels[i].DiValues[j] != recipeItemModels[i].DiValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Di" + (j + 1);
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].DiValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].DiValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                }
                for (int j = 0; j < 2; ++j)
                {
                    if (recipeItemModels[i].PadValues[j] != recipeItemModels[i].PadValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Paddle" + ((j==0)?"Position":"Speed");
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].PadValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].PadValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                }
                for (int j = 0; j < 1; ++j)
                {
                    if (recipeItemModels[i].AbortStepIdxs[j] != recipeItemModels[i].AbortStepIdxEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Abort Step";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AbortStepIdxs[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AbortStepIdxEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                }
                for (int j = 0; j < 1; ++j)
                {
                    if (recipeItemModels[i].DelayValues[j] != recipeItemModels[i].DelayValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "Delay Time";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].DelayValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].DelayValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                }
                for (int j = 0; j < 2; ++j)
                {
                    if (recipeItemModels[i].AnaAbortValues[j] != recipeItemModels[i].AnaAbortValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "ANA" + (j + 1) + " Abort";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaAbortValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AnaAbortValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                    if (recipeItemModels[i].AnaHoldValues[j] != recipeItemModels[i].AnaHoldValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "ANA" + (j + 1) + " Hold";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaHoldValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AnaHoldValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                    if (recipeItemModels[i].AnaNextValues[j] != recipeItemModels[i].AnaNextValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "ANA" + (j + 1) + " Next";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaNextValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AnaNextValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                    if (recipeItemModels[i].AnaAlarmValues[j] != recipeItemModels[i].AnaAlarmValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "ANA" + (j + 1) + " Alarm";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].AnaAlarmValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].AnaAlarmValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                }
                for (int j = 0; j < 6; ++j)
                {
                    if (recipeItemModels[i].TemperAbortValues[j] != recipeItemModels[i].TemperAbortValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Abort";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperAbortValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].TemperAbortValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                    if (recipeItemModels[i].TemperHoldValues[j] != recipeItemModels[i].TemperHoldValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Hold";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperHoldValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].TemperHoldValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                    if (recipeItemModels[i].TemperNextValues[j] != recipeItemModels[i].TemperNextValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Next";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperNextValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].TemperNextValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                    if (recipeItemModels[i].TemperAlarmValues[j] != recipeItemModels[i].TemperAlarmValueEdits[j])
                    {
                        TubeRecipeModifyViewModel recipeModifyViewModel = new TubeRecipeModifyViewModel(++itemIndex);
                        recipeModifyViewModel.StepID = "Step" + (i + 1);
                        recipeModifyViewModel.Description = "TEMPER" + (j + 1) + " Alarm";
                        recipeModifyViewModel.OldValue = "" + recipeItemModels[i].TemperAlarmValues[j];
                        recipeModifyViewModel.NewValue = "" + recipeItemModels[i].TemperAlarmValueEdits[j];
                        mItemModels.Add(recipeModifyViewModel);
                    }
                }
            }
        }
    }
}
