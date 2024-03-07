using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Manager;

/// <summary>
/// Interaction logic for GanntWindow.xaml
/// </summary>
public partial class GanntWindow : Window
{
    public GanntWindow()
    {
        InitializeComponent();
        //// יצירת מערך גאנט עם משימות ותלות ביניהן
        //var ganttChart = new GanttChart();
        //var taskA = new GanttTask("Task A", "1", DateTime.Today, TimeSpan.FromDays(3));
        //var taskB = new GanttTask("Task B", "2", DateTime.Today.AddDays(2), TimeSpan.FromDays(2));
        //var taskC = new GanttTask("Task C", "3", DateTime.Today.AddDays(4), TimeSpan.FromDays(2));
        //var taskD = new GanttTask("Task D", "4", DateTime.Today.AddDays(6), TimeSpan.FromDays(1));
        //var taskE = new GanttTask("Task E", "5", DateTime.Today.AddDays(7), TimeSpan.FromDays(3));

        //// הוספת משימות לתרשים גאנט
        //ganttChart.AddTask(taskA);
        //ganttChart.AddTask(taskB);
        //ganttChart.AddTask(taskC);
        //ganttChart.AddTask(taskD);
        //ganttChart.AddTask(taskE);

        //// הוספת תרשים גאנט לחלון
        //Content = ganttChart;

        //// קביעת צבעים למשימות בהתאם למצבן
        //ganttChart.SetTaskColor(taskA, Brushes.Green); // משימה שהושלמה
        //ganttChart.SetTaskColor(taskB, Brushes.Green); // משימה שהושלמה
        //ganttChart.SetTaskColor(taskC, Brushes.Red); // משימה שמתעכבת
        //ganttChart.SetTaskColor(taskD, Brushes.Red); // משימה שמתעכבת
        //ganttChart.SetTaskColor(taskE, Brushes.Yellow); // משימה שטרם התחילה
    }
    // מחלקה המייצגת משימה בתרשים גאנט
    public class GanttTask
    {
        public string Name { get; }
        public string Id { get; }
        public DateTime StartDate { get; }
        public TimeSpan Duration { get; }

        public GanttTask(string name, string id, DateTime startDate, TimeSpan duration)
        {
            Name = name;
            Id = id;
            StartDate = startDate;
            Duration = duration;
        }
    }

    // מחלקה המייצגת תרשים גאנט
    public class GanttChart : Canvas
    {
        private const double TaskHeight = 30;

        public void AddTask(GanttTask task)
        {
            var rectangle = new Rectangle
            {
                Width = task.Duration.TotalDays * 20,
                Height = TaskHeight,
                Fill = Brushes.LightBlue
            };

            Children.Add(rectangle);

            SetLeft(rectangle, (task.StartDate - DateTime.Today).TotalDays * 20);
            SetTop(rectangle, Children.Count * TaskHeight);
        }

        public void SetTaskColor(GanttTask task, Brush color)
        {
            foreach (var child in Children)
            {
                if (child is Rectangle rectangle && (string)rectangle.Tag == task.Id)
                {
                    rectangle.Fill = color;
                }
            }
        }
    }
}

