   M S B A M L   `   `   `  ���� ?  ;Demo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Demo.ui.view.cardDemo.ui.view.card  5clr-namespace:Demo.ui.view.cardDemo.ui.view.card  ?$clr-namespace:Demo.ui.view.converterDemo.ui.view.converter  ,    &Demo.ui.view.card.TubeAlarmHistoryCard   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     1localclr-namespace:Demo.ui.view.card     0cardclr-namespace:Demo.ui.view.card     :	converter$clr-namespace:Demo.ui.view.converter     $
��Left=�$	��Top=�.��� .+�&   System.Windows.Controls.DataGrid  -      Name$  alarmDataGrid��$��1��   FrozenColumnCount$ 2��$
��1740��$	��820��   CanUserAddRows .     AutoGenerateColumns .  $
��Left=�$	��Top=�����    Columns 4  .System.Windows.Controls.DataGridCheckBoxColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Checked��   CanUserSort .  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	AlarmID+ �� Mode$ TwoWay=�	 �� UpdateSourceTrigger$	 PropertyChanged=�
   ElementStyle
 ��  �� 
TargetType" ��.���  �� Property � �� �� Value$ Center=���  � ��$ Center=�0  *System.Windows.Controls.DataGridTextColumn  -   desc��$ Description��   LeftAlignStyle#
 [     Width5  /System.Windows.Controls.DataGridLengthConverter$
 1300    
IsReadOnly .    
Foreground
 ����� �� *	AlarmDesc+