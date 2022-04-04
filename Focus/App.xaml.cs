using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Focus {
    public partial class App : Application {
        public static MainWindowViewModel MainViewModel => ((App)Current)._mainViewModel;
        private readonly MainWindowViewModel _mainViewModel = new();
    }
}
