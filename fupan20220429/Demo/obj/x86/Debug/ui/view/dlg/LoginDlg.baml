   M S B A M L   `   `   `  ����?  ;Demo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Demo.ui.view.dialogDemo.ui.view.dialog  5   	   "    Demo.ui.view.dialog.LoginDlg   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:Demo.ui.view.dialog     5   	     � Title$  Login��5   	   $	��210��6   $	��400��6$    � 	Resources 5	         
MyCommand1 �� Text  
MyCommand2��%X   &	        &	      �� 5
   
   $
 Text��6-   �� 5   
   $ Another Text��6-   5       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command# [  5      5        5   
   -   5        # [ 5      5       J� InputBindings 5      �� 5   
   
 �� Key$ Return��6    �� Command# [  6"   �� 5   
   $ A��6    �� 	Modifiers$	 Altf�6   # [  6-   �� 5   
   $	 Esc��6   # [ 6   �� 5   
   $ C��6   $	 Altf�6   # [ 6-   .��5      � 	 � ColumnDefinitions	 5   
   �� 5      $��1* �6   �� 5      $	��300 �6   �� 5      $��1* �6   
 � RowDefinitions
 5!   
   �� 5"      $��1* �6   �� 5#      $	��100 �6   �� 5$      $��30 �6   �� 5%      $��1* �6   .+�5'   
   � $��1��6   $��1��6   .+�5(      �� $��User ID:��6   $��0,0,0,0q�6'   $	��100��68   $��30��6D   $��14(�6P   $
��Left=�6^   $	��Top=�6y   �� 5)      -   5         ��Name$ userNameBox��5)      $��Administrator��6+   $��	100,0,0,0q�6@   $��5,4,0,0q�6S   $	��200��6e   $��30��6q   $��14(�6}   $
��Left=�6�   $	��Top=�6�   {�.  6�   �� 5+      $��	Password:��6   $��0,35,0,0q�6(   $	��100��6:   $��30��6F   $��14(�6R   $
��Left=�6`   $	��Top=�6{   G� 5,      -   5         G�Name$ passwordBox��5,       G� 	MaxLength$ 8��6/   $��
100,35,0,0q�6=   $��5,4,0,0q�6Q   $	��200��6c   $��30��6o   $��14(�6{   $
��Left=�6�   $	��Top=�6�   5-      � 5.   
   $��2��6   $��1��6   .+�5/      �� -   5        $��Apply��5/      $��	0,2,110,2q�6=   $��Right=�6P   $��90��6l   $��26��6w   �� 50      -   5        $��Cancel��50      $��0,2,10,2q�6>   $��Right=�6P   $��90��6l   $��26��6w   51      53      55      