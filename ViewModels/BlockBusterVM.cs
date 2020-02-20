using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BlockBusters
{
    class BlockBusterVM : ViewModelBase
    {
        public int Counter { get; set; }
        public Dictionary<string, string> BoardsDictionary { get; set; }

        #region Properties accessed by the UI
        private bool _showForm;
        public bool ShowForm
        {
            get { return _showForm; }
            set { _showForm = value; RaisePropertyChanged("ShowForm"); }
        }

        private bool _showBoard;        // Todo- Change the converter so we only need one flag instead of both showform adn showboard
        public bool ShowBoard
        {
            get { return _showBoard; }
            set { _showBoard = value; RaisePropertyChanged("ShowBoard"); }
        }

        public string SelectedBoard { get; set; }


        public ObservableCollection<string> BoardNames { get; set; }
        public ObservableCollection<string> AllColors { get; set; }

        public RelayCommand FlashAllCommand { get; set; }
        public RelayCommand FlashTeam1Command { get; set; }
        public RelayCommand FlashTeam2Command { get; set; }
        public RelayCommand SetNewGameCommand { get; set; }
        public RelayCommand ResetGameCommand { get; set; }
        public RelayCommand<int> SetTeam1ColorCommand { get; set; }
        public RelayCommand<int> SetTeam2ColorCommand { get; set; }
        public RelayCommand<int> FlashButtonCommand { get; set; }
        public RelayCommand<int> FlashOrResetButtonCommand { get; set; }
        public RelayCommand StartNewGameCommand { get; set; }

        private Board _board;
        public Board Board
        {
            get { return _board; }
            set { _board = value; RaisePropertyChanged("Board"); }
        }

        #endregion

        public BlockBusterVM()
        {
            // Initialize commands here
            FlashAllCommand = new RelayCommand(SetGameWon);
            FlashTeam1Command = new RelayCommand(FlashTeam1Winning);
            FlashTeam2Command = new RelayCommand(FlashTeam2Winning);
            SetTeam1ColorCommand = new RelayCommand<int>(SetTeam1Color);
            SetTeam2ColorCommand = new RelayCommand<int>(SetTeam2Color);
            FlashOrResetButtonCommand = new RelayCommand<int>(FlashOrResetButton);
            SetNewGameCommand = new RelayCommand(SetNewGame);
            ResetGameCommand = new RelayCommand(ResetGame);
            StartNewGameCommand = new RelayCommand(StartNewGame);

            // Get these values for the first time and only once during app
            SelectedBoard = "Board1";
            GetAllColors();
            GetAllBoards();

            Board = Board.Get();
            SetNewGame();

        }

        private void SetGameWon()
        {
            try
            {
                if (Board != null && Board.Cells != null && Board.Cells.Count > 0)
                {
                    foreach (Cell cell in Board.Cells)
                    {

                        cell.FlashingFlag = !cell.FlashingFlag;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private void FlashTeam2Winning()
        {
            try
            {
                if (Board != null && Board.Cells != null && Board.Cells.Count > 0)
                {
                    foreach (Cell cell in Board.Cells)
                    {
                        if (cell.CellColor == Board.Team2Color)
                        {
                            cell.FlashingFlag = !cell.FlashingFlag;
                        }
                        else
                        {
                            cell.FlashingFlag = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private void FlashTeam1Winning()
        {
            try
            {
                if (Board != null && Board.Cells != null && Board.Cells.Count > 0)
                {
                    foreach (Cell cell in Board.Cells)
                    {
                        if (cell.CellColor == Board.Team1Color)
                        {
                            cell.FlashingFlag = !cell.FlashingFlag;
                        }
                        else
                        {
                            cell.FlashingFlag = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private void StartNewGame()
        {
            try
            {
                if (Board.Cells.Count > 0)
                    Board.Cells.Clear();

                Board.Team1ColorGradient = CreateColorGradient(System.Drawing.Color.FromName(Board.Team1Color), "TopBottomLabel");
                Board.Team2ColorGradient = CreateColorGradient(System.Drawing.Color.FromName(Board.Team2Color), "LeftRightLabel");
                string line = BoardsDictionary[SelectedBoard];
                //assign to each cell
                List<string> letters = line.Split(',').ToList();

                for (int i = 0; i < 20; i++)
                {
                    Board.Cells.Add(new Cell() { CellColor = Board.DefaultColor, Letter = letters[i], CellNumber = i, CellBorderGradient = Board.DefaultBorderGradient });
                }

                ShowBoard = true;
                ShowForm = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private void GetAllColors()
        {
            try
            {
                AllColors = new ObservableCollection<string>();
                foreach (var color in typeof(Colors).GetProperties())
                {
                    AllColors.Add(color.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private void SetNewGame()
        {
            try
            {
                ShowForm = true;
                ShowBoard = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private void FlashOrResetButton(int cellNumber)
        {
            try
            {
                // for given cell number. Make the button flash
                // stop any other button from flashing
                foreach (Cell cell in Board.Cells)
                {
                    if (cell.CellNumber == cellNumber)
                    {
                        cell.FlashingFlag = !cell.FlashingFlag;
                        cell.CellColor = Board.DefaultColor;
                        cell.CellBorderGradient = (Application.Current.TryFindResource("GoldenGradientBrush") as LinearGradientBrush);
                    }
                    else
                    {
                        cell.FlashingFlag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private void SetTeam2Color(int cellNumber)
        {
            try
            {
                foreach (Cell cell in Board.Cells)
                {
                    if (cell.CellNumber == cellNumber)
                    {
                        cell.CellColor = Board.Team2Color;
                        cell.FlashingFlag = false;
                        cell.CellBorderGradient = CreateColorGradient(System.Drawing.Color.FromName(Board.Team2Color), "Cell");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private void SetTeam1Color(int cellNumber)
        {
            try
            {
                foreach (Cell cell in Board.Cells)
                {
                    if (cell.CellNumber == cellNumber)
                    {
                        cell.CellColor = Board.Team1Color;
                        cell.FlashingFlag = false;
                        cell.CellBorderGradient = CreateColorGradient(System.Drawing.Color.FromName(Board.Team1Color), "Cell");

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private void ResetGame()
        {
            try
            {
                LinearGradientBrush brush = (Application.Current.TryFindResource("GoldenGradientBrush") as LinearGradientBrush);
                foreach (Cell cell in Board.Cells)
                {
                    cell.CellColor = Board.DefaultColor;
                    cell.CellBorderGradient = brush;
                    cell.FlashingFlag = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }

        private LinearGradientBrush CreateColorGradient(System.Drawing.Color color, string type)
        {
            try
            {
                float r, g, b = 0;
                if (color.Name.StartsWith("#"))
                {
                    string hex = color.Name;
                    r = Convert.ToInt32(hex.Substring(3, 2), 16);
                    g = Convert.ToInt32(hex.Substring(5, 2), 16);
                    b = Convert.ToInt32(hex.Substring(7, 2), 16);
                }
                else
                {
                    r = color.R;
                    g = color.G;
                    b = color.B;
                }

                byte darkred = (byte)(r * 0.8);
                byte darkgreen = (byte)(g * 0.8);
                byte darkblue = (byte)(b * 0.8);
                Color darkColor = Color.FromRgb(darkred, darkgreen, darkblue);

                byte lightred = r == 0 ? (byte)0 : (byte)((255 - r) * 0.5 + r);
                byte lightgreen = g == 0 ? (byte)0 : (byte)((255 - g) * 0.5 + g);
                byte lightblue = b == 0 ? (byte)0 : (byte)((255 - b) * 0.5 + b);
                Color lightColor = Color.FromRgb(lightred, lightgreen, lightblue);

                Color normalColor = Color.FromRgb((byte)r, (byte)g, (byte)b);

                LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
                if (type == "Cell")
                {
                    linearGradientBrush.StartPoint = new Point(0, 0);
                    linearGradientBrush.EndPoint = new Point(1, 1);
                }
                else if (type == "TopBottomLabel")
                {
                    linearGradientBrush.StartPoint = new Point(0.5, 0);
                    linearGradientBrush.EndPoint = new Point(0.5, 1);
                }
                else if (type == "LeftRightLabel")
                {
                    linearGradientBrush.StartPoint = new Point(1, 0.5);
                    linearGradientBrush.EndPoint = new Point(0, 0.5);
                }

                linearGradientBrush.GradientStops.Add(new GradientStop(darkColor, 0.0));
                linearGradientBrush.GradientStops.Add(new GradientStop(normalColor, 0.5));
                linearGradientBrush.GradientStops.Add(new GradientStop(lightColor, 1.0));

                return linearGradientBrush;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
                return null;
            }
        }

        public async void GetAllBoards()
        {
            try
            {
                StreamReader file = new StreamReader(@".\Boards\boards.json");
                string content = await file.ReadToEndAsync();
                if (content != null)
                {
                    BoardsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                    BoardNames = new ObservableCollection<string>();
                    foreach (var keyvalue in BoardsDictionary)
                    {
                        BoardNames.Add(keyvalue.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occcured: {ex.Message}");
            }
        }
    }

}
