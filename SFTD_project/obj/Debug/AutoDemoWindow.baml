   M S B A M L   `   `   `  ����G  CSFTD_project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:SFTD_projectSFTD_project  5   	   !    SFTD_project.AutoDemoWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:SFTD_project     5   	     � Title$  Automatic Demo��5   	   $	��300��6    $	��378��6-    � 
ResizeMode$ NoResize=�69   .��5	      � $��10q�6    J� RenderTransformOrigin$ -1.1,-0.492&�6    � ColumnDefinitions 5
   
   �� 5      �� 5      .+�5   
   �� -   5         ��Name$ SubscribeToFeedButton��5      $��Subscribe To Feed��60   $��29��6L   $��	10,10,0,0q�6X   $	��117��6k   $ 0.513,2.267&�6w   $	��Top=�6�   $��Center=�6�   6�   �� 5   
   -   5        $ FullDemoButton��5      $��29��6)   $��
10,213,0,0q�65   $	��117��6I   $ 0.513,2.267&�6U   $	��Top=�6y   $��Center=�6�   $��Run Full Demo��6�   ���	#FF92E4856�   �� 5   
   -   5        $ ManageFeedsButton��5      $��Manage Channels��6,   $��29��6F   $��
10,160,0,0q�6R   $	��117��6f   $ 0.513,2.267&�6r   $	��Top=�6�   $��Center=�6�   6�   �� 5   
   -   5        $ ShowMapInterfaceButton��5      $��29��61   $��
10,110,0,0q�6=   $	��117��6Q   $ 0.513,2.267&�6]   $	��Top=�6�   $��Center=�6�   $��Show Map Interface��6�   6�   �� 5   
   -   5        $ ViewFavoritesButton��5      $��View Favorites��6.   $��29��6G   $��	10,61,0,0q�6S   $	��117��6f   $ 0.513,2.267&�6r   $	��Top=�6�   $��Center=�6�   6�   �� 5   
   -   5         ��Name$ InstructionsBox��5      $��1��6+   $
��Left=�6;   $	��198��6V   $��	10,44,0,0q�6c    �� TextWrapping$
 Wrap=�6v   $	��Top=�6�   $	��156��6�   
���   � �� 	UndoLimit$ 1��6�    �� 
IsReadOnly . 6�   �� 5   
   $��Instructions��6   $��1��6'   $
��Left=�67   $��29��6R   $��	10,10,0,0q�6^   $	��Top=�6q   $	��156��6�   ���	#FFF1F1F1$��2q�6�   ���	#FFC8FAFF6�   5      5      