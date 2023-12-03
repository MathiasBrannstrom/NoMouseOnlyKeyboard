using NoMouseOnlyKeyboard.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Utilities;
using Point = NoMouseOnlyKeyboard.Interfaces.Point;

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

        public Rectangle Region { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChange([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Point GetCenter()
        {
            return new Point() { X = Region.Left + (Left + Width / 2), Y = Region.Top + Top + Height / 2};
        }
    }

    public class RegionViewModel
    {
        public IEnumerable<GridNavigationItem> GridNavigationItems { get; set; }
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

        //private bool _isMainRegion;
        //public bool IsMainRegion
        //{
        //    get { return _isMainRegion; }
        //    set
        //    {
        //        if (_isMainRegion != value)
        //        {
        //            _isMainRegion = value;
        //            NotifyOfPropertyChange();
        //        }
        //    }
        //}

        public ValueHolder<string> MatchString { get; set; }

        public ValueHolder<bool> Visible { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChange([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class GridNavigationViewModel : INotifyPropertyChanged
    {


        private bool _visible;
        private TaskCompletionSource<Point>? _taskCompletionSource;
        private Dictionary<string, Rectangle> _regions;

        public ValueHolder<bool> Visible = new ValueHolder<bool>();

        public List<RegionViewModel> RegionViewModels { get; private set; }

        private ValueHolder<string> _matchString = new ValueHolder<string>();

        public GridNavigationViewModel()
        {
            RegionViewModels = new List<RegionViewModel>();
            _matchString.ValueChanged += NewMatchString;
        }

        public System.Action CloseOverlay { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChange([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void NewMatchString()
        {
            var allItems = RegionViewModels.SelectMany(r => r.GridNavigationItems);
            var value = _matchString.Value;
            if (value.Length > 0)
            {
                foreach (var x in allItems)
                {
                    x.Active = false;
                }
            }

            var matching = allItems.Where(x => x.Key.StartsWith(value, StringComparison.OrdinalIgnoreCase)).ToArray();

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

        internal void UpdateSizeOfGrid()
        {
            RegionViewModels.Clear();
            foreach (var region in  _regions)
            {
                var rows = 14;
                var columns = 14;
                var labels = HintLabelGeneration.GenerateHintStrings(rows * columns);
   
                var margin = 10.0;

                var width = (region.Value.Width - margin * (columns + 1)) / columns;
                var height = (region.Value.Height - margin * (rows + 1)) / rows;

                var items = new ObservableCollection<GridNavigationItem>();
                for (var r = 0; r < rows; r++)
                    for (var c = 0; c < columns; c++)
                    {
                        items.Add(new GridNavigationItem { Top = r * (height + margin) + margin, Left = c * (width + margin) + margin, Height = height, Width = width, Key = region.Key + labels[r * columns + c], Region = region.Value });
                    }

                var regionViewModel = new RegionViewModel { GridNavigationItems = items, Top = region.Value.Top, Left = region.Value.Left, Height = region.Value.Height, Width = region.Value.Width, Visible = Visible };
                if(region.Key == "")
                {
                    regionViewModel.MatchString = _matchString;
                }

                RegionViewModels.Add(regionViewModel);
                //return;
            }
        }

        internal void SelectGridNavigationItem(GridNavigationItem item)
        {
            var center = item.GetCenter();
            if(_taskCompletionSource != null)
            {
                Visible.Value = false;
                _taskCompletionSource.SetResult(center);
                _taskCompletionSource = null;
            }
        }

        public Task<Point> GetSelectedPosition()
        {
            Visible.Value = true;
            if (_taskCompletionSource != null)
            {
                _taskCompletionSource.TrySetCanceled();
            }

            _taskCompletionSource = new TaskCompletionSource<Point>();


            return _taskCompletionSource.Task;
        }

        public void UpdateAvailableRegions(Rectangle primaryRegion, IEnumerable<Rectangle> otherRegions)
        {
            _regions = new Dictionary<string, Rectangle>
            {
                { "", primaryRegion }
            };

            var regionCharacters = new[] { "L", "D" }; // Should be moved somewhere.
            var i = 0;
            foreach (var region in otherRegions)
            {
                _regions.Add(regionCharacters[i++], region);
            }

            UpdateSizeOfGrid();
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
