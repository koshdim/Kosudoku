using System;
using System.Windows;
using Kosudoku.ViewModels;

namespace Kosudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            InitializeComponent();

            Closing += MainWindowClosing;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindowViewModel) DataContext).Save();
        }
    }
}
