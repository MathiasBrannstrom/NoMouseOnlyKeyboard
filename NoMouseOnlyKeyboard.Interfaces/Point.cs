namespace NoMouseOnlyKeyboard.Interfaces
{
    public struct Point
    {
        public double X;
        public double Y;
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
