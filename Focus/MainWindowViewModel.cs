using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFUI.Common;
using static PInvoke.User32;

namespace Focus {

    public class MainWindowViewModel: DependencyObject {
        public ICommand RefreshCommand { get; }

        public static readonly DependencyProperty WindowsFilterProperty =
            DependencyProperty.Register(
                nameof(WindowsFilter),
                typeof(string),
                typeof(MainWindowViewModel),
                new PropertyMetadata(string.Empty, OnWindowsFilterChanged));
        public string WindowsFilter {
            get => (string)GetValue(WindowsFilterProperty);
            set => SetValue(WindowsFilterProperty, value);
        }
        private static void OnWindowsFilterChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e) {
            ((MainWindowViewModel)d).UpdateWindowsFiltered();
        }


        public ObservableCollection<RunningWindowViewModel> WindowsFiltered { get; private set; } = new();

        private List<RunningWindowViewModel> Windows { get; } = new();


        public MainWindowViewModel() {
            RefreshCommand = new RelayCommand(RefreshWindows);
        }

        private void RefreshWindows() {
            Windows.Clear();
            var windows = RunningWindowEnumerator.Enumerate();
            foreach (var w in windows) {
                if (! RunningWindowFilters.Default(w)) continue;
                Windows.Add(new RunningWindowViewModel(w));
            }
            UpdateWindowsFiltered();
        }

        private void UpdateWindowsFiltered() {
            WindowsFiltered.Clear();
            var filtered = Windows.Where(
                o => o.Model.Name.ToLower().Contains(WindowsFilter.ToLower()));
            foreach (var o in filtered)
                WindowsFiltered.Add(o);
        }
    }
}
