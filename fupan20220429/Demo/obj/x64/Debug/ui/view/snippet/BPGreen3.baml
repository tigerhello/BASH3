   M S B A M L   `   `   `  ����?  ;Demo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Demo.ui.view.snippetDemo.ui.view.snippet  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      #    Demo.ui.view.snippet.BPGreen3   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:Demo.ui.view.snippet     5      9system&clr-namespace:System;assembly=mscorlib     5      $
��Left=�5	      $	��Top=�5
      .��5      � ��5   
   �� 5        �� ImageSource$&   /Demo;component/images/GREEN.png��6    � RowDefinitions 5   
   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � 	Resources 5   
    �� 
TargetType �� Property �� Value   Resource.Brush.AccentColor �� Color  Resource.Brush.NoticeOrange  Resource.Brush.NoticeGreen��%)  '��       &	  �     &	 �     &	 �     �� 5      " ��6   .�5      ��  � ��$ Center=�68   �� 5       � ��$ Center=�66   �� 5       � ��
 �����6/   5       �� 5!      $ #008cee��6A   �� 5"      $ 	#FFFBC302��6B   �� 5#      $ 	#FF06F306��6A   5$      .+�5&   
   � $��0��6   $��0��6   $��Stretch=�6-   $��Stretch=�6K   .+�5(      �� $��Normal$�6   $��11(�6,   $
��Left=�6:   $��Center=�6U   $��2,0,0,0q�6p   �� 5*      
���   ��6   ��  �� RelativeSource �� 	 �� Mode$	 FindAncestor=�
 �� AncestorType#   Demo.ui.view.snippet.BPGreen3"
   �� Path$ MyProperty0�6�   5,      5.      � 50   
   $��0��6   $��1��6   $��Stretch=�6-   $��Stretch=�6K   .+�52      �� -   5        $
��Tmin��52      $��0q�6"   $��Stretch=�66   $��Stretch=�6T   $��Normal$�6p   $��11(�6�   $��Center=�6�   $��2,6,3,6q�6�   .��6�   53      55      � 57   
   $��0��6   $��2��6   $��Stretch=�6-   $��Stretch=�6K   .+�59      �� -   5        $
��Tmax��59      $��0q�6"   $��Stretch=�66   $��Stretch=�6T   $��Normal$�6p   $��11(�6�   $��Center=�6�   $��2,6,5,6q�6�   .��6�   5:      5<      5=      5>      