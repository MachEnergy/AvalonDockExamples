using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using Xceed.Wpf.Toolkit;

namespace AvalonDockExamples
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public List<MaterialToast> AllToasts = new List<MaterialToast>();

    public MainWindow()
    {
      InitializeComponent();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      using (var writer = new StreamWriter("AvalonDockSavedFile.txt"))
      {
        var layoutSerializer = new XmlLayoutSerializer(_dockingManager);
        layoutSerializer.Serialize(writer);
      }
    }

    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
      using (var reader = new StreamReader("AvalonDockSavedFile.txt"))
      {
        var layoutSerializer = new XmlLayoutSerializer(_dockingManager);
        layoutSerializer.Deserialize(reader);
      }
    }

    private void ViewNewGUI_Click(object sender, RoutedEventArgs e)
    {
      _dockingManager.Visibility = Visibility.Collapsed;
      _newDockingManager.Visibility = Visibility.Visible;
    }

    private void ViewXceed_Click(object sender, RoutedEventArgs e)
    {
      _dockingManager.Visibility = Visibility.Visible;
      _newDockingManager.Visibility = Visibility.Collapsed;
    }

    private void ToastButton_Click(object sender, RoutedEventArgs e)
    {
      MaterialToast newToast = new MaterialToast(ToastGrid);
      newToast.Content = "I am inside the Application!";
      newToast.DisplayTime = new TimeSpan(0, 0, 1);
      newToast.HideCompleted += NewToast_HideCompleted;
      newToast.IsCloseButtonVisible = true;
      newToast.HideOnClick = false;

      if (NotificationLayoutAnchor.IsHidden)
      {
        NotificationLayoutAnchor.Show();
      }
      
      AllToasts.Add(newToast);
      newToast.ShowToast();
    }

    private void NewToast_HideCompleted(object sender, RoutedEventArgs e)
    {
      AllToasts.Remove((MaterialToast)sender);
      if (AllToasts.Count == 0)
      {
        NotificationLayoutAnchor.Hide();
      }
    }
  }
}