using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime startTime = DateTime.Now;
        double writtenChars = 0;

        bool Caps
        {
            get
            {
                return (Keyboard.IsKeyToggled(Key.CapsLock) != (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            if (Caps)
            {
                Capital(true);
            }
        }

        private void Capital(bool capital = false)
        {
            for (char i = 'A'; i <= 'Z'; i++)
            {
                Button button = FindName("Button" + i) as Button;
                string text = ((button.Content as Viewbox).Child as TextBlock).Text;
                ((button.Content as Viewbox).Child as TextBlock).Text = capital ? text.ToUpper() : text.ToLower();
            }
            (((FindName("ButtonOem3") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "~" : "`";
            (((FindName("ButtonD1") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "!" : "1";
            (((FindName("ButtonD2") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "@" : "2";
            (((FindName("ButtonD3") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "#" : "3";
            (((FindName("ButtonD4") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "$" : "4";
            (((FindName("ButtonD5") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "%" : "5";
            (((FindName("ButtonD6") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "^" : "6";
            (((FindName("ButtonD7") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "&" : "7";
            (((FindName("ButtonD8") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "*" : "8";
            (((FindName("ButtonD9") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "(" : "9";
            (((FindName("ButtonD0") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? ")" : "0";
            (((FindName("ButtonOemMinus") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "_" : "-";
            (((FindName("ButtonOemPlus") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "+" : "=";
            (((FindName("ButtonOemOpenBrackets") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "{" : "[";
            (((FindName("ButtonOem6") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "}" : "]";
            (((FindName("ButtonOem5") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "|" : "\\";
            (((FindName("ButtonOem1") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? ":" : ";";
            (((FindName("ButtonOemQuotes") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "\"" : "\'";
            (((FindName("ButtonOemComma") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "<" : ",";
            (((FindName("ButtonOemPeriod") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? ">" : ".";
            (((FindName("ButtonOemQuestion") as Button).Content as Viewbox).Child as TextBlock).Text = capital ? "?" : "/";
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            writtenChars = 0;
            SpeedLabel.Text = "-";
            FailsLabel.Text = "0";
            CaseSensetiveCheckBox.IsEnabled = false;
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            startTime = DateTime.Now;
            words = (wordList.Split('|')).OrderBy(a => Guid.NewGuid()).ToList();
            SourceText.Text = String.Empty;
            for (int i = 0; i < 16; i++)
            {
                SourceText.Text += (words[i] + ' ');
            }
            UserText.IsEnabled = true;
            UserText.Focus();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            SpeedLabel.Text = (writtenChars/(DateTime.Now - startTime).TotalMinutes).ToString("F0");
            CaseSensetiveCheckBox.IsEnabled = true;
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            UserText.IsEnabled = false;
            UserText.Text = String.Empty;

            ButtonEnter.Background = new SolidColorBrush(Colors.LightGray);
            ButtonSystem.Background = ButtonEnter.Background;
            ButtonBack.Background = ButtonEnter.Background;
            ButtonSpace.Background = ButtonEnter.Background;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                string buttonName = "Button" + e.Key.ToString();
                try
                {
                    Button button = FindName(buttonName) as Button;
                    Color color = ((SolidColorBrush)button.Background).Color;
                    button.Background = new SolidColorBrush(Color.Subtract(color, Color.FromArgb(0, 191, 191, 191)));
                }
                catch (Exception)
                {

                }
                finally
                {
                    if(e.Key==Key.Capital || e.Key == Key.LeftShift || e.Key == Key.RightShift)
                    {
                        Capital(Caps);
                    }
                    if (e.Key == Key.Enter)
                    {
                        ButtonEnter.Background = new SolidColorBrush(Color.Subtract(((SolidColorBrush)ButtonEnter.Background).Color, Color.FromArgb(0, 191, 191, 191)));
                    }
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string buttonName = "Button" + e.Key.ToString();
                Button button = FindName(buttonName) as Button;
                Color color = ((SolidColorBrush)button.Background).Color;
                button.Background = new SolidColorBrush(Color.Add(color, Color.FromArgb(0, 191, 191, 191)));
            }
            catch (Exception)
            {

            }
            finally
            {
                if (e.Key == Key.Capital || e.Key == Key.LeftShift || e.Key == Key.RightShift)
                {
                    Capital(Caps);
                }
                if(e.Key == Key.Enter)
                {
                    ButtonEnter.Background = new SolidColorBrush(Color.Add(((SolidColorBrush)ButtonEnter.Background).Color, Color.FromArgb(0, 191, 191, 191)));
                }
                if (e.Key == Key.Space && StartButton.IsEnabled == false)
                {
                    if ((bool)CaseSensetiveCheckBox.IsChecked && words[0] == UserText.Text.Substring(0, UserText.Text.Length - 1))
                    {
                        writtenChars += UserText.Text.Length;
                    }
                    else if (!(bool)CaseSensetiveCheckBox.IsChecked && words[0].ToLower() == UserText.Text.Substring(0, UserText.Text.Length - 1).ToLower())
                    {
                        writtenChars += UserText.Text.Length;
                    }
                    else
                    {
                        FailsLabel.Text = (Int32.Parse(FailsLabel.Text) + 1).ToString();
                    }
                    UserText.Text = String.Empty;
                    words.RemoveAt(0);
                    SourceText.Text = String.Empty;
                    if (words.Count == 0)
                    {
                        StopButton_Click(sender, new RoutedEventArgs());
                    }
                    else
                    {
                        if (words.Count >= 16)
                        {
                            for (int i = 0; i < 16; i++)
                            {
                                SourceText.Text += (words[i] + ' ');
                            }
                        }
                        else
                        {
                            foreach (var item in words)
                            {
                                SourceText.Text += (item + ' ');
                            }
                        }
                    }
                }
            }
        }

        readonly string wordList = "about|above|add|after|again|air|all|almost|along|also|always|America|an|and|animal|another|answer|any|" +
            "are|around|as|ask|at|away|back|be|because|been|before|began|begin|being|below|between|big|book|both|boy|but|by|call|" +
            "came|can|car|carry|change|children|city|close|come|could|country|cut|day|did|different|do|does|don't|down|each|earth|" +
            "eat|end|enough|even|every|example|eye|face|family|far|father|feet|few|find|first|follow|food|for|form|found|four|from|" +
            "get|girl|give|go|good|got|great|group|grow|had|hand|hard|has|have|he|head|hear|help|her|here|high|him|his|home|house|" +
            "how|idea|if|important|in|Indian|into|is|it|its|it's|just|keep|kind|know|land|large|last|later|learn|leave|left|let|" +
            "letter|life|light|like|line|list|little|live|long|look|made|make|man|many|may|me|mean|men|might|mile|miss|more|most|" +
            "mother|mountain|move|much|must|my|name|near|need|never|new|next|night|no|not|now|number|of|off|often|oil|old|on|once|" +
            "one|only|open|or|other|our|out|over|own|page|paper|part|people|picture|place|plant|play|point|put|question|quick|" +
            "quickly|quite|read|really|right|river|run|said|same|saw|say|school|sea|second|see|seem|sentence|set|she|should|show|" +
            "side|small|so|some|something|sometimes|song|soon|sound|spell|start|state|still|stop|story|study|such|take|talk|tell|" +
            "than|that|the|their|them|then|there|these|they|thing|think|this|those|thought|three|through|time|to|together|too|took|" +
            "tree|try|turn|two|under|until|up|us|use|very|walk|want|was|watch|water|way|we|well|went|were|what|when|where|which|" +
            "while|white|who|why|will|with|without|word|work|world|would|write|year|you|young|your";
        List<string> words;
    }
}
