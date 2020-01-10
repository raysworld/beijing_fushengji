using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MicroMvvm;

namespace Life40Days
{
    /// <summary>
    /// Interaction logic for TradePage.xaml
    /// </summary>
    public partial class TradePage : Window
    {
        #region Hide the close buttion on the upright corner
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        #endregion


        public TradeViewModel TVM { get; }
        public TradePage(
            String title = "Message", String main = "Please notice",
            Double value = 10, Double min = 0, Double max = 100,
            String c_text = "Buy", String d_text = "No Way")
        {
            InitializeComponent();

            DataContext = TVM = new TradeViewModel(title, main, value, min, max, c_text, d_text);

            TVM.ButtonClicked += TVM_ButtonClicked;
        }

        private void TVM_ButtonClicked()=> this.Close();


    }

    public class TradeViewModel:ObservableObject
    {
        private String _titleMsg;
        private String _mainMsg;
        private Double _value;
        private Double _max,_min;
        private String _confirmBtnText;
        private String _declineBtnText;
        private Boolean? _isConfirm;

        public TradeViewModel(
            String title = "Message", String main = "Please notice",
            Double value = 10, Double min = 0, Double max = 100,
            String c_text = "Buy", String d_text = "No Way")
        {
            _titleMsg = title;
            _mainMsg = main;
            _value = value;
            _max = max;
            _min = min;
            _confirmBtnText = c_text;
            _declineBtnText = d_text;
            _isConfirm = null;
        }

        public String TitleMsg
        {
            get => _titleMsg;
            set => Set(ref _titleMsg, value);
        }
        public String MainMsg
        {
            get => _mainMsg;
            set => Set(ref _mainMsg, value);
        }
        public Double Value
        {
            get => _value;
            set => Set(ref _value, value);
        }
        public Double Max
        {
            get => _max;
            set => Set(ref _max, value);
        }
        public Double Min
        {
            get => _min;
            set => Set(ref _min, value);
        }
        public String ConfirmBtnText
        {
            get => _confirmBtnText;
            set => Set(ref _confirmBtnText, value);
        }
        public String DeclineBtnText
        {
            get => _declineBtnText;
            set => Set(ref _declineBtnText, value);
        }
        public Boolean? IsConfirm
        {
            get => _isConfirm;
            set => Set(ref _isConfirm, value);
        }

        public delegate void ButtonClickHandler();
        public event ButtonClickHandler ButtonClicked;
        public void OnButtonClicked() => ButtonClicked?.Invoke();


        public ICommand ConfirmCommand { get => new RelayCommand(ConfirmExecute, CanConfirmExecute); }
        private bool CanConfirmExecute() => true;
        private void ConfirmExecute()
        { IsConfirm = true; OnButtonClicked(); }

        public ICommand DeclineCommand { get => new RelayCommand(DeclineExecute, CanDeclineExecute); }
        private bool CanDeclineExecute() => true;
        private void DeclineExecute()
        { IsConfirm = false; OnButtonClicked(); }
    }
}
