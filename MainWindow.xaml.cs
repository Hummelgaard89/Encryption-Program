using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Forms;

namespace Encryption_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EncryptionClass ec = new EncryptionClass();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void EncryptInputClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                txtEncryptInput.Text = Path.GetFullPath(openFileDialog.FileName);
            }
        }

        private void EncryptOutputClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK")
            {
                txtEncryptOutput.Text = folderDialog.SelectedPath;
            }
        }

        private void DecryptInputClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                txtDecryptInput.Text = Path.GetFullPath(openFileDialog.FileName);
            }
        }

        private void DecryptOutputClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK")
            {
                txtDecrptOutput.Text = folderDialog.SelectedPath;
            }

        }

        private void btnGenerateKey(object sender, RoutedEventArgs e)
        {
            ec.SetEncryptionTypeAndMode(cbEncryptionType.Text, cbEncryptionMode.Text);
            txtEncryptionKey.Text = Convert.ToBase64String(ec.KEY);
            txtEncryptionIV.Text = Convert.ToBase64String(ec.IV);
        }

        private void btnEncryptdata(object sender, RoutedEventArgs e)
        {
            txtTimeElapsed.Text = "Time elapsed during encryption: " + ec.EncryptData(txtEncryptInput.Text, txtEncryptOutput.Text, txtEncryptionKey.Text, txtEncryptionIV.Text) + "ms";
        }

        private void btnDecryptData(object sender, RoutedEventArgs e)
        {
            txtTimeElapsed.Text = "Time elapsed during decryption: " + ec.Decryptdata(txtDecryptInput.Text, txtDecrptOutput.Text, txtEncryptionKey.Text, txtEncryptionIV.Text) + "ms";

        }
    }
}
