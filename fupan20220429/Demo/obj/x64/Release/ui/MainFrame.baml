   M S B A M L   `   `   `  ���� ?  ;Demo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:Demo.uiDemo.ui  5clr-namespace:Demo.ui.view.cardDemo.ui.view.card  ;"clr-namespace:Demo.ui.view.snippetDemo.ui.view.snippet  ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tOclr-namespace:System.Windows.Forms.Integration;assembly=WindowsFormsIntegration System.Windows.Forms.Integration [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms     Demo.ui.MainFrame   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:Demo.ui     2gif"http://wpfanimatedgif.codeplex.com     0cardclr-namespace:Demo.ui.view.card     6snippet"clr-namespace:Demo.ui.view.snippet     _wfiOclr-namespace:System.Windows.Forms.Integration;assembly=WindowsFormsIntegration     Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     -     � Title$  BOHR-820  V4.0.0.0�� � Icon$) #/Demo;component/images/favicon1.ico��$
��1000��$
��1500�� � WindowState$ Normal=� J� OpacityMask
 �����
������  � 	Resources    WindowsGrid �� 
TargetType �� Property �� Value��%3   &	        �� " �.���  � �� �#EFEFEF.���� ���#CCCCCC$��1q�$	��0,0q�$��Stretch=�$��Stretch=�
�������._�� $��	0,-0,-0,0q�
������� � RowDefinitions �� $	��85* ��� $	��20* ��� $
��864* �	 � ColumnDefinitions	 �� $
��0.7* ��� $��3* ��� $��* �.+�� $��0��$��0��$��1��$��Stretch=�$��Stretch=�$��0,0,0.4,19.471q�$��2��	 �� $��* ��� $	��10* ��� $��2* ��� $	��10* ��� $��2* ��� $	��10* ��� $��2* ��� $	��10* �.+��� -   
 ��Name$
 Button_Setting��$��0��$��设置��$��Stretch=�$��Stretch=�$��1��{�.  �� -   $
 Button_Fupan��$��0��$��3��$��1��$��复判��$��Stretch=�$��Stretch=��� -   $
 Button_Check��$��0��$��4��$��3��$��	点检项��$
��Left=�$��Stretch=�$��9.2,0,0,-0.2q�$��48���� -   $
 Button_Connect��$��0��$��7��$��1��$��连接��$��Stretch=�$��Stretch=�� $��0��$��1��$��2��$��Stretch=�$��Stretch=�$��951.6,0,0.2,19.471q�$��2��	 �� $��* ��� $	��10* ��� $��* ��� $	��10* ��� $��* ��� $	��10* �.+��� -   $
 Button_OpenPath��$��0��$��5��$��1��$��打开路径��$��Stretch=�$��Stretch=�$��6.2,14,9.6,15.8q� J� RenderTransformOrigin$ 0.502,0.502&��� -   $	��TBc��$��1��$��0��$��3��$��2��$��Stretch=�$��Stretch=�$��0,0.2,0.2,0q�.@��� $��Status��$	��100��$��0��$��14(�$��0��.��� #��[  $��	0,-0,-0,0q�
������� �� $��0.001* ��� $��5* ��� $
��0.2* �	 �� $
��0.7* ��� $��3* ��� $��* �.+�� -   $��产品列表��$��1��$��0��$��1��$��Stretch=�$��Stretch=� �� $��* ��� $��* �.+��� $��0��$��0��$��1��$��产品列表：��$��Stretch=�$��Stretch=�.���� -	   $��	ProductLi���� $��1��$��0��$��1��$��	筛选：��$��Stretch=�$��Stretch=�.��� $��1��$��1��$��1��$��Stretch=�$��Stretch=� �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �	 �� $��* ��� $��* �.+��� -
   $��Today��$��当天��$��0��$��0��$��2��$��0,0,99.8,0.2q��� -   $
��La15��$��最近15分钟��$��0��$��1���� -   $��Timee��$��时间��$��1��$��0��7  1System.Windows.Forms.Integration.WindowsFormsHost  $��1��$��0��$��2��$��0,18.8,-0.2,0.4q�   Child. )  #System.Windows.Forms.DateTimePicker  -   	TimeU��   Format$ Custom=�   CustomFormat$ yyyy-MM-dd HH:mm:ss��   
ShowUpDown .   $��2��$��0��$��2��$��0,9.6,-0.2,0.6q�.   -   	TimeD��$ Custom=�$ yyyy-MM-dd HH:mm:ss�� . �� -   $��SNCode��$��条码��$��3��$��0���� -   $��PCODE��$��3��$��0��$��Stretch=�$��Stretch=�$��2��$��0,24.4,-0.2,66.2q�$��2���� -   $	
 Inq��$��4��$��0��$��1��$��查询��$��Stretch=�$��Stretch=�$��10,13.2,16.8,10.2q��� -   $
 WriteTo��$��4��$��1��$��1��$��导出��$��Stretch=�$��Stretch=�$��10.2,13.2,16.8,10.2q��� -   $
 BatchOut��$��5��$��1��$��1��$��批量导出图片��$��Stretch=�$��Stretch=�$��0.2,9.8,9.8,13.4q�� $��1��$��2��$��1��$��Stretch=�$��Stretch=� �� $
��1.5* ��� $
��0.8* ��� $
��3.1* �.+��� $��0��$��0��$��1��$��MES上传结果：��$��Stretch=�$��Stretch=�.���  �� $��3* ��� $��* �.+��� -   $��MesRes��$��0��$��0��$��1��� $��1��$��0��$��1��$��Stretch=�$��Stretch=�	 �� $��* ��� $��* ��� $
��0.1* ��� $
��1.2* ��� $
��0.1* ��� $��* �.+��� -    ��Name$ WorkerNo��$��工号:��$��0��$��0��$��1��$��Stretch=�$��Stretch=��� -   $��WorkO��$��0��$��0��$��2��$��1��$��10336383��$��Stretch=�$��Stretch=�$��0,29,0.2,0.4q��� -   $
 MESIN��$��0��$��3��$��1��$��	MES登录��$��Stretch=�$��Stretch=�$��
0,29,0,0.4q��� -   $��MESSHOW��$��0��$��5��$��1��$��Stretch=�$��Stretch=�$��0.2,29,0,0.4q��� $��1��$��0��$��1��$��复判结果：��$��Stretch=�$��Stretch=�.��� -   $��	FailPass1��$��1��$��0��$��1��$��Stretch=�$��Stretch=�	 �� $��2* ��� $
��0.1* ��� $��* �.+��� -   $
 
Button_OK1��$��1��$��0��$��1��$
��PASS��$��Stretch=�$��Stretch=��� -   $
 
Button_NG1��$��1��$��2��$��1��$
��FAIL��$��Stretch=�$��Stretch=��� $��2��$��0��$��1��$��缺陷导航图：��$��Stretch=�$��Stretch=�.���  �� $
��0.5* ��� $	��16* ��� $��1* �	 �� $��* ��� $��* �.+��� -   $��ImgeIS��$��是否显示OK图片��$��0��$��0��$��1��$��Stretch=�$��Stretch=��� -   $��MESIS��$��禁止自动MES��$��0��$��1��$��1��$��Stretch=�$��Stretch=�    Demo.ui.view.card.DaoHangV  -     Name$ 	Navigator��$��0��$��2��$��Stretch=�$��Stretch=�$��1���� -   $ Status1��$��C:/USER/Bohr��$��2��$��0��$��1��$��Stretch=�$��Stretch=��� -    $
 Button_Checks��$��2��$��1��$��1��$��导出当前图片��$��Stretch=�$��Stretch=�� $��2��$��0��$��3��$��Stretch=�$��Stretch=�.+��� -!   $ Status��$�� ��$��Stretch=�$��Stretch=�� $��1��$��0��$��1��$��Stretch=�$��Stretch=� �� $	��10* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��1��$��1��$��1��$��	结果图��$��Stretch=�$��Stretch=�.���  �� $��* ��� $	��10* ��� $��3* �.+�� $��0��$��0��$��1��$��Stretch=�$��Stretch=�	 �� $
��0.2* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� -"   $
 SN_L��$��0��$��0��$��1��$	��SN:��$��Right=�$��Stretch=��� -#   $
��SNin��$��0��$��1��$��Stretch=�$��Stretch=�$��10.2,0,0.2,19.2q�$��1��.���� -$   $ Model��$��0��$��2��$��1��$��Model:��$��Right=�$��Stretch=��� -%   $��TextBox1��$��0��$��3��$��1��$
��B820��$��Stretch=�$��Stretch=�$��2��$��0.4,0,175.8,19q��� -&   $ Result��$��Result:��$��0��$��4��$��1��$��Right=�$��Stretch=��� -'   $
��RESA��$��0��$��5��$��1��$��Stretch=�$��Stretch=�$��
0.2,0,0,19q�!   Demo.ui.view.card.ZoomImage  -(     Name$ 	mainPanel��$��0��$��1��}�. $��Stretch=�$��Stretch=����#fcdfde$��0,59,0.2,10.4q�$��3��� $��1��$��2��$��1��$��Stretch=�$��Stretch=� �� $	��10* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��Property��$	��100��$��0��$��14(�$��0��{�. .���  �� �� $	��200 �	 �� .+�� -)    �Name$ pageContent��$��0��$��Stretch=�$��Stretch=�$��0,0.4,0,0.6q�.+�,   &Demo.ui.view.card.TubeAlarmHistoryCard  -*     Name$ RecordTable��$��0,0,0,0q�� $��1��$��Stretch=�$��Stretch=�$��0,0.4,0,0.6q� �� $��* ��� $��* ��� $
��0.3* ��� $��* ��� $��* �	 �� $
��0.1* ��� $
��0.2* ��� $
��0.5* ��� $
��0.1* ��� $
��0.2* ��� $
��0.5* ��� $
��0.1* ��� $
��0.3* ��� $
��0.1* �.+��� $!��操作员签名(签全名):��$��1��$��1��$��Stretch=�$��Stretch=��� -+   $��OPName��$��1��$��2��$��Stretch=�$��Stretch=�$��23(��� $��ME签名(签全名):��$��3��$��1��$��Stretch=�$��Stretch=��� -,   $��MEName��$��3��$��2��$��Stretch=�$��Stretch=�$��23(��� $��确认人(签全名):��$��1��$��4��$��Stretch=�$��Stretch=��� --   $��OPName2��$��1��$��5��$��Stretch=�$��Stretch=�$��23(��� $��确认人(签全名):��$��3��$��4��$��Stretch=�$��Stretch=��� -.   $��MEName2��$��3��$��5��$��Stretch=�$��Stretch=�$��23(��� -/   $��保存��$��3��$��7��$��Stretch=�$��Stretch=