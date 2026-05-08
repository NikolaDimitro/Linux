using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Threading;

namespace LinuxMintOS
{
    public partial class MainWindow : Window
    {
        private sealed class InternalAppWindow
        {
            public string Key { get; set; }
            public Border Shell { get; set; }
            public Border Body { get; set; }
            public bool IsMinimized { get; set; }
            public Rect RestoreBounds { get; set; }
            public double RestoreWidth { get; set; }
            public double RestoreHeight { get; set; }
            public double RestoreLeft { get; set; }
            public double RestoreTop { get; set; }
        }

        private readonly Dictionary<string, InternalAppWindow> internalWindows = new Dictionary<string, InternalAppWindow>();
        private int internalWindowZ = 100;
        private DispatcherTimer clockTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeClock();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var assetsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
            var fallbackIconPath = Path.Combine(assetsDir, "browser.png");
            foreach (var button in DesktopAppsPanel.Children.OfType<Button>())
            {
                var image = (button.Content as StackPanel)?.Children.OfType<Image>().FirstOrDefault();
                var sourceText = image?.Source?.ToString() ?? string.Empty;
                if (image != null && string.IsNullOrWhiteSpace(sourceText))
                {
                    if (File.Exists(fallbackIconPath))
                    {
                        image.Source = new BitmapImage(new Uri(fallbackIconPath, UriKind.Absolute));
                    }
                }
            }

            // #region agent log
            DebugLog(
                "baseline-inapp-windows",
                "H1",
                "MainWindow.xaml.cs:MainWindow_Loaded",
                "Main desktop shell loaded",
                $"{{\"openWindowCount\":{Application.Current.Windows.Count},\"menuPanelVisibility\":\"{EscapeForJson(MintMenuPanel.Visibility.ToString())}\"}}");
            // #endregion
        }

        private static void DebugLog(string runId, string hypothesisId, string location, string message, string data)
        {
            var payload = $"{{\"sessionId\":\"96355c\",\"runId\":\"{EscapeForJson(runId)}\",\"hypothesisId\":\"{EscapeForJson(hypothesisId)}\",\"location\":\"{EscapeForJson(location)}\",\"message\":\"{EscapeForJson(message)}\",\"data\":{data},\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}";
            try
            {
                File.AppendAllText(@"c:\Users\Acer\Desktop\Linux os based on vusial studio\debug-96355c.log", payload + Environment.NewLine);
            }
            catch
            {
                // intentionally ignore logging failure
            }
        }

