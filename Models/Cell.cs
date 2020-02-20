using System.Windows.Media;

namespace BlockBusters
{
    class Cell : ViewModelBase
    {
        private string _letter;
        public string Letter
        {
            get { return _letter; }
            set { _letter = value; RaisePropertyChanged("Letter"); }
        }

        public int CellNumber { get; set; }

        private string _cellColor;
        public string CellColor
        {
            get { return _cellColor; }
            set
            {
                _cellColor = value; RaisePropertyChanged("CellColor");
            }
        }

        private LinearGradientBrush _cellBorderGradient;
        public LinearGradientBrush CellBorderGradient
        {
            get { return _cellBorderGradient; }
            set { _cellBorderGradient = value; RaisePropertyChanged("CellBorderGradient"); }
        }

        private bool _flashingFlag;

        public bool FlashingFlag
        {
            get { return _flashingFlag; }
            set { _flashingFlag = value; RaisePropertyChanged("FlashingFlag"); }
        }

    }

}
