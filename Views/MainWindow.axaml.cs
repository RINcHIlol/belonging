using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;

namespace belonging.Views;

public partial class MainWindow : Window
{
//variables
    private string? _selectedShape;
    private bool _checkMode;
    private List<Shape> _shapes;
    private Shape? _currentShape;
    private TextBlock _resultText;

    public MainWindow()
    {
        InitializeComponent();

        _shapes = new List<Shape>();

        _resultText = new TextBlock
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            FontSize = 20,
            Margin = new Thickness(0, 10, 0, 0)
        };
        MainGrid.Children.Add(_resultText);

        this.Loaded += OnWindowLoaded;
    }

    private void OnWindowLoaded(object? sender, EventArgs e)
    {
        GenerateRandomShapes(3);
    }

//buttons
    private void OnRefreshShapesClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DrawingCanvas.Children.Clear();
        _shapes.Clear();
        _resultText.Text = string.Empty;

        GenerateRandomShapes(3);
    }

    private void OnClearCanvasClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DrawingCanvas.Children.Clear();
        _shapes.Clear();
        _resultText.Text = string.Empty;
    }

    private void OnDrawSquareClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _selectedShape = "Square";
        _checkMode = false;
    }

    private void OnDrawPentagonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _selectedShape = "Pentagon";
        _checkMode = false;
    }

    private void OnDrawHexagonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _selectedShape = "Hexagon";
        _checkMode = false;
    }

    private void OnCheckModeClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _checkMode = true;
    }



    private void OnCanvasClick(object? sender, PointerPressedEventArgs e)
    {
        var pointerPosition = e.GetPosition(DrawingCanvas);

        if (_checkMode)
        {
            var hitShape = _shapes.FirstOrDefault(shape => IsPointInsideShape(shape, pointerPosition));

            if (hitShape != null)
            {
                string shapeType = hitShape.Tag?.ToString() ?? "Неизвестная фигура";
                _resultText.Text = $"Попал в {shapeType}!";
                _resultText.Foreground = Brushes.Green;
                Console.WriteLine($"Попал в {shapeType}!");
            }
            else
            {
                _resultText.Text = "Не попал!";
                _resultText.Foreground = Brushes.Red;
                Console.WriteLine("Не попал!");
            }
            return;
        }

        if (_selectedShape == null) return;

        _currentShape = _selectedShape switch
        {
            "Square" => new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Blue,
                Tag = "Квадрат"
            },
            "Pentagon" => new Polygon
            {
                Points = new Avalonia.Collections.AvaloniaList<Point>
                {
                    new Point(25, 0),
                    new Point(50, 15),
                    new Point(40, 50),
                    new Point(10, 50),
                    new Point(0, 15)
                },
                Fill = Brushes.Green,
                Tag = "Пятиугольник"
            },
            "Hexagon" => new Polygon
            {
                Points = new Avalonia.Collections.AvaloniaList<Point>
                {
                    new Point(25, 0),
                    new Point(50, 13),
                    new Point(50, 38),
                    new Point(25, 50),
                    new Point(0, 38),
                    new Point(0, 13)
                },
                Fill = Brushes.Red,
                Tag = "Шестиугольник"
            },
            _ => null
        };

        if (_currentShape != null)
        {
            double shapeWidth = _currentShape is Polygon ? 50 : _currentShape.Bounds.Width;
            double shapeHeight = _currentShape is Polygon ? 50 : _currentShape.Bounds.Height;

            double x = Math.Max(0, Math.Min(pointerPosition.X - shapeWidth / 2, DrawingCanvas.Bounds.Width - shapeWidth));
            double y = Math.Max(0, Math.Min(pointerPosition.Y - shapeHeight / 2, DrawingCanvas.Bounds.Height - shapeHeight));

            Canvas.SetLeft(_currentShape, x);
            Canvas.SetTop(_currentShape, y);

            DrawingCanvas.Children.Add(_currentShape);
            _shapes.Add(_currentShape);
        }
    }

    private bool IsPointInsideShape(Shape shape, Point point)
    {
        if (shape is Rectangle rectangle)
        {
            double left = Canvas.GetLeft(rectangle);
            double top = Canvas.GetTop(rectangle);
            return point.X >= left && point.X <= left + rectangle.Width &&
                point.Y >= top && point.Y <= top + rectangle.Height;
        }

        if (shape is Polygon polygon)
        {
            var transformedPoints = polygon.Points
                .Select(p => new Point(p.X + Canvas.GetLeft(polygon), p.Y + Canvas.GetTop(polygon)))
                .ToList();

            return IsPointInsidePolygon(point, transformedPoints);
        }

        return false;
    }

    private bool IsPointInsidePolygon(Point point, IList<Point> polygonPoints)
    {
        bool isInside = false;
        int j = polygonPoints.Count - 1;

        for (int i = 0; i < polygonPoints.Count; i++)
        {
            if (polygonPoints[i].Y < point.Y && polygonPoints[j].Y >= point.Y ||
                polygonPoints[j].Y < point.Y && polygonPoints[i].Y >= point.Y)
            {
                if (polygonPoints[i].X + (point.Y - polygonPoints[i].Y) /
                    (polygonPoints[j].Y - polygonPoints[i].Y) * (polygonPoints[j].X - polygonPoints[i].X) < point.X)
                {
                    isInside = !isInside;
                }
            }
            j = i;
        }

        return isInside;
    }

    private void GenerateRandomShapes(int count)
    {
        var random = new Random();
        var shapeTypes = new[] { "Square", "Pentagon", "Hexagon" };

        for (int i = 0; i < count; i++)
        {
            string randomShape = shapeTypes[random.Next(shapeTypes.Length)];

            Shape? shape = randomShape switch
            {
                "Square" => new Rectangle
                {
                    Width = 50,
                    Height = 50,
                    Fill = Brushes.Blue,
                    Tag = "Квадрат"
                },
                "Pentagon" => new Polygon
                {
                    Points = new Avalonia.Collections.AvaloniaList<Point>
                    {
                        new Point(25, 0),
                        new Point(50, 15),
                        new Point(40, 50),
                        new Point(10, 50),
                        new Point(0, 15)
                    },
                    Fill = Brushes.Green,
                    Tag = "Пятиугольник"
                },
                "Hexagon" => new Polygon
                {
                    Points = new Avalonia.Collections.AvaloniaList<Point>
                    {
                        new Point(25, 0),
                        new Point(50, 13),
                        new Point(50, 38),
                        new Point(25, 50),
                        new Point(0, 38),
                        new Point(0, 13)
                    },
                    Fill = Brushes.Red,
                    Tag = "Шестиугольник"
                },
                _ => null
            };

            if (shape != null)
            {
                double canvasWidth = DrawingCanvas.Bounds.Width;
                double canvasHeight = DrawingCanvas.Bounds.Height;

                double x = random.NextDouble() * Math.Max(0, canvasWidth - 50);
                double y = random.NextDouble() * Math.Max(0, canvasHeight - 50);

                Canvas.SetLeft(shape, x);
                Canvas.SetTop(shape, y);

                DrawingCanvas.Children.Add(shape);
                _shapes.Add(shape);
            }
        }
    }
}