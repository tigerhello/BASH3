   M S B A M L   `   `   `  ���� ?  ;Demo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Demo.ui.view.dialogDemo.ui.view.dialog  '    !Demo.ui.view.dialog.UserUpdateDlg   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     3local!clr-namespace:Demo.ui.view.dialog       � Title$  User Modification��$	��330��$	��400�� � 	Resources    
MyCommand1 �� Text  
MyCommand2��%<   &	        &	      �� $
 Text���� $ Another Text�� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command# [    -   # [  J� InputBindings �� 
 �� Key$ Return�� �� Command# [  �� $ A�� �� 	Modifiers$	 Altf�# [  �� $	 Esc��# [ �� $ C��$	 Altf�# [ .��� 	 � ColumnDefinitions	 �� $��1* ��� $	��300 ��� $��1* �
 � RowDefinitions
 �� $��1* ��� $	��220 ��� $��30 ��� $��1* �.+�� $��1��$��1��.+��� $��User ID:��$��0,0,0,0q�$	��100��$��30��$��14(�$
��Left=�$	��Top=��� $��Administrator��$��	100,0,0,0q� �� Padding$ 4,4,0,0q�$	��200��$��30��$��14(�$
��Left=�$	��Top=� �� TextAlignment$
 Left=�{�.  �� $��Old Password:��$��0,70,0,0q�$	��100��$��30��$��14(�$
��Left=�$	��Top=�G� -    G�Name$ passwordOld��$��
100,70,0,0q�$��4,4,0,0q�$	��200��$��30��$��14(�$
��Left=�$	��Top=��� $��New Password:��$��	0,105,0,0q�$	��100��$��30��$��14(�$
��Left=�$	��Top=�G� -   $ passwordNew��$��100,105,0,0q�$��4,4,0,0q�$	��200��$��30��$��14(�$
��Left=�$	��Top=��� $��Confirmation:��$��	0,140,0,0q�$	��100��$��30��$��14(�$
��Left=�$	��Top=�G� -   $ passwordConf��$��100,140,0,0q�$��4,4,0,0q�$	��200��$��30��$��14(�$
��Left=�$	��Top=�� $��2��$��1��.+��� -   $��Apply��$��	0,2,110,2q�$��Right=�$��90��$��26���� -   $��Cancel��$��0,2,10,2q�$��Right=�$��90��$��26��