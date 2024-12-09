using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System.Threading.Tasks;

namespace belonging.Views;

public partial class MainWindow : Window
{
    private TextBlock? _messageText;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnDrawSquareClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DrawingCanvas.Children.Clear();

        var square = new Rectangle
        {
            Width = 100,
            Height = 100,
            Fill = Brushes.Blue
        };

        Canvas.SetLeft(square, (DrawingCanvas.Bounds.Width - square.Width) / 2);
        Canvas.SetTop(square, (DrawingCanvas.Bounds.Height - square.Height) / 2);

        DrawingCanvas.Children.Add(square);
    }

    private void OnDrawPentagonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DrawingCanvas.Children.Clear();

        var pentagon = new Polygon
        {
            Points = new Avalonia.Collections.AvaloniaList<Point>
            {
                new Point(50, 0),   // Верхняя вершина
                new Point(100, 38), // Правая верхняя
                new Point(81, 100), // Правая нижняя
                new Point(19, 100), // Левая нижняя
                new Point(0, 38)    // Левая верхняя
            },
            Fill = Brushes.Green
        };

        Canvas.SetLeft(pentagon, (DrawingCanvas.Bounds.Width - 100) / 2);
        Canvas.SetTop(pentagon, (DrawingCanvas.Bounds.Height - 100) / 2);

        DrawingCanvas.Children.Add(pentagon);
    }

    private void OnDrawHexagonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DrawingCanvas.Children.Clear();

        var hexagon = new Polygon
        {
            Points = new Avalonia.Collections.AvaloniaList<Point>
            {
                new Point(50, 0),   // Верхняя центральная
                new Point(100, 25), // Правая верхняя
                new Point(100, 75), // Правая нижняя
                new Point(50, 100), // Нижняя центральная
                new Point(0, 75),   // Левая нижняя
                new Point(0, 25)    // Левая верхняя
            },
            Fill = Brushes.Red
        };

        Canvas.SetLeft(hexagon, (DrawingCanvas.Bounds.Width - 100) / 2);
        Canvas.SetTop(hexagon, (DrawingCanvas.Bounds.Height - 100) / 2);

        DrawingCanvas.Children.Add(hexagon);
    }

    private async void OnCanvasClick(object? sender, PointerPressedEventArgs e)
    {
        if (_messageText != null)
        {
            DrawingCanvas.Children.Remove(_messageText);
        }

        _messageText = new TextBlock
        {
            Text = "ТЫ ПОПАЛ!!",
            FontSize = 40,
            Foreground = Brushes.Green,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            TextAlignment = Avalonia.Media.TextAlignment.Center
        };

        Canvas.SetLeft(_messageText, (DrawingCanvas.Bounds.Width - 150) / 2);
        Canvas.SetTop(_messageText, 10);

        DrawingCanvas.Children.Add(_messageText);

        await Task.Delay(2000);
        if (_messageText != null)
        {
            DrawingCanvas.Children.Remove(_messageText);
            _messageText = null;
        }
    }
}