   M S B A M L   `   `   `  ����G  CSFTD_project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:SFTD_projectSFTD_project  5   	   #    SFTD_project.TimeFilterWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:SFTD_project     5   	     � Title$  Window1��5   	   $��173.333��6   $	��300��6*   .��5	      � .+�5
   
   �� -   5         ��Name$ label��5
      $��Display articles ��6   $
��Left=�6;   $��	10,10,0,0q�6V   $	��Top=�6i   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ DateA��5      $��
10,41,10,0q�6$   $	��Top=�68   {�.  6P     5   
   -   5        $ DateB��5      $��
10,70,10,0q�6$   $	��Top=�68   {�.  6P   �� 5   
   -   5         ��Name$ comboBox��5      $��107,10,10,0q�6%   $	��Top=�6:   $��26��6R    �� SelectedIndex$ 0��5      .@�5      �� $��from all time��6   �� 5      $��on��6   �� 5      $��
newer than��6   �� 5      $��
older than��6   �� 5      $��between��6   5      �� 5   
   -   5         ��Name$ 	CloseTime��5      $��Save Changes��6$   $��	0,0,10,10q�6;   $��20��6N   $��Bottom=�6Z   $��Right=�6u   $	��105��6�   6�   5      5      