   M S B A M L   `   `   `  ����?  ;Demo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Demo.ui.view.cardDemo.ui.view.card  5      5clr-namespace:Demo.ui.view.cardDemo.ui.view.card  5      ?$clr-namespace:Demo.ui.view.converterDemo.ui.view.converter  5      ,    &Demo.ui.view.card.TubeAlarmHistoryCard   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Demo.ui.view.card     5      0cardclr-namespace:Demo.ui.view.card     5      :	converter$clr-namespace:Demo.ui.view.converter     5      $
��Left=�5
      $	��Top=�5      .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5           Name$  alarmDataGrid��5      $��1��6*      FrozenColumnCount$ 2��6:   $
��1740��6h   $	��820��6u      CanUserAddRows .     AutoGenerateColumns .  $
��Left=�6�   $	��Top=�6�   ��6P   ��    Columns 5      4  .System.Windows.Controls.DataGridCheckBoxColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Checked��6y      CanUserSort .  1  +System.Windows.Controls.DataGridBoundColumn   Binding 6)   �� *	AlarmID+ �� Mode$ TwoWay=�	 �� UpdateSourceTrigger$	 PropertyChanged=�
   ElementStyle
 5      �� 5       �� 
TargetType" ��6    .�5      ��  �� Property � �� �� Value$ Center=�6E   �� 5       � ��$ Center=�6C   5!      5#      0  *System.Windows.Controls.DataGridTextColumn  5&      -   5        desc��$ Description��5&   3      LeftAlignStyle#
 [  6H      Width5  /System.Windows.Controls.DataGridLengthConverter$
 1300 6w      
IsReadOnly .    
Foreground
 ����� 6�   �� *	AlarmDesc+5'      5+      5,      5.      