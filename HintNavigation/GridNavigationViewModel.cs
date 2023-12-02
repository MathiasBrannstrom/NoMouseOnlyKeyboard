using NoMouseOnlyKeyboard.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HintNavigation
{
    public class GridNavigationItem : INotifyPropertyChanged
    {
        private double _top;
        public double Top
        {
            get { return _top; }
            set
            {
                if (_top != value)
                {
                    _top = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private double _left;
        public double Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key != value)
                {
                    _key = value;
                    NotifyOfPropertyChange();
                }
            }
        }


        private bool _isActive = true;
        public bool Active
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChange([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Point GetCenter()
        {
            return new Point() { X = (Left + Width / 2), Y = Top + Height / 2};
        }
    }

    public class GridNavigationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<GridNavigationItem> _items;

        public IEnumerable<GridNavigationItem> GridNavigationItems => _items;

        private bool _visible;
        private TaskCompletionSource<Point>? _taskCompletionSource;

        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public GridNavigationViewModel()
        {
            _items = new ObservableCollection<GridNavigationItem>();
        }

        public System.Action CloseOverlay { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChange([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        internal void UpdateSizeOfGrid(double actualWidth, double actualHeight)
        {
            var rows = 14;
            var columns = 14;
            var labels = HintLabelGeneration.GenerateHintStrings(rows * columns);

            _items.Clear();

            var margin = 10.0;

            var width = (actualWidth-margin*(columns+1)) / columns;
            var height = (actualHeight-margin*(rows+1)) / rows;

            for (var r = 0; r < rows; r++)
                for (var c = 0; c < columns; c++)
                {
                    _items.Add(new GridNavigationItem {Top=r*(height+margin)+margin, Left = c*(width+margin)+margin, Height=height, Width=width, Key = labels[r * columns + c] });
                }
        }

        public string MatchString
        {
            set
            {
                if (value.Length > 0)
                {
                    foreach (var x in GridNavigationItems)
                    {
                        x.Active = false;
                    }
                }
                
                var matching = GridNavigationItems.Where(x => x.Key.StartsWith(value, StringComparison.OrdinalIgnoreCase)).ToArray();

                if (matching.Count() == 1)
                {
                    SelectGridNavigationItem(matching[0]);
                    return;
                }

                foreach (var x in matching)
                {
                    x.Active = true;
                }
            }
        }

        internal void SelectGridNavigationItem(GridNavigationItem item)
        {
            var center = item.GetCenter();
            if(_taskCompletionSource != null)
            {
                Visible = false;
                _taskCompletionSource.SetResult(center);
                _taskCompletionSource = null;
            }
        }

        public Task<Point> GetSelectedPosition()
        {
            Visible = true;
            if (_taskCompletionSource != null)
            {
                _taskCompletionSource.TrySetCanceled();
            }

            _taskCompletionSource = new TaskCompletionSource<Point>();


            return _taskCompletionSource.Task;
        }
    }

    //public class HintViewModel : INotifyPropertyChanged
    //{
    //    private string _label;
    //    private bool _active;
    //    private string _fontSizeReadValue;

    //    public HintViewModel(Hint hint)
    //    {
    //        Hint = hint;
    //        FontSizeReadValue = "14"; //TODO 
    //    }

    //    public Hint Hint { get; set; }

    //    public bool Active
    //    {
    //        get { return _active; }
    //        set { _active = value; NotifyOfPropertyChange(); }
    //    }

    //    public string Label
    //    {
    //        get { return _label; }
    //        set { _label = value; NotifyOfPropertyChange(); }
    //    }

    //    public string FontSizeReadValue
    //    {
    //        get { return _fontSizeReadValue; }
    //        set { _fontSizeReadValue = value; NotifyOfPropertyChange(); }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void NotifyOfPropertyChange([CallerMemberName] string name = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    //    }
    //}
}
