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
    private string? _selectedShape;
    private bool _checkMode;
    private Shape? _currentShape;
    private TextBlock _resultText;

    public MainWindow()
    {
        InitializeComponent();

        _resultText = new TextBlock
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            FontSize = 20,
            Margin = new Thickness(0, 10, 0, 0)
        };
        MainGrid.Children.Add(_resultText);
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
            if (_currentShape != null && IsPointInsideShape(_currentShape, pointerPosition))
            {
                _resultText.Text = "Попал!";
                _resultText.Foreground = Brushes.Green;
                Console.WriteLine("Попал!");
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

        DrawingCanvas.Children.Clear();

        _currentShape = _selectedShape switch
        {
            "Square" => new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Blue
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
                Fill = Brushes.Green
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
                Fill = Brushes.Red
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
}

// double x = Math.Max(0, Math.Min(pointerPosition.X - shapeWidth / 2, DrawingCanvas.Bounds.Width - shapeWidth)); 
// double y = Math.Max(0, Math.Min(pointerPosition.Y - shapeHeight / 2, DrawingCanvas.Bounds.Height - shapeHeight));