using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace BlockBusters
{
    class Board : ViewModelBase
    {
        #region make singleton
        private static Board board = null;
        public static Board Get()
        {
            if (board == null)
                board = new Board();
            return board;
        }
        #endregion

        private string _team1Color;
        public string Team1Color
        {
            get { return _team1Color; }
            set { _team1Color = value; RaisePropertyChanged("Team1Color"); }
        }

        private LinearGradientBrush _team1ColorGradient;
        public LinearGradientBrush Team1ColorGradient
        {
            get { return _team1ColorGradient; }
            set { _team1ColorGradient = value; RaisePropertyChanged("Team1ColorGradient"); }
        }

        private string _team1Name;
        public string Team1Name
        {
            get { return _team1Name; }
            set { _team1Name = value; RaisePropertyChanged("Team1Name"); }
        }

        private string _team2Name;
        public string Team2Name
        {
            get { return _team2Name; }
            set { _team2Name = value; RaisePropertyChanged("Team2Name"); }
        }


        private string _team2Color;
        public string Team2Color
        {
            get { return _team2Color; }
            set { _team2Color = value; RaisePropertyChanged("Team2Color"); }
        }

        private LinearGradientBrush _team2ColorGradient;
        public LinearGradientBrush Team2ColorGradient
        {
            get { return _team2ColorGradient; }
            set { _team2ColorGradient = value; RaisePropertyChanged("Team2ColorGradient"); }
        }

        private string _defaultColor;
        public string DefaultColor
        {
            get { return _defaultColor; }
            set { _defaultColor = value; RaisePropertyChanged("DefaultColor"); }
        }

        private LinearGradientBrush _defaultBorderGradient;
        public LinearGradientBrush DefaultBorderGradient
        {
            get { return _defaultBorderGradient; }
            set { _defaultBorderGradient = value; RaisePropertyChanged("DefaultBorderGradient"); }
        }

        private int _rowSpan;
        public int RowSpan
        {
            get { return _rowSpan; }
            set { _rowSpan = value; RaisePropertyChanged("RowSpan"); }
        }

        private int _columnSpan;
        public int ColumnSpan
        {
            get { return _columnSpan; }
            set { _columnSpan = value; RaisePropertyChanged("ColumnSpan"); }
        }

        public ObservableCollection<Cell> Cells { get; set; }

        private Board()
        {
            Cells = new ObservableCollection<Cell>();
           // var temp = 
            DefaultColor = "#ffcc33";
            DefaultBorderGradient = (Application.Current.TryFindResource("GoldenGradientBrush") as LinearGradientBrush);
            Team1Color = Colors.DodgerBlue.ToString();
            Team2Color = Colors.White.ToString();
        }
    }
}