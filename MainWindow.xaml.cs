using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using Xceed.Wpf.Toolkit;

namespace AvalonDockExamples
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged
  {
    private ObservableCollection<MaterialToast> m_AllToasts;
    public ObservableCollection<MaterialToast> AllToasts
    {
      get { return m_AllToasts; }
      set
      {
        m_AllToasts = value;
        OnPropertyChanged("AllToasts");

        AllToasts.CollectionChanged -= AllToasts_CollectionChanged;
        AllToasts.CollectionChanged += AllToasts_CollectionChanged;
      }
    }

    private void AllToasts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      OnPropertyChanged("AllToasts");
      NumToasts = AllToasts.Count.ToString();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string m_NumToasts;
    public string NumToasts
    {
      get { return m_NumToasts; }
      set { m_NumToasts = value; OnPropertyChanged("NumToasts"); }
    }

    public MainWindow()
    {
      InitializeComponent();
      AllToasts = new ObservableCollection<MaterialToast>();
      NumToasts = AllToasts.Count.ToString();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      using (var writer = new StreamWriter("AvalonDockSavedFile.txt"))
      {
        var layoutSerializer = new XmlLayoutSerializer(_newDockingManager);
        layoutSerializer.Serialize(writer);
      }
    }

    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
      using (var reader = new StreamReader("AvalonDockSavedFile.txt"))
      {
        var layoutSerializer = new XmlLayoutSerializer(_newDockingManager);
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
      //MaterialToast newToast = new MaterialToast();
      //ToastListView.Items.Add(newToast);
      MaterialToast newToast = new MaterialToast();
      //ToastItemsControl.Items.Add(newToast);
      AllToasts.Add(newToast);

      //newToast.DisplayTime = new TimeSpan(0, 0, 1);
      newToast.HideCompleted += NewToast_HideCompleted;
      newToast.IsCloseButtonVisible = true;
      newToast.HideOnClick = true;
      newToast.ToolTip = DateTime.Now.ToString();

      //newToast.Content = Resources["ToastContent"]; // [System.Windows.ControlTemplate]
      //newToast.Template = (ControlTemplate)Resources["ToastContent"]; // Replaced style, lost close button
      //newToast.Content = "This is my content."; // Embeds string inside xceed toast
      //newToast.Content = new Rectangle() { Width = 20, Height = 20, Fill = Brushes.Aqua }; // Embeds aqua rect inside xceed toast
      
      ////////// WORKS VIA XAML ////////////////////
      newToast.ContentTemplate = (DataTemplate)Resources["ToastContent"];
      //////////////////////////////////////////////////////////

      ////////// WORKS VIA FULL CODE BEHIND ////////////////////
      //StackPanel toastStack = new StackPanel();
      //toastStack.Orientation = Orientation.Vertical;
      //toastStack.Children.Add(new TextBlock() { Text = "This is my Title" });
      //toastStack.Children.Add(new TextBlock() { Text = "This is my Content" });
      //toastStack.Children.Add(new TextBlock() { Text = DateTime.Now.TimeOfDay.ToString(@"mm\:ss\.ff") });
      //newToast.Content = toastStack;
      //////////////////////////////////////////////////////////

      if (NotificationLayoutAnchor.IsHidden)
      {
        NotificationLayoutAnchor.Show();
      }
      
      newToast.ShowToast();
    }

    private void NewToast_HideCompleted(object sender, RoutedEventArgs e)
    {
      ToastItemsControl.Items.Remove(sender);
      AllToasts.Remove((MaterialToast)sender);

      if (AllToasts.Count == 0)
      {
        NotificationLayoutAnchor.Hide();
      }
    }

    private void NumToastsButton_Click(object sender, RoutedEventArgs e)
    {
      // After user has manually closed the anchor, we need to re-add it to display it
      if (NotificationLayoutAnchor.Parent == null)
      {
        NotificationLayoutAnchor.Parent = NotificationLayoutAnchorParent;
        NotificationLayoutAnchorParent.Children.Add(NotificationLayoutAnchor);
      }

      if (NotificationLayoutAnchor.IsAutoHidden)
      {
        NotificationLayoutAnchor.ToggleAutoHide();
      }

      if (!NotificationLayoutAnchor.IsVisible || NotificationLayoutAnchor.IsHidden)
      {
        NotificationLayoutAnchor.Show();
      }
    }

    private void ClearAllNotificationsButton_Click(object sender, RoutedEventArgs e)
    {
      List<MaterialToast> toastsToRemove = AllToasts.ToList();
      foreach (MaterialToast toast in toastsToRemove)
      {
        toast.HideToast();
      }
    }

    private void NotificationLayoutAnchor_IsVisibleChanged(object sender, EventArgs e)
    {
      //if (NotificationLayoutAnchor.IsVisible)
      //{
      //  if (AllToasts != null)
      //  {
      //    foreach (MaterialToast toast in AllToasts)
      //    {
      //      if (toast?.IsVisible == false)
      //      {
      //        toast?.ShowToast();
      //      }
      //    }
      //  }
      //}
    }
  }
}