        private static string EscapeForJson(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private void InitializeClock()
        {
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += (s, e) => ClockDisplay.Text = DateTime.Now.ToString("HH:mm");
            clockTimer.Start();
        }

        private void FileManager_Click(object sender, RoutedEventArgs e)
        {
            LaunchInternalApp("files", "Files", () => new FileManagerWindow(), "H2");
        }

        private void Browser_Click(object sender, RoutedEventArgs e)
        {
            LaunchInternalApp("browser", "Browser", () => new BrowserWindow(), "H3");
        }

        private void Terminal_Click(object sender, RoutedEventArgs e)
        {
            LaunchInternalApp("terminal", "Terminal", () => new TerminalWindow(), "H4");
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            LaunchInternalApp("settings", "Settings", () => new SettingsWindow(), "H5");
        }

        private void TextEditor_Click(object sender, RoutedEventArgs e)
        {
            LaunchInternalApp("text-editor", "Text Editor", () => new TextEditorWindow(), "H6");
        }

        private void LaunchInternalApp(string key, string title, Func<Window> factory, string hypothesisId)
        {
            if (internalWindows.TryGetValue(key, out var existing))
            {
                if (existing.IsMinimized)
                {
                    existing.Shell.Width = existing.RestoreWidth;
                    existing.Shell.Height = existing.RestoreHeight;
                    Canvas.SetLeft(existing.Shell, existing.RestoreLeft);
                    Canvas.SetTop(existing.Shell, existing.RestoreTop);
                    existing.Shell.Visibility = Visibility.Visible;
                    existing.IsMinimized = false;
                    UpdateTaskbarButton(key, title, false);
                }
                BringToFront(existing);
                // #region agent log
                DebugLog(
                    "post-fix-inapp-windows",
                    hypothesisId,
                    "MainWindow.xaml.cs:LaunchInternalApp",
                    "Focused existing internal app window",
                    $"{{\"appKey\":\"{EscapeForJson(key)}\",\"inAppWindowCount\":{internalWindows.Count},\"desktopLayerChildren\":{DesktopWindowLayer.Children.Count},\"isMinimized\":{existing.IsMinimized.ToString().ToLowerInvariant()},\"topLevelWindowCount\":{Application.Current.Windows.Count}}}");
                // #endregion
                return;
            }

            Window appWindow = null;
            UIElement content = null;
            try
            {
                appWindow = factory();
                content = appWindow.Content as UIElement;
                if (content != null)
                    appWindow.Content = null;
                appWindow.Close();
            }
            catch (Exception ex)
            {
                // #region agent log
                DebugLog(
                    "post-fix-inapp-windows",
                    hypothesisId,
                    "MainWindow.xaml.cs:LaunchInternalApp",
                    "Failed creating internal app content",
                    $"{{\"appKey\":\"{EscapeForJson(key)}\",\"error\":\"{EscapeForJson(ex.Message)}\"}}");
                // #endregion
                return;
            }

            if (content == null)
            {
                // #region agent log
                DebugLog(
                    "post-fix-inapp-windows",
                    hypothesisId,
                    "MainWindow.xaml.cs:LaunchInternalApp",
                    "No app content available for internal host",
                    $"{{\"appKey\":\"{EscapeForJson(key)}\"}}");
                // #endregion
                return;
            }

            var appState = BuildInternalWindow(key, title, appWindow.Width, appWindow.Height, content);
            internalWindows[key] = appState;
            DesktopWindowLayer.Children.Add(appState.Shell);
            PlaceInternalWindow(appState.Shell, internalWindows.Count);
            BringToFront(appState);
            AddTaskbarButton(key, title);

            // #region agent log
            DebugLog(
                "post-fix-inapp-windows",
                hypothesisId,
                "MainWindow.xaml.cs:LaunchInternalApp",
                "Opened new in-app window",
                $"{{\"appKey\":\"{EscapeForJson(key)}\",\"inAppWindowCount\":{internalWindows.Count},\"desktopLayerChildren\":{DesktopWindowLayer.Children.Count},\"topLevelWindowCount\":{Application.Current.Windows.Count}}}");
            // #endregion
        }

        private InternalAppWindow BuildInternalWindow(string key, string title, double initialWidth, double initialHeight, UIElement content)
        {
            var root = new Grid();
            root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var header = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(230, 36, 44, 56)),
                Height = 34,
                BorderBrush = new SolidColorBrush(Color.FromArgb(90, 255, 255, 255)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Cursor = Cursors.SizeAll
            };
            Grid.SetRow(header, 0);

            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            header.Child = headerGrid;

            var titleText = new TextBlock
            {
                Text = title,
                Foreground = Brushes.White,
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };
            headerGrid.Children.Add(titleText);

            var controls = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 6, 0)
            };
            Grid.SetColumn(controls, 1);
            headerGrid.Children.Add(controls);

            var minimizeButton = new Button { Content = "—", Width = 26, Height = 22, Margin = new Thickness(2, 0, 2, 0) };
            var maximizeButton = new Button { Content = "□", Width = 26, Height = 22, Margin = new Thickness(2, 0, 2, 0) };
            var closeButton = new Button { Content = "×", Width = 26, Height = 22, Margin = new Thickness(2, 0, 2, 0), Background = new SolidColorBrush(Color.FromArgb(180, 160, 42, 42)) };
            controls.Children.Add(minimizeButton);
            controls.Children.Add(maximizeButton);
            controls.Children.Add(closeButton);

            var body = new Border
            {
                Background = Brushes.White,
                Child = content
            };
            Grid.SetRow(body, 1);

            root.Children.Add(header);
            root.Children.Add(body);

            var shell = new Border
            {
                Width = Math.Max(initialWidth, 640),
                Height = Math.Max(initialHeight, 420),
                BorderBrush = new SolidColorBrush(Color.FromArgb(140, 255, 255, 255)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Background = new SolidColorBrush(Color.FromArgb(220, 20, 28, 38)),
                Child = root
            };

            var state = new InternalAppWindow
            {
                Key = key,
                Shell = shell,
                Body = body,
                IsMinimized = false
            };

            shell.MouseLeftButtonDown += (s, e) => BringToFront(state);

            bool dragging = false;
            Point dragOffset = new Point();
            header.MouseLeftButtonDown += (s, e) =>
            {
                BringToFront(state);
                dragging = true;
                dragOffset = e.GetPosition(shell);
                header.CaptureMouse();
            };
            header.MouseMove += (s, e) =>
            {
                if (!dragging)
                    return;
                var position = e.GetPosition(DesktopWindowLayer);
                Canvas.SetLeft(shell, position.X - dragOffset.X);
                Canvas.SetTop(shell, position.Y - dragOffset.Y);
            };
            header.MouseLeftButtonUp += (s, e) =>
            {
                dragging = false;
                header.ReleaseMouseCapture();
            };
            header.MouseLeave += (s, e) =>
            {
                if (!header.IsMouseCaptured)
                    dragging = false;
            };

            minimizeButton.Click += (s, e) =>
            {
                state.RestoreWidth = state.Shell.Width;
                state.RestoreHeight = state.Shell.Height;
                state.RestoreLeft = Canvas.GetLeft(state.Shell);
                state.RestoreTop = Canvas.GetTop(state.Shell);
                
                state.Shell.Visibility = Visibility.Collapsed;
                state.IsMinimized = true;
                UpdateTaskbarButton(state.Key, title, true);
                // #region agent log
                DebugLog(
                    "post-fix-inapp-windows",
                    "H7",
                    "MainWindow.xaml.cs:BuildInternalWindow",
                    "Internal window minimized",
                    $"{{\"appKey\":\"{EscapeForJson(state.Key)}\",\"isMinimized\":true}}");
                // #endregion
            };

            maximizeButton.Click += (s, e) =>
            {
                if (state.IsMinimized)
                {
                    state.Body.Visibility = Visibility.Visible;
                    state.IsMinimized = false;
                    state.Shell.Height = Math.Max(initialHeight, 420);
                }

                if (state.RestoreBounds.Width <= 0)
                {
                    state.RestoreBounds = new Rect(
                        Canvas.GetLeft(state.Shell),
                        Canvas.GetTop(state.Shell),
                        state.Shell.Width,
                        state.Shell.Height);
                    Canvas.SetLeft(state.Shell, 0);
                    Canvas.SetTop(state.Shell, 0);
                    state.Shell.Width = DesktopWindowLayer.ActualWidth;
                    state.Shell.Height = DesktopWindowLayer.ActualHeight;
                }
                else
                {
                    Canvas.SetLeft(state.Shell, state.RestoreBounds.Left);
                    Canvas.SetTop(state.Shell, state.RestoreBounds.Top);
                    state.Shell.Width = state.RestoreBounds.Width;
                    state.Shell.Height = state.RestoreBounds.Height;
                    state.RestoreBounds = Rect.Empty;
                }
            };

            closeButton.Click += (s, e) => CloseInternalWindow(state.Key);

            return state;
        }

        private void CloseInternalWindow(string key)
        {
            if (!internalWindows.TryGetValue(key, out var state))
                return;

            DesktopWindowLayer.Children.Remove(state.Shell);
            internalWindows.Remove(key);
            RemoveTaskbarButton(key);
            // #region agent log
            DebugLog(
                "post-fix-inapp-windows",
                "H8",
                "MainWindow.xaml.cs:CloseInternalWindow",
                "Internal window closed",
                $"{{\"appKey\":\"{EscapeForJson(key)}\",\"inAppWindowCount\":{internalWindows.Count},\"topLevelWindowCount\":{Application.Current.Windows.Count}}}");
            // #endregion
        }

        private void BringToFront(InternalAppWindow state)
        {
            Panel.SetZIndex(state.Shell, ++internalWindowZ);
        }

        private void PlaceInternalWindow(Border shell, int index)
        {
            var left = 140 + ((index - 1) % 3) * 36;
            var top = 90 + ((index - 1) % 3) * 28;
            Canvas.SetLeft(shell, left);
            Canvas.SetTop(shell, top);
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MintMenuPanel.Visibility = MintMenuPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MintMenuPanel.Visibility != Visibility.Visible)
                return;

            var source = e.OriginalSource as DependencyObject;
            while (source != null)
            {
                if (ReferenceEquals(source, MintMenuPanel) || ReferenceEquals(source, MenuButton))
                    return;
                source = VisualTreeHelper.GetParent(source);
            }

            MintMenuPanel.Visibility = Visibility.Collapsed;
        }

        private void AddTaskbarButton(string key, string title)
        {
            var button = new Button
            {
                Content = title,
                Background = new SolidColorBrush(Color.FromArgb(170, 51, 51, 51)),
                Foreground = Brushes.White,
                Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(5, 0, 5, 0),
                Cursor = Cursors.Hand,
                Tag = key
            };
            
            button.Click += TaskbarButton_Click;
            button.MouseEnter += (s, e) => button.Background = new SolidColorBrush(Color.FromArgb(200, 68, 68, 68));
            button.MouseLeave += (s, e) => button.Background = new SolidColorBrush(Color.FromArgb(170, 51, 51, 51));
            
            TaskbarItems.Items.Add(button);
        }

        private void UpdateTaskbarButton(string key, string title, bool isMinimized)
        {
            foreach (Button button in TaskbarItems.Items)
            {
                if (button.Tag?.ToString() == key)
                {
                    if (isMinimized)
                    {
                        button.Background = new SolidColorBrush(Color.FromArgb(180, 80, 80, 80));
                        button.FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        button.Background = new SolidColorBrush(Color.FromArgb(170, 51, 51, 51));
                        button.FontWeight = FontWeights.Normal;
                    }
                    break;
                }
            }
        }

        private void RemoveTaskbarButton(string key)
        {
            for (int i = TaskbarItems.Items.Count - 1; i >= 0; i--)
            {
                if (TaskbarItems.Items[i] is Button button && button.Tag?.ToString() == key)
                {
                    TaskbarItems.Items.RemoveAt(i);
                    break;
                }
            }
        }

        private void TaskbarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag?.ToString() is string key)
            {
                if (internalWindows.TryGetValue(key, out var state))
                {
                    if (state.IsMinimized)
                    {
                        state.Shell.Width = state.RestoreWidth;
                        state.Shell.Height = state.RestoreHeight;
                        Canvas.SetLeft(state.Shell, state.RestoreLeft);
                        Canvas.SetTop(state.Shell, state.RestoreTop);
                        state.Shell.Visibility = Visibility.Visible;
                        state.IsMinimized = false;
                        UpdateTaskbarButton(key, button.Content.ToString(), false);
                    }
                    BringToFront(state);
                }
            }
        }
    }
}