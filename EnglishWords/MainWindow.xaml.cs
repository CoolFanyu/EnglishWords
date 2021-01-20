using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace EnglishWords
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int m_count = 0;
        private int m_correctCount = 0;
        private List<string> m_EnglishList = new List<string>();
        private List<string> m_ChineseList = new List<string>();
        private List<string> m_ChineseTestList = new List<string>();
        private List<int> errorList = new List<int>();

        static Random m_rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnAdd(object sender, RoutedEventArgs e)
        {
            if (tbEnglish.Text != null && tbChinese.Text != null)
            {
                m_EnglishList.Add(tbEnglish.Text);
                m_ChineseList.Add(tbChinese.Text);
                m_count++;
                tbEnglish.Clear();
                tbChinese.Clear();
                tbOK.Visibility = Visibility.Visible;
            }
            else
            {
                tbOK.Visibility = Visibility.Collapsed;
                MessageBox.Show("Please enter your word in the text box area first.");
            }
        }

        private void OnTest(object sender, RoutedEventArgs e)
        {
            if (m_count > 0)
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnConfirm.Visibility = Visibility.Visible;
                tbOK.Visibility = Visibility.Collapsed;
                m_ChineseTestList = m_ChineseList.ToList();
                ShowRandomChinese();
            }
            else
            {
                MessageBox.Show("Please add some words to the list first");
            }
        }

        private void ShowRandomChinese()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                tbChinese.Text = m_ChineseTestList[m_rnd.Next(m_ChineseTestList.Count)];
                m_ChineseTestList.Remove(tbChinese.Text);
            }));
        }

        private void CheckAns(object sender, RoutedEventArgs e)
        {
            int index = m_ChineseList.IndexOf(tbChinese.Text);
            if(tbEnglish.Text == m_EnglishList[index])
            {
                m_correctCount++;
            }
            else
            {
                MessageBox.Show("Incorrect!\nAnswer:" + m_EnglishList[index] + "\t" + m_ChineseList[index]);
                errorList.Add(index);
            }

            tbEnglish.Clear();
            tbChinese.Clear();
            if(m_ChineseTestList.Count > 0 )
            {
                ShowRandomChinese();
            }
            else
            {
                float score = m_correctCount / m_count * 100;
                string result = string.Format("Your score is {0}%\n", score);

                if (errorList.Count != 0)
                {
                    result += "Error List:\n";
                    for (int i = 0; i < errorList.Count(); i++)
                    {
                        result += string.Format("{0}\t{1}\n", m_EnglishList[errorList[i]], m_ChineseList[errorList[i]]);
                    }
                }

                // reset
                m_correctCount = 0;
                errorList.Clear();

                // show result
                MessageBox.Show(result);
 
                btnConfirm.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Visible;
            }
        }
    }
}
