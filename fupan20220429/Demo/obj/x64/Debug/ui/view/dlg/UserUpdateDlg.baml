   M S B A M L   `   `   `  ����?  ;Demo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Demo.ui.view.dialogDemo.ui.view.dialog  5   	   '    !Demo.ui.view.dialog.UserUpdateDlg   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:Demo.ui.view.dialog     5   	     � Title$  User Modification��5
   	   $	��330��6#   $	��400��60    � 	Resources 5         
MyCommand1 �� Text  
MyCommand2��%X   &	        &	      �� 5   
   $
 Text��6-   �� 5   
   $ Another Text��6-   5       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command# [  5      5        5   
   -   5        # [ 5      5       J� InputBindings 5      �� 5   
   
 �� Key$ Return��6    �� Command# [  6"   �� 5   
   $ A��6    �� 	Modifiers$	 Altf�6   # [  6-   �� 5   
   $	 Esc��6   # [ 6   �� 5   
   $ C��6   $	 Altf�6   # [ 6-   .��5      � 	 � ColumnDefinitions	 5    
   �� 5!      $��1* �6   �� 5"      $	��300 �6   �� 5#      $��1* �6   
 � RowDefinitions
 5%   
   �� 5&      $��1* �6   �� 5'      $	��220 �6   �� 5(      $��30 �6   �� 5)      $��1* �6   .+�5+   
   � $��1��6   $��1��6   .+�5,      �� $��User ID:��6   $��0,0,0,0q�6'   $	��100��68   $��30��6D   $��14(�6P   $
��Left=�6^   $	��Top=�6y   �� 5-      $��Administrator��6   $��	100,0,0,0q�6-    �� Padding$ 4,4,0,0q�6@   $	��200��6R   $��30��6^   $��14(�6j   $
��Left=�6x   $	��Top=�6�    �� TextAlignment$
 Left=�6�   {�.  6�   �� 5.      $��Old Password:��6   $��0,70,0,0q�6,   $	��100��6>   $��30��6J   $��14(�6V   $
��Left=�6d   $	��Top=�6   G� 5/      -   5         G�Name$ passwordOld��5/      $��
100,70,0,0q�6/   $��4,4,0,0q�6C   $	��200��6U   $��30��6a   $��14(�6m   $
��Left=�6{   $	��Top=�6�   �� 50      $��New Password:��6   $��	0,105,0,0q�6,   $	��100��6?   $��30��6K   $��14(�6W   $
��Left=�6e   $	��Top=�6�   G� 51      -   5        $ passwordNew��51      $��100,105,0,0q�6/   $��4,4,0,0q�6D   $	��200��6V   $��30��6b   $��14(�6n   $
��Left=�6|   $	��Top=�6�   �� 52      $��Confirmation:��6   $��	0,140,0,0q�6,   $	��100��6?   $��30��6K   $��14(�6W   $
��Left=�6e   $	��Top=�6�   G� 53      -   5        $ passwordConf��53      $��100,140,0,0q�60   $��4,4,0,0q�6E   $	��200��6W   $��30��6c   $��14(�6o   $
��Left=�6}   $	��Top=�6�   54      � 55   
   $��2��6   $��1��6   .+�56      �� -   5        $��Apply��56      $��	0,2,110,2q�6=   $��Right=�6P   $��90��6l   $��26��6w   �� 57      -   5        $��Cancel��57      $��0,2,10,2q�6>   $��Right=�6P   $��90��6l   $��26��6w   58      5:      5<      