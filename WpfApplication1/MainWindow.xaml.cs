using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;



namespace Strokes {
    //{Binding AppSet.lbTickets, Mode=TwoWay}//

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window {

        #region Debug UI and Transition Control Code
        int selectedItem;
        IntPtr hwndListBox;
        GameConsole listControl;
        Application app;
        Window myWindow;
        int itemCount;

        /// <summary>
        /// GameConsole ListBox Control Event Handler.
        /// Method used to record UI Events within the ListBox Control.
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">EventArgs</param>
        private void On_UIReady(object sender, EventArgs e) {
            app = System.Windows.Application.Current;
            myWindow = app.MainWindow;
            myWindow.SizeToContent = SizeToContent.WidthAndHeight;

            listControl = new GameConsole(GameConsoleElement.ActualHeight, GameConsoleElement.ActualWidth);
            GameConsoleElement.Child = listControl;
            listControl.MessageHook += new HwndSourceHook(ControlMsgFilter);
            hwndListBox = listControl.hwndListBox;
            //
            // Populate listbox
            for (int i = 0; i < 15; i++) {
                string itemText = "Game [" + i.ToString() + "]";
                SendMessage(hwndListBox, LB_ADDSTRING, IntPtr.Zero, itemText);
            }
            itemCount = SendMessage(hwndListBox, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);
            numItems.Text = "" + itemCount.ToString();
        }

        /// <summary>
        /// The ListBox and Event Message Filter to bring in Win32 Interopability
        /// </summary>
        /// <param name="hwnd">IntPtr</param>
        /// <param name="msg">Int32</param>
        /// <param name="wParam">IntPtr</param>
        /// <param name="lParam">IntPtr</param>
        /// <param name="handled">Bool</param>
        /// <returns>IntPtr</returns>
        private IntPtr ControlMsgFilter(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            int textLength;

            handled = false;
            if (msg == WM_COMMAND) {
                //
                // Extract the HIWORD
                switch ((uint)wParam.ToInt32() >> 16 & 0xFFFF) {
                    //
                    // Get the item text and display it
                    case LBN_SELCHANGE:
                        selectedItem = SendMessage(listControl.hwndListBox, LB_GETCURSEL, IntPtr.Zero, IntPtr.Zero);
                        textLength = SendMessage(listControl.hwndListBox, LB_GETTEXTLEN, IntPtr.Zero, IntPtr.Zero);
                        StringBuilder itemText = new StringBuilder();
                        SendMessage(hwndListBox, LB_GETTEXT, selectedItem, itemText);
                        selectedText.Text = itemText.ToString();
                        handled = true;
                        break;
                }
            }
            return IntPtr.Zero;
        }

        internal const int
          LBN_SELCHANGE = 0x00000001,
          WM_COMMAND = 0x00000111,
          LB_GETCURSEL = 0x00000188,
          LB_GETTEXTLEN = 0x0000018A,
          LB_ADDSTRING = 0x00000180,
          LB_GETTEXT = 0x00000189,
          LB_DELETESTRING = 0x00000182,
          LB_GETCOUNT = 0x0000018B;

        /// <summary>
        /// The samples calls SendMessage to obtain information from the control and modify its contents. 
        /// </summary>
        /// <param name="hwnd">IntPtr</param>
        /// <param name="msg">Int32</param>
        /// <param name="wParam">IntPtr</param>
        /// <param name="lParam">IntPtr</param>
        /// <returns>Int32</returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern Int16 SendMessage(IntPtr hwnd,
                                               int msg,
                                               IntPtr wParam,
                                               IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern Int16 SendMessage(IntPtr hwnd,
                                               int msg,
                                               int wParam,
                                               [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hwnd,
                                                  int msg,
                                                  IntPtr wParam,
                                                  String lParam);

        /// <summary>
        /// To append items, send the list box an LB_ADDSTRING message. 
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="args">EventArgs</param>
        private void AppendText(object sender, EventArgs args) {
            if (txtAppend.Text != string.Empty) {
                SendMessage(hwndListBox, LB_ADDSTRING, IntPtr.Zero, txtAppend.Text);
            }
            itemCount = SendMessage(hwndListBox, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);
            numItems.Text = "" + itemCount.ToString();
        }

        /// <summary>
        /// To delete items, send LB_GETCURSEL to get the index of the current selection and then LB_DELETESTRING to delete the item. 
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="args">EventArgs</param>
        private void DeleteText(object sender, EventArgs args) {
            selectedItem = SendMessage(listControl.hwndListBox, LB_GETCURSEL, IntPtr.Zero, IntPtr.Zero);
            //
            // Check for selected item
            if (selectedItem != -1) {
                SendMessage(hwndListBox, LB_DELETESTRING, (IntPtr)selectedItem, IntPtr.Zero);
            }
            itemCount = SendMessage(hwndListBox, LB_GETCOUNT, IntPtr.Zero, IntPtr.Zero);
            numItems.Text = "" + itemCount.ToString();
        }
        #endregion

        #region Legacy Properties --Partially supplied also by ApplicationSettings Class
        public static CreditsViewModel CreditsViewM;
        public static string strCompany { get; set; }
        public static string strProduct { get; set; }
        public static string strVersion { get; set; }
        private static int _intCredits;
        public static int intCredits {
            get { return _intCredits; }
            set { _intCredits = value;
            }
        }
        public static int intTickets { get; set; }
        public static int intScore { get; set; }
        public static string strFSOpathScore { get; set; }
        public static string strFSOpathSettings { get; set; }
        public static string strFSOpathGameExeFull { get; set; }
        public static string strFSOpathGameExeShort { get; set; }
        //public static ApplicationSettings AppSet { get; set; }
        public static string strFSOpathGameExe { get; set; }
        public static string scorePath { get; set; }
        public static double Ratio { get; set; }
        public static bool isOn { get; set; }
        public static int instructionsTimer { get; set; }
        public static int resultTimer { get; set; }
        public static int playAgainTimer { get; set; }
        private static bool _firsttime;
        public static bool firsttime { get { return _firsttime; } set { if (_firsttime != value) { _firsttime = value; } } }
        public static bool boolPlayAgain = false;
        public static int cycle { get; set; }
        private static bool _isSerialAlive { get; set; }
        public static int TimeofResultScreen { get; set; }
        public static int playAgainTimer1 { get; set; }
        public static double RatioConfig { get; set; }
        public string StrokesJSONfilename { get; set; }
        #endregion

        #region Embedded Game XML Properties
        /// <summary>
        /// Function to retrieve the file contents at runtime
        /// </summary>
        /// <param name="filename">String</param>
        /// <returns>String</returns>
        public string GetResourceXMLFile(string filename) {
            string strResult = string.Empty;
            using (Stream strStream = this.GetType().Assembly.GetManifestResourceStream("Strokes.Strokes_Priatek." + filename)) {
                using (StreamReader strSr = new StreamReader(strStream)) {
                    strResult = strSr.ReadToEnd();
                }
            }
            return strResult;
        }
        #endregion

        #region Background Worker and Cursor PInvoke UI

        /// <summary>
        /// Ticket Printer Line Feed (End of Line)
        /// </summary>
        const char LF = '\n';

        /// <summary>
        /// PInvoke Function for Cursor Properties
        /// </summary>
        /// <param name="bShow">Boolean</param>
        /// <returns>Int32</returns>
        [DllImport("user32.dll")]
        static extern int ShowCursor(bool bShow);
        /// <summary>
        /// Strokes Game Private Instance of ApplicationSettings. Background Worker by Windows Shell Executes
        /// </summary>
        public static BackgroundWorker bw = new BackgroundWorker();
        /// <summary>
        /// ForegroundWindow Property Imported Attribute 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns>Boolean</returns>
        [DllImport("user32")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        /// <summary>
        /// Background Worker Threaded Execution for Gameplay
        /// Calls embedded game executable
        /// Does timeout checks and handles (incorrectly) transitions for UI
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">DoWorkEventArgs</param>
        public void bw_DoWork(object sender, DoWorkEventArgs e) {


            strFSOpathGameExeFull = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\Strokes.exe";
            strFSOpathGameExeShort = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\";
            strFSOpathGameExe = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\Strokes.exe";
            scorePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\StrokesScores.xml";
            while (true) {
                if (isOn == true) {
                    isOn = false;
                    Process currentProcess = Process.GetCurrentProcess();
                    SetForegroundWindow(currentProcess.MainWindowHandle);
                    using (Process proc = new Process()) {
                        proc.StartInfo = new ProcessStartInfo(strFSOpathGameExe) {
                            WorkingDirectory = strFSOpathGameExeShort,
                            UseShellExecute = true,
                            CreateNoWindow = true,
                        };
                        proc.Start();
                        SetForegroundWindow(proc.MainWindowHandle);

                        //
                        // Show Results Screen Background
                        proc.WaitForExit();
                        if (firsttime) {
                            Dispatcher.BeginInvoke(new Action(() => {
                                mpStartScreen.LoadedBehavior = MediaState.Stop;
                                mpStartScreen.Visibility = Visibility.Collapsed;
                                imgPlayAgainBg.Visibility = Visibility.Visible;
                                imgResultScreenBg.Visibility = Visibility.Visible;
                                imgResultScreenBg.BringIntoView();
                            }));
                            //
                            // Hide Start Button UIs
                            //
                            //12
                            if (imgPlayAgainBg.Visibility == Visibility.Visible) {
                                Dispatcher.BeginInvoke(new Action(() => {
                                    imgPlayAgainBg.Visibility = Visibility.Collapsed;
                                }));
                            }
                            //
                            // Stop Music and Hide UI
                        } else {
                            Dispatcher.BeginInvoke(new Action(() => {
                                mpStartScreen.Visibility = Visibility.Collapsed;
                                mpStartScreenMusic.Visibility = Visibility.Collapsed;
                                mpStartScreen.LoadedBehavior = MediaState.Stop;
                                mpStartScreenMusic.LoadedBehavior = MediaState.Stop;
                                mpStartScreenMusic.Position = TimeSpan.Zero;
                                mpStartScreen.Position = TimeSpan.Zero;
                                imgStartGameBtn.Visibility = Visibility.Collapsed;
                            }));
                        }
                    }
                    if (firsttime) {
                        //
                        //14
                        Dispatcher.BeginInvoke(new Action(() => {
                            imgResultScreenBg.Visibility = Visibility.Visible;
                            imgResultScreenBg.BringIntoView();
                        }));

                        //
                        // Hide Start Button UIs
                        //
                        //13
                        if (imgPlayAgainBg.Visibility == Visibility.Visible) {
                            Dispatcher.BeginInvoke(new Action(() => {
                                imgPlayAgainBg.Visibility = Visibility.Collapsed;
                            }));
                        }
                        //
                        // Stop Music and Hide UI
                    } else {
                        Dispatcher.BeginInvoke(new Action(() => {
                            mpStartScreen.Visibility = Visibility.Collapsed;
                            mpStartScreen.Visibility = Visibility.Collapsed;
                            mpStartScreen.LoadedBehavior = MediaState.Stop;
                            mpStartScreen.LoadedBehavior = MediaState.Stop;
                            mpStartScreen.Position = TimeSpan.Zero;
                            mpStartScreen.Position = TimeSpan.Zero;
                            imgStartGameBtn.Visibility = Visibility.Collapsed;
                        }));
                    }
                    //ShowCursor(false);
                    //SetForegroundWindow(currentProcess.MainWindowHandle);
                    //this.Load(AppSet.strFSOpathSettings);                                  // Not being used --Moved to App.xaml.cs
                    //this.Save(AppSet, AppSet.StrokesJSONfilename);                     // Not being used --Moved to App.xaml.cs

                    //2
                    if (!firsttime) {
                        Dispatcher.BeginInvoke(new Action(() => {
                            imgResultScreenBg.Visibility = Visibility.Visible;
                            imgResultScreenBg.BringIntoView();
                        }));
                    }
                    Ratio = RatioConfig;
                    //
                    // Create XmlReader to read Strokes Game XML "Score" element and set runtime property for the score value
                    try {
                        //
                        // Forked from using scorePath instead using strFSOpathScore
                        //System.Windows.Forms.MessageBox.Show("Reading/Writing XML Data to this location:   " + scorePath.ToString(), "Strokes Scores: XML File");
                        using (XmlReader reader = XmlReader.Create(scorePath)) {
                            while (reader.Read()) {
                                if (reader.Value.Contains('\n'))
                                    continue;
                                switch (reader.Name) {
                                    case "score":
                                        if (reader.Read() && !String.IsNullOrWhiteSpace(reader.Value))
                                            if (reader.Value.Trim() != "\n") {
                                                intScore = Convert.ToInt32(reader.Value.Trim());
                                                intTickets = Convert.ToInt32(intScore * Ratio);
                                                AppSet.intTickets = intTickets;
                                                AppSet.intScore = intScore;
                                                Dispatcher.BeginInvoke(new Action(() => {
                                                    lbTickets.Content = intTickets.ToString();
                                                    lbScore.Content = intScore.ToString();
                                                    //lbCredits.Content = "\r\n" + intCredits.ToString() + @" / " + AppSet.intTokensMin.ToString();
                                                    if (intCredits > 0) {
                                                        intCredits--;
                                                    }
                                                    PrintTickets();
                                                }));
                                                if (cycle == 4) {
                                                    cycle = 1;
                                                } else {
                                                    cycle++;
                                                }
                                                AppSet.cycle = cycle;
                                                //ShellSetup.Score = 2000;
                                                //ShellSetup.PrintTickets();
                                            }
                                        break;
                                }
                            }
                        }
                    } catch (Exception ex1) {
                        System.Windows.MessageBox.Show(ex1.Message + ex1.InnerException);
                    }
                    //
                    //2#15
                    Dispatcher.BeginInvoke(new Action(() => {
                        lbTickets.Visibility = Visibility.Visible;
                        lbScore.Visibility = Visibility.Visible;
                        lbTickets.BringIntoView();
                        lbScore.BringIntoView();
                        imgResultScreenBg.Visibility = Visibility.Visible;
                        lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                        lbCredits.Visibility = Visibility.Visible;
                    }));
                    //Image image = Image.FromFile("./Strokes Screens/Results Screen/Tip" + cycle.ToString() + ".png");
                    //this.Invoke(new Action(() => { ShellSetup.Attract.pictureBox2.Image = image; }));
                    string strUri2 = Directory.GetCurrentDirectory() + @"/Tip" + cycle.ToString() + ".png";
                    Dispatcher.BeginInvoke(new Action(() => {
                        imgTip1.Source = new BitmapImage(new Uri(strUri2));
                        imgTip1.Visibility = Visibility.Visible;

                    }));
                    if (cycle == 4) {
                        cycle = 1;
                    } else {
                        cycle++;
                    }
                    AppSet.cycle = cycle;
                    //1#15
                    Dispatcher.BeginInvoke(new Action(() => {
                        lbTickets.BringIntoView();
                        lbScore.BringIntoView();
                        lbCredits.BringIntoView();
                        lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                        imgTip1.BringIntoView();
                    }));

                    //1
                    if (imgResultScreenBg.Visibility == Visibility.Visible) {
                        resultTimer--;
                        Thread.Sleep(1000);
                        if (resultTimer == 0) {
                            resultTimer = TimeofResultScreen;
                            Dispatcher.BeginInvoke(new Action(() => {
                                imgPlayAgainBg.Visibility = Visibility.Visible;
                                imgPlayAgainBg.BringIntoView();
                                imgResultScreenBg.Visibility = Visibility.Collapsed;
                                lbTickets.Visibility = Visibility.Collapsed;
                                lbScore.Visibility = Visibility.Collapsed;
                                imgTip1.Visibility = Visibility.Collapsed;
                            }));
                        }
                    } else if (imgPlayAgainBg.Visibility == Visibility.Visible) {
                        playAgainTimer--;
                        Thread.Sleep(1000);
                        if (playAgainTimer == 0) {
                            playAgainTimer = playAgainTimer1;
                            Dispatcher.BeginInvoke(new Action(() => {
                                mpAttractMode.Visibility = Visibility.Visible;
                                mpAttractMode.LoadedBehavior = MediaState.Play;
                                mpAttractMode.BringIntoView();
                                imgPlayAgainBg.Visibility = Visibility.Collapsed;
                            }));
                        }
                    }
                }
                //try { 
                ////
                //// Save JSON Configuration to File now that the Game !isOn
                //Dispatcher.BeginInvoke(new Action(() => {
                //    Save(AppSet, @"Strokes.json");
                //    PrintTickets();
                //}));
                //} catch (Exception eXe) {
                //    MessageBox.Show(eXe.Message,"HINT: Remove this method if an exception occurs.");
                //}
            }

        }

        #endregion

        #region GamePRO SerialPort, SerialDataReceived Handler, and PrintTickets
        /// <summary>
        /// Serial Port Object for Coin/Token/Deposit GamePro Mechanism 
        /// </summary>
        public static SerialPort gameProSerial = new SerialPort();
        /// <summary>
        /// SerialPort Object SerialData Received Event Handler. For Token/Coin Mechanism Deposit.
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">SerialDataReceivedEventArgs</param>
        private void SerialDataReceivedHandler(object sender, SerialDataReceivedEventArgs e) {
            //SerialPort serial = (SerialPort)sender;
            string indata = gameProSerial.ReadExisting();
            //Console.Write(indata);
            //CI001 is sent back from the GamePRO when Coin Input is received.
            if (indata == "CI001\n") {
                //System.Windows.Forms.MessageBox.Show("Coin Inserted" + "\r\nDATA Read: " + indata, "GamePro SerialData Received");
                intCredits++;
                //
                // Update ApplicationSettings Properties
                //if (intCredits >= AppSet.intTokensMin) {
                    //System.Windows.Forms.MessageBox.Show("Credits >= Minimum Tokens" + "\r\nCREDITS Value: " + intCredits.ToString() + "\r\nAppJSON Min Tokens Value: " + AppSet.intTokensMin.ToString(), "GamePro SerialData Received");
                //} else {
                    //if (intCredits < AppSet.intTokensMin) {
                        //System.Windows.Forms.MessageBox.Show("Credits < Minimum Tokens", "GamePro SerialData Received");
                    //}
                //}
                //##1
                //if ((!AppSet.AttractMode) && (!AppSet.isOn) && (lbCredits.IsVisible)) {
                //lbCredits.UpdateLayout();
                //Invoke(new Action(() => { lbCredits.Text = MainWindow.AppSet.intCredits.ToString(); }));
                //lbCredits.Invoke(new Action(() => { lbCredits.Refresh(); }));
                //lbCredits.Content = "\r\n" + MainWindow.AppSet.intCredits.ToString() + @" / " + MainWindow.AppSet.intTokensMin.ToString();
                //}
                if (intCredits > 0) {
                    AppSet.intCredits = intCredits;
                    MainWindow.AppSet.intCredits = intCredits;
                }
                Dispatcher.BeginInvoke(new Action(() => {
                    //lbTickets.BringIntoView();
                    //lbScore.BringIntoView();
                    //lbCredits.BringIntoView();
                    lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                    //imgTip1.BringIntoView();
                }));
            }
            
            

            //if (intCredits >= AppSet.intTokensMin) {
            //    //System.Windows.MessageBox.Show("Priatek Strokes Game Demo", "StartGameBtn_MouseDown Button | Click");
            //    //if (imgPlayAgainBg.Visibility == Visibility.Visible) {
            //    //  playAgainTimer = playAgainTimer1;
            //    //}
            //    imgResultScreenBg.Visibility = Visibility.Visible;
            //    imgPlayAgainBg.Visibility = Visibility.Visible;
            //    lbCredits.Visibility = Visibility.Visible;
            //    AppSet.AttractMode = false;
            //    AppSet.isOn = true;
            //    isOn = AppSet.isOn;
            //    AppSet.firsttime = true;
            //    firsttime = AppSet.firsttime;
            //    //lbCredits.Visibility = Visibility.Visible;
            //    //lbCredits.Content = "\r\n" + intCredits.ToString() + @" / " + AppSet.intTokensMin.ToString();
            //    //lbCredits.Cursor = Cursors.None;
            //    //mpStartScreen.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(mpStartScreen.Opacity, 0, TimeSpan.FromSeconds(2)));
            //    mpStartScreen.LoadedBehavior = MediaState.Stop;
            //    mpStartScreenMusic.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(mpStartScreenMusic.Volume, 0, TimeSpan.FromSeconds(2)));
            //    imgPlayAgainBg.Visibility = Visibility.Visible;

            //    //4
            //    imgPlayAgainBg.BringIntoView();
            //}
        }

        /// <summary>
        /// Print Tickets Method
        /// Send Signal to print tickets to GamePRO
        /// Logic to handle over 10 tickets
        /// Get in contact with Ken Krone on this issue??
        /// </summary>
        public static void PrintTickets() {
            intTickets = Convert.ToInt32(intScore * Ratio);
            intTickets = intTickets - 1;
            MainWindow.AppSet.intTickets = intTickets;
            AppSet.intTickets = intTickets;
            if (intTickets > 99) {
                intTickets = 99;
            }
            while (intTickets > 0) {
                if (intTickets < 10) {
                    MessageBox.Show("Would Print: TIC0 [" + MainWindow.AppSet.intTickets.ToString() + "] Tickets.\r\nFor testing we print 1.", "GamePro Print Tickets");
                    //   USE THIS  gameProSerial.Write("TIC0" + MainWindow.AppSet.intTickets.ToString() + LF);
                    gameProSerial.Write("TIC00" + LF);
                    MainWindow.AppSet.intTickets -= MainWindow.AppSet.intTickets;
                    intTickets -= intTickets;
                    //MessageBox.Show("Printing Tickets (intTickets)#: \r\nTIC0" + intTickets.ToString(), "GamePro Print Tickets");
                    //MessageBox.Show("Printing Tickets (AppSet.intTickets)#: \r\nTIC0" + AppSet.intTickets.ToString(), "GamePro Print Tickets");
                    
                } else {
                    MessageBox.Show("Would Print: TIC [" + MainWindow.AppSet.intTickets.ToString() + "] Tickets.\r\nFor testing we print 2.", "GamePro Print Tickets");
                    //   USE THIS  gameProSerial.Write("TIC" + MainWindow.AppSet.intTickets.ToString() + LF);
                    gameProSerial.Write("TIC01" + LF);
                    MainWindow.AppSet.intTickets -= MainWindow.AppSet.intTickets;
                    intTickets -= intTickets;
                    //MessageBox.Show("Printing Tickets (intTickets)#: \r\nTIC" + intTickets.ToString(), "GamePro Print Tickets");
                    //MessageBox.Show("Printing Tickets (AppSet.intTickets)#: \r\nTIC" + AppSet.intTickets.ToString(), "GamePro Print Tickets");
                    System.Threading.Thread.Sleep(4200);
                }
            }
        }
        #endregion

        #region UI Handlers
        /// <summary>
        /// Mouse Down Event Handler
        /// Strokes Game Play Again Background
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void ResultScreenBg_MouseDown(object sender, MouseButtonEventArgs e) {
            if ((!AppSet.AttractMode) && (!AppSet.isOn)) {
                lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
            }
            // Set UI Value for player interest to start another game
            // Ability to play again depends on tokens deposited and current transition phase
            //boolPlayAgain = false;
            //AppSet.boolPrint = true;
            ////while (!AppSet.boolPrint) {
            //if (intScore > 0) {
            //    AppSet.intCredits = intCredits;
            //    AppSet.intScore = intScore;
            //    AppSet.intTickets = intTickets;
            //    lbCredits.Content = "\r\n" + AppSet.intCredits.ToString() + @" / " + AppSet.intTokensMin.ToString();
            //    lbScore.Content = Convert.ToString(AppSet.intScore);
            //    lbTickets.Content = Convert.ToString(AppSet.intTickets);
            //    PrintTickets();
            //} else {
            //    MessageBox.Show("Strokes Shell Game is not passing the Score.", "Check the Desktop for the XML File to Verify.");
            //}
            //}
            //3
            //AppSet.AttractMode = true;
            //AppSet.firsttime = true;
            //AppSet.isOn = false;
            //isOn = AppSet.isOn;
            //firsttime = AppSet.firsttime;
            //AppSet.boolPrint = true;
            imgResultScreenBg.Cursor = Cursors.None;
            lbCredits.Visibility = Visibility.Visible;
            //imgInsertCreditBtn.Visibility = Visibility.Hidden;
            PrintTickets();
            
        }
        /// <summary>
        /// Mouse Down Event Handler for Insert Credit Button
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void imgInsertCreditBtn_MouseDown(object sender, MouseButtonEventArgs e) {
            intCredits++;
            //MessageBox.Show("intCredts #: " + intCredits.ToString(), "Insert Credit Button MouseDown");
            AppSet.intCredits = intCredits;
            //MessageBox.Show("AppSet.intCredts #: " + AppSet.intCredits.ToString(), "Insert Credit Button MouseDown");
            MainWindow.AppSet.intCredits = AppSet.intCredits;
            //MessageBox.Show("MainWindow.AppSet.intCredts #: " + MainWindow.AppSet.intCredits.ToString(), "Insert Credit Button MouseDown");
            //StartGameBtn_MouseDown(sender, e);
            lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
            if (MainWindow.AppSet.intCredits >= AppSet.intTokensMin) {
                imgInsertCreditBtn.Visibility = Visibility.Hidden;
                imgStartGameBtn.Visibility = Visibility.Visible;
                AppSet.AttractMode = false;
            }
        }
        /// <summary>
        /// AttractMode Background MouseDown Click Event Handler.
        /// Strokes Game Opening Media before StartGame MediaPlayer Game Media
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void mpAttractMode_MouseDown(object sender, MouseButtonEventArgs e) {
            if (AppSet.intCredits <= 0) {
                if (MainWindow.AppSet.intCredits > 0) {
                    AppSet.intCredits = MainWindow.AppSet.intCredits;
                }
                if (intCredits > 0) {
                    AppSet.intCredits = intCredits;
                }
            }
            mpAttractMode.Visibility = Visibility.Hidden;
            mpStartScreenMusic.LoadedBehavior = MediaState.Play;
            mpStartScreenMusic.Position = TimeSpan.Zero;
            //
            //10
            mpStartScreen.Visibility = Visibility.Visible;
            mpStartScreen.LoadedBehavior = MediaState.Play;
            mpStartScreen.BringIntoView();
            AppSet.isOn = false;
            isOn = AppSet.isOn;
            if (AppSet.intCredits >= AppSet.intTokensMin) {
                imgStartGameBtn.Visibility = Visibility.Visible;
                imgInsertCreditBtn.Visibility = Visibility.Hidden;
                lbCredits.Visibility = Visibility.Visible;
            } else {
                imgInsertCreditBtn.Visibility = Visibility.Visible;
                imgInsertCreditBtn.Cursor = Cursors.None;
                imgStartGameBtn.Visibility = Visibility.Hidden;
                lbCredits.Visibility = Visibility.Visible;
                lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + @" / " + AppSet.intTokensMin.ToString() + ")";
            }
            lbCredits.Cursor = Cursors.None;
            AppSet.AttractMode = false;
            if ((!AppSet.AttractMode) && (!AppSet.isOn)) {
                lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
            }
        }

        /// <summary>
        /// Mouse Down Event Handler
        /// Strokes Start Screen Ended Media
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void mpStartScreen_MouseDown(object sender, MouseButtonEventArgs e) {
            //lbCredits.Content = "\r\n" + intCredits.ToString() + @" / " + AppSet.intTokensMin.ToString();
        }

        /// <summary>
        /// Mouse Down Event Handler
        /// imgStartGameBtn Strokes Start Game Button
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void StartGameBtn_MouseDown(object sender, MouseButtonEventArgs e) {
            if (intCredits <= 0) {
                if (MainWindow.AppSet.intCredits > 0) {
                    intCredits = MainWindow.AppSet.intCredits;
                }
                if (AppSet.intCredits > 0) {
                    intCredits = AppSet.intCredits;
                }
            }
            AppSet.intCredits -= AppSet.intTokensMin;
            imgResultScreenBg.Visibility = Visibility.Visible;
            imgPlayAgainBg.Visibility = Visibility.Visible;
            AppSet.AttractMode = false;
            if ((!AppSet.AttractMode) && (!AppSet.isOn)) {
                lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
            }
            AppSet.isOn = true;
            isOn = AppSet.isOn;
            AppSet.firsttime = true;
            firsttime = AppSet.firsttime;
            mpStartScreen.LoadedBehavior = MediaState.Stop;
            mpStartScreenMusic.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(mpStartScreenMusic.Volume, 0, TimeSpan.FromSeconds(2)));
            imgPlayAgainBg.Visibility = Visibility.Visible;
            imgPlayAgainBg.BringIntoView();
        }
        #endregion

        #region MediaPlayer Handlers
        /// <summary>
        /// Media Ended Event Handler
        /// mpAttractMode Strokes Opening Media
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void mpAttractMode_MediaEnded(object sender, RoutedEventArgs e) {
            //6
            if (AppSet.AttractMode) {
                //lbCredits.Visibility = Visibility.Hidden;
                imgStartGameBtn.Visibility = Visibility.Hidden;
                imgInsertCreditBtn.Visibility = Visibility.Hidden;

                //5
                imgStartGameBtn.Visibility = Visibility.Collapsed;
                mpStartScreenMusic.LoadedBehavior = MediaState.Stop;
                //mpStartScreen.LoadedBehavior = MediaState.Stop;
                mpStartScreen.Visibility = Visibility.Hidden;
                mpAttractMode.Position = TimeSpan.Zero;
                //
            }
            if (!AppSet.AttractMode) {
                lbCredits.Visibility = Visibility.Visible;
                if (!AppSet.isOn) {
                    lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                }
                mpStartScreenMusic.LoadedBehavior = MediaState.Play;
                mpStartScreen.Visibility = Visibility.Visible;
                if (MainWindow.AppSet.intCredits >= AppSet.intTokensMin) {
                    imgStartGameBtn.Visibility = Visibility.Visible;
                    imgInsertCreditBtn.Visibility = Visibility.Hidden;
                }
                if (MainWindow.AppSet.intCredits < AppSet.intTokensMin) {
                    imgStartGameBtn.Visibility = Visibility.Hidden;
                    imgInsertCreditBtn.Visibility = Visibility.Visible;
                }

            }
        }

        /// <summary>
        /// Strokes Media Fairytale MP3 Ended Event Handler
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void mpStartScreenMusic_MediaEnded(object sender, RoutedEventArgs e) {
            if (!AppSet.AttractMode) {
                //mpStartScreenMusic.Position = TimeSpan.Zero;
                //mpStartScreenMusic.LoadedBehavior = MediaState.Play;
            } else {
                
            }
        }
        /// <summary>
        /// Media Ended Event Handler
        /// Strokes Start Screen Ended Media
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void mpStartScreen_MediaEnded(object sender, RoutedEventArgs e) {
            //MessageBox.Show("Before !AppSet.AttractMode", "mpStartScreen_MediaEnded");
            if (!AppSet.AttractMode) {
                if (!AppSet.isOn) {
                    lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                }
                //AppSet.AttractMode = true;
                //MessageBox.Show("Is !AppSet.AttractMode", "mpStartScreen_MediaEnded");
                mpStartScreen.Position = TimeSpan.Zero;
                //mpStartScreenMusic.Position = TimeSpan.Zero;
                if (MainWindow.AppSet.intCredits >= AppSet.intTokensMin) {
                    //MessageBox.Show("Is !AppSet.AttractMode" + "\r\n" + "(MainWindow.AppSet.intCredits >= AppSet.intTokensMin)", "mpStartScreen_MediaEnded");
                    imgStartGameBtn.Visibility = Visibility.Visible;
                    lbCredits.Visibility = Visibility.Visible;
                    imgInsertCreditBtn.Visibility = Visibility.Hidden;
                }
                if (MainWindow.AppSet.intCredits < AppSet.intTokensMin) {
                    //MessageBox.Show("Is !AppSet.AttractMode" + "\r\n" + "(MainWindow.AppSet.intCredits < AppSet.intTokensMin)", "mpStartScreen_MediaEnded");
                    imgStartGameBtn.Visibility = Visibility.Hidden;
                    lbCredits.Visibility = Visibility.Visible;
                    imgInsertCreditBtn.Visibility = Visibility.Visible;
                }
                if (AppSet.isOn) {
                    //MessageBox.Show("Is !AppSet.AttractMode" + "\r\n" + "(AppSet.isOn)", "mpStartScreen_MediaEnded");
                    AppSet.AttractMode = false;
                    //AppSet.firsttime = false;
                    //firsttime = AppSet.firsttime;
                    //imgResultScreenBg.Visibility = Visibility.Visible;
                    //imgPlayAgainBg.Visibility = Visibility.Visible;
                    //mpStartScreen.Visibility = Visibility.Collapsed;
                    //imgResultScreenBg.BringIntoView();
                    lbCredits.Visibility = Visibility.Visible;
                }
                if (!AppSet.isOn) {
                    if (!AppSet.AttractMode) {
                        lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                    }
                    //MessageBox.Show("Is !AppSet.AttractMode" + "\r\n" + "(!AppSet.isOn)", "mpStartScreen_MediaEnded");
                    mpStartScreen.LoadedBehavior = MediaState.Play;
                    //
                    //8
                    if (MainWindow.AppSet.intCredits < AppSet.intTokensMin) {
                        AppSet.AttractMode = true;
                        imgStartGameBtn.Visibility = Visibility.Hidden;
                        imgInsertCreditBtn.Visibility = Visibility.Hidden;
                        lbCredits.Visibility = Visibility.Hidden;


                        //mpStartScreen.LoadedBehavior = MediaState.Stop;
                        //mpStartScreen.Visibility = Visibility.Collapsed;
                        //
                        //7
                        mpAttractMode.Visibility = Visibility.Visible;
                        mpAttractMode.LoadedBehavior = MediaState.Play;
                        mpAttractMode.BringIntoView();
                        mpAttractMode.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(mpAttractMode.Opacity, 1, TimeSpan.FromSeconds(2)));
                        mpAttractMode.BeginAnimation(MediaElement.VolumeProperty, new DoubleAnimation(mpAttractMode.Volume, 1, TimeSpan.FromSeconds(2)));
                    }
                }
            }
            //MessageBox.Show("Else (!AppSet.AttractMode)", "mpStartScreen_MediaEnded");
        }
        #endregion

        #region ApplicationSettings Class Methods and Calls to Load/Save/etc.
        /// <summary>
        /// ApplicationSettings Class Returns.  Requires Read and Write Priviledges in Home Folder and valid JSON FSO
        /// </summary>
        /// <param name="filename">String</param>
        /// <returns>Object</returns>
        public ApplicationSettings Load(string filename) {
            
            //
            // Clear out any ApplicationSettings previously recorded/stored in memory
            var directory = GetSettingsDirectory();
            //
            // Check to see if directory exists and if not create it
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            //
            // Set Full Path of Settings File System Object (Path and Filename)
            strFSOpathSettings = Convert.ToString(directory + filename);
            //
            // Check to see if file exists and if not create it
            if (!File.Exists(strFSOpathSettings)) {
                File.WriteAllText(strFSOpathSettings, "");
            }
            //
            // Load JSON File and Parse
            string fileData = File.ReadAllText(strFSOpathSettings);
            scorePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\StrokesScores.xml";
            AppSet.scorePath = scorePath;
            AppSet.strFSOpathGameExe = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\Strokes.exe";
            //
            // Setting Default Values:
            AppSet.TimeofResultScreen = 5;
            AppSet.TimeofResultScreen = Convert.ToInt32(AppSet.TimeofResultScreen);
            AppSet.firsttime = false;
            AppSet.firsttime = Convert.ToBoolean(AppSet.firsttime);
            AppSet.boolPlayAgain = true;
            AppSet.boolPlayAgain = Convert.ToBoolean(AppSet.boolPlayAgain);
            AppSet._isSerialAlive = false;
            AppSet._isSerialAlive = Convert.ToBoolean(AppSet._isSerialAlive);
            AppSet.resultTimer = 7;
            AppSet.resultTimer = Convert.ToInt32(AppSet.resultTimer);
            AppSet.cycle = 4;
            AppSet.cycle = Convert.ToInt32(AppSet.cycle);

            AppSet.strFSOpathSettings = Convert.ToString(AppSet.strFSOpathSettings);
            AppSet.playAgainTimer1 = 7;
            AppSet.playAgainTimer1 = Convert.ToInt32(AppSet.playAgainTimer1);
            AppSet.RatioConfig = 0.0005;
            AppSet.RatioConfig = Convert.ToDouble(AppSet.RatioConfig);
            AppSet.Ratio = 0.0005;
            AppSet.Ratio = Convert.ToDouble(AppSet.Ratio);
            AppSet.isOn = false;
            AppSet.isOn = Convert.ToBoolean(isOn);

            AppSet.StrokesJSONfilename = @"\Strokes.json";
            AppSet.StrokesJSONfilename = Convert.ToString(AppSet.StrokesJSONfilename);
            string strBWDpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var attrs = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            strBWDpath = System.IO.Path.Combine(strBWDpath, ((AssemblyCompanyAttribute)attrs[0]).Company);
            AppSet.BaseWorkingDirectory = Convert.ToString(strBWDpath);
            attrs = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            strBWDpath = System.IO.Path.Combine(strBWDpath, ((AssemblyProductAttribute)attrs[0]).Product);
            AppSet.StrokesWorkingDirectory = strBWDpath;
            AppSet.strFSOpathSettings = Convert.ToString(strBWDpath + AppSet.StrokesJSONfilename);
            strFSOpathSettings = AppSet.strFSOpathSettings;
            AppSet.ExtraFileExtensions = Convert.ToString("");
            AppSet.GalleryCreationSubCategory = Convert.ToString("");
            AppSet.GameNameFormat = Convert.ToString("");
            AppSet.NextUpdateCheck = new DateTime();
            AppSet.strGameFSOfile = AppSet.strFSOpathGameExe;
            //
            // Check if file exists before jumping to define XML FSOs
            // This area should be given lots of attention for encoding, security and exploit vulnerability
            if (File.Exists(scorePath)) {
                AppSet.strFSOpathScore = scorePath.ToString();
                AppSet.scorePath = Convert.ToString(scorePath);
            }
            //
            // Check if file exists before jumping to define EXE FSOs
            // This area should be given lots of attention for encoding, security and exploit vulnerability
            if (File.Exists(strFSOpathGameExe)) {
                AppSet.strFSOpathGameExe = strFSOpathGameExe.ToString();
                AppSet.strFSOpathGameExe = Convert.ToString(strFSOpathGameExe);
            }

            AppSet.intOutOfBoundsPenalty = Convert.ToInt32(0);
            AppSet.intGameCompletePercent = Convert.ToInt32(10);
            AppSet.intGameBrushSize = Convert.ToInt32(1);
            AppSet.intLeaderboardTimeout = Convert.ToInt32(5);
            AppSet.intTutorialTimeout = Convert.ToInt32(5);
            AppSet.intTokensMin = Convert.ToInt32(1);
            AppSet.intScoreTickets01 = Convert.ToInt32(100);
            AppSet.intScoreTickets02 = Convert.ToInt32(200);
            AppSet.intScoreTickets03 = Convert.ToInt32(300);
            AppSet.intScoreTickets04 = Convert.ToInt32(400);
            AppSet.intScoreTickets05 = Convert.ToInt32(500);
            AppSet.intScoreTickets06 = Convert.ToInt32(600);
            AppSet.intScoreTickets07 = Convert.ToInt32(700);
            AppSet.intScoreTickets08 = Convert.ToInt32(800);
            AppSet.intScoreTickets09 = Convert.ToInt32(900);
            AppSet.intScoreTickets10 = Convert.ToInt32(999);
            AppSet.intCredits = Convert.ToInt32(0);

            AppSet = MainWindow.AppSet;            
            //
            // Used to call the old AppSet object, now calling MainWindow.AppSet object (testing)
            Save(MainWindow.AppSet, MainWindow.AppSet.strFSOpathSettings);                                                    // Save Initialized Class called "settings" to File System
            //varAppSet = AppSet;
            //}
            //
            // Return either new ApplicationSettings values or the default setting values
            return MainWindow.AppSet;
        }

        /// <summary>
        /// A predictable path for storing settings.
        /// Easy to backup for users and document.
        /// Uses AssemblyCompany attribute and the AssemblyProduct attribute as folder location.
        /// </summary>
        /// <returns>String</returns>
        public string GetSettingsDirectory() {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //System.Windows.Forms.MessageBox.Show("APPLICATION DATA:" + path.ToString(), "GetSettingsDrect Paths");
            var attrs = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            //System.Windows.Forms.MessageBox.Show("ASSEMBLYATTRIBUTES:" + attrs.ToString(), "GetSettingsDrect Paths");
            if (attrs.Length == 1) {
                path = System.IO.Path.Combine(path, ((AssemblyCompanyAttribute)attrs[0]).Company);
                //System.Windows.Forms.MessageBox.Show("ASSEMBLYCOMPANY:" + path.ToString(), "GetSettingsDrect Paths");
            }
            attrs = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attrs.Length == 1) {
                path = System.IO.Path.Combine(path, ((AssemblyProductAttribute)attrs[0]).Product);
                //System.Windows.Forms.MessageBox.Show("ASSEMBLYPRODUCT:" + path.ToString(), "GetSettingsDrect Paths");
            }
            //System.Windows.Forms.MessageBox.Show(path, "Strokes JSON Settings | Local Storage Location");

            return path;
        }

        /// <summary>
        /// SerializeObject method returns a string which is then written to a file using a StreamWriter.
        /// Requires Write Priviledges in Home Folder
        /// </summary>
        /// <param name="settings">ApplicationSettings Object</param>
        /// <param name="filename">String</param>
        public void Save(ApplicationSettings settingsSave, string filename) {
            Debug.Assert(settingsSave != null);
            var directory = GetSettingsDirectory();
            //var directory = StrokesWorkingDirectory;
            //var path = System.IO.Path.Combine(directory, filename);
            var path = System.IO.Path.Combine(directory, filename);
            //JsonConvert.SerializeObject(AppSet);
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            try {
                using (StreamWriter writer = File.CreateText(settingsSave.strFSOpathSettings)) {
                    //System.Windows.Forms.MessageBox.Show(settingsSave.strFSOpathSettings + settingsSave.StrokesJSONfilename.ToString(), "JSON DATA | Local Storage Location");
                    string fileDataSaveJSON = JsonConvert.SerializeObject(settingsSave, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.All,
                        ObjectCreationHandling = ObjectCreationHandling.Auto,
                        MaxDepth = 2,
                    });
                    //System.Windows.Forms.MessageBox.Show("OBJ_STROKES_DEMO:   " + fileData.ToString(), "JSON DATA | Local Storage Settings");
                    writer.Write(fileDataSaveJSON);
                    writer.Close();
                }
            } catch (Exception saveEx) {
                System.Windows.Forms.MessageBox.Show(saveEx.Message + saveEx.InnerException, "ERROR: ApplicationSettings Save Method");
            }
        }


        //
        // Strokes ApplicationSettings Event Handlers
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string caller) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
        private void AppSet_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            //Console.WriteLine("A property has changed: " + e.PropertyName);
            RaisePropertyChanged(e.PropertyName);
        }
        /// <summary>
        /// A starter method for saving Default Values to ApplicationSettings JSON configuration.
        /// </summary>
        /// <param name="settings">ApplicationSettings</param>
        public void SetDefaults(ApplicationSettings settingsDefaults) {
            MainWindow.AppSet.PropertyChanged += new PropertyChangedEventHandler(AppSet_PropertyChanged);
            //
            // Forked from saving StrokesScores as XML on Desktop to use the new WorkingDirectory instead
            //
            // Changed obsolete Namespace ConfigurationSettings to ConfigurationManager
            //settingsDefaults.TimeofResultScreen = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["TimeofResultScreen"]);
            //settingsDefaults.playAgainTimer1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["playAgainTimer"]);
            //settingsDefaults.RatioConfig = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["ratio"]);
            //
            // Forked from XML .NET ConfigurationManager to use this ApplicationSettings JSON Class instead
            settingsDefaults.TimeofResultScreen = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimeofResultScreen"]);
            settingsDefaults.playAgainTimer1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["playAgainTimer"]);
            settingsDefaults.RatioConfig = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["ratio"]);
            settingsDefaults.strFSOpathSettings = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + settingsDefaults.StrokesJSONfilename.ToString();
            settingsDefaults.scorePath = settingsDefaults.BaseWorkingDirectory + @"\StrokesScores.xml";
            //
            // Object => settingsDefaults Properties 
            settingsDefaults.scorePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\StrokesScores.xml";
            settingsDefaults.strFSOpathGameExe = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\Strokes.exe";
            ////
            //// Setting Default Values:
            settingsDefaults.TimeofResultScreen = 5;
            settingsDefaults.TimeofResultScreen = Convert.ToInt32(settingsDefaults.TimeofResultScreen);
            settingsDefaults.firsttime = false;
            settingsDefaults.firsttime = Convert.ToBoolean(settingsDefaults.firsttime);
            settingsDefaults.boolPlayAgain = false;
            settingsDefaults.boolPlayAgain = Convert.ToBoolean(settingsDefaults.boolPlayAgain);
            settingsDefaults._isSerialAlive = false;
            settingsDefaults._isSerialAlive = Convert.ToBoolean(settingsDefaults._isSerialAlive);
            settingsDefaults.resultTimer = 7;
            settingsDefaults.resultTimer = Convert.ToInt32(settingsDefaults.resultTimer);
            settingsDefaults.cycle = 4;
            settingsDefaults.cycle = Convert.ToInt32(settingsDefaults.cycle);
            settingsDefaults.StrokesJSONfilename = @"\Strokes.json";
            settingsDefaults.StrokesJSONfilename = Convert.ToString(settingsDefaults.StrokesJSONfilename);
            settingsDefaults.strFSOpathSettings = strFSOpathSettings;
            settingsDefaults.strFSOpathSettings = Convert.ToString(settingsDefaults.strFSOpathSettings);
            settingsDefaults.playAgainTimer1 = 7;
            settingsDefaults.playAgainTimer1 = Convert.ToInt32(settingsDefaults.playAgainTimer1);
            settingsDefaults.RatioConfig = 0.002;
            settingsDefaults.RatioConfig = Convert.ToDouble(settingsDefaults.RatioConfig);
            settingsDefaults.Ratio = 0.002;
            settingsDefaults.Ratio = Convert.ToDouble(settingsDefaults.Ratio);
            settingsDefaults.isOn = false;
            settingsDefaults.isOn = Convert.ToBoolean(settingsDefaults.isOn);
            settingsDefaults.StrokesWorkingDirectory = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek";
            settingsDefaults.BaseWorkingDirectory = strFSOpathSettings;
            ////
            //// Check if file exists before jumping to define XML FSOs
            //// This area should be given lots of attention for encoding, security and exploit vulnerability
            if (File.Exists(settingsDefaults.scorePath)) {
                settingsDefaults.strFSOpathScore = settingsDefaults.scorePath.ToString();
                settingsDefaults.scorePath = Convert.ToString(settingsDefaults.scorePath);
            } else {
                System.Windows.Forms.MessageBox.Show("XML File Does Not Exist:" + scorePath.ToString(), "ERROR: Strokes Score Path");
            }
            ////
            //// Check if file exists before jumping to define EXE FSOs
            //// This area should be given lots of attention for encoding, security and exploit vulnerability
            if (File.Exists(settingsDefaults.strFSOpathGameExe)) {
                settingsDefaults.strFSOpathGameExe = settingsDefaults.strFSOpathGameExe.ToString();
                strFSOpathGameExe = Convert.ToString(settingsDefaults.strFSOpathGameExe);
            } else {
                System.Windows.Forms.MessageBox.Show("EXE File Does Not Exist:" + settingsDefaults.strFSOpathGameExe.ToString(), "ERROR: Strokes Score Path");
            }
            settingsDefaults.scorePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\StrokesScores.xml";
            MainWindow.scorePath = scorePath;
            settingsDefaults.strFSOpathGameExe = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\Strokes.exe";
            //
            // Setting Default Values:
            settingsDefaults.TimeofResultScreen = 5;
            MainWindow.TimeofResultScreen = Convert.ToInt32(TimeofResultScreen);
            settingsDefaults.firsttime = false;
            MainWindow.firsttime = Convert.ToBoolean(firsttime);
            settingsDefaults.boolPlayAgain = false;
            MainWindow.boolPlayAgain = Convert.ToBoolean(boolPlayAgain);
            settingsDefaults._isSerialAlive = false;
            settingsDefaults._isSerialAlive = Convert.ToBoolean(_isSerialAlive);
            MainWindow.resultTimer = 7;
            settingsDefaults.resultTimer = Convert.ToInt32(resultTimer);
            settingsDefaults.cycle = 1;
            MainWindow.cycle = Convert.ToInt32(cycle);
            settingsDefaults.StrokesJSONfilename = @"\Strokes.json";
            settingsDefaults.StrokesJSONfilename = Convert.ToString(StrokesJSONfilename);
            settingsDefaults.strFSOpathSettings = Convert.ToString(strFSOpathSettings);
            settingsDefaults.playAgainTimer1 = 7;
            settingsDefaults.playAgainTimer1 = Convert.ToInt32(playAgainTimer1);
            settingsDefaults.RatioConfig = 0.002;
            settingsDefaults.RatioConfig = Convert.ToDouble(RatioConfig);
            settingsDefaults.Ratio = 0.002;
            settingsDefaults.Ratio = Convert.ToDouble(Ratio);
            settingsDefaults.isOn = false;
            settingsDefaults.isOn = Convert.ToBoolean(isOn);
            settingsDefaults.StrokesWorkingDirectory = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek";
            settingsDefaults.BaseWorkingDirectory = Convert.ToString(strFSOpathSettings);
            settingsDefaults.ExtraFileExtensions = Convert.ToString("");
            settingsDefaults.GalleryCreationSubCategory = Convert.ToString("");
            settingsDefaults.GameNameFormat = Convert.ToString("");
            settingsDefaults.NextUpdateCheck = new DateTime();
            settingsDefaults.intCredits = Convert.ToInt32(0);
            settingsDefaults.intTickets = Convert.ToInt32(0);
            settingsDefaults.intScore = Convert.ToInt32(0);
            settingsDefaults.IsAttractMode = Convert.ToBoolean(true);
            settingsDefaults.IsPlayAgainMode = Convert.ToBoolean(false);
            settingsDefaults.IsRewardsMode = Convert.ToBoolean(false);
            settingsDefaults.isOn = Convert.ToBoolean(false);
            settingsDefaults.TokenReady = Convert.ToBoolean(true);
            settingsDefaults.intScoreTickets01 = Convert.ToInt32(100);
            settingsDefaults.intScoreTickets02 = Convert.ToInt32(200);
            settingsDefaults.intScoreTickets03 = Convert.ToInt32(300);
            settingsDefaults.intScoreTickets04 = Convert.ToInt32(400);
            settingsDefaults.intScoreTickets05 = Convert.ToInt32(500);
            settingsDefaults.intScoreTickets06 = Convert.ToInt32(600);
            settingsDefaults.intScoreTickets07 = Convert.ToInt32(700);
            settingsDefaults.intScoreTickets08 = Convert.ToInt32(800);
            settingsDefaults.intScoreTickets09 = Convert.ToInt32(900);
            settingsDefaults.intScoreTickets10 = Convert.ToInt32(999);
            settingsDefaults.intOutOfBoundsPenalty = Convert.ToInt32(0);
            settingsDefaults.intGameCompletePercent = Convert.ToInt32(10);
            settingsDefaults.intGameBrushSize = Convert.ToInt32(1);
            settingsDefaults.intLeaderboardTimeout = Convert.ToInt32(5);
            settingsDefaults.intTutorialTimeout = Convert.ToInt32(5);
            settingsDefaults.intTokensMin = Convert.ToInt32(1);


            //
            // Check if file exists before jumping to define XML FSOs
            // This area should be given lots of attention for encoding, security and exploit vulnerability
            if (File.Exists(scorePath)) {
                settingsDefaults.strFSOpathScore = scorePath.ToString();
                settingsDefaults.scorePath = Convert.ToString(scorePath);
            } else {
                //System.Windows.Forms.MessageBox.Show("XML File Does Not Exist:" + scorePath.ToString(), "ERROR: Strokes Score Path");
            }
            //
            // Check if file exists before jumping to define EXE FSOs
            // This area should be given lots of attention for encoding, security and exploit vulnerability
            if (File.Exists(strFSOpathGameExe)) {
                settingsDefaults.strFSOpathGameExe = strFSOpathGameExe.ToString();
                settingsDefaults.strFSOpathGameExe = Convert.ToString(strFSOpathGameExe);
            } else {
                //System.Windows.Forms.MessageBox.Show("EXE File Does Not Exist:" + settingsDefaults.strFSOpathGameExe.ToString(), "ERROR: Strokes Score Path");
            }
            //
            // Read Strokes Game Settings XML Configuration into ApplicationSettings
            settingsDefaults.strFSOpathGameXML = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\gameSettings.xml";
            if (File.Exists(settingsDefaults.strFSOpathGameXML)) {
                //
                // Create XmlReader to read Strokes Game XML Settings and set runtime properties in case we need them or need to change them in the XML file within the project
                try {
                    //
                    // Forked from using scorePath instead using strFSOpathScore
                    //System.Windows.Forms.MessageBox.Show("Reading/Writing XML Data to this location:   " + scorePath.ToString(), "Strokes Scores: XML File");
                    using (XmlReader readerXMLgame = XmlReader.Create(settingsDefaults.strFSOpathGameXML)) {
                        while (readerXMLgame.Read()) {
                            if (readerXMLgame.Value.Contains('\n'))
                                continue;
                            switch (readerXMLgame.Name) {
                                case "totalGameTimeInMinutes":
                                    if (readerXMLgame.Read() && !String.IsNullOrWhiteSpace(readerXMLgame.Value))
                                        if (readerXMLgame.Value.Trim() != "\n") {
                                            settingsDefaults.totalGameTimeInMinutes = Convert.ToString(readerXMLgame.Value.Trim());
                                        }
                                    break;
                                case "highScoreTimeOut":
                                    if (readerXMLgame.Read() && !String.IsNullOrWhiteSpace(readerXMLgame.Value))
                                        if (readerXMLgame.Value.Trim() != "\n") {
                                            settingsDefaults.highScoreTimeOut = Convert.ToString(readerXMLgame.Value.Trim());
                                        }
                                    break;
                                case "startWithoutBgMusic":
                                    if (readerXMLgame.Read() && !String.IsNullOrWhiteSpace(readerXMLgame.Value))
                                        if (readerXMLgame.Value.Trim() != "\n") {
                                            settingsDefaults.startWithoutBgMusic = Convert.ToString(readerXMLgame.Value.Trim());
                                        }
                                    break;
                                case "sizeMult":
                                    if (readerXMLgame.Read() && !String.IsNullOrWhiteSpace(readerXMLgame.Value))
                                        if (readerXMLgame.Value.Trim() != "\n") {
                                            settingsDefaults.sizeMult = Convert.ToString(readerXMLgame.Value.Trim());
                                        }
                                    break;
                                case "coloredThresholdPercentage":
                                    if (readerXMLgame.Read() && !String.IsNullOrWhiteSpace(readerXMLgame.Value))
                                        if (readerXMLgame.Value.Trim() != "\n") {
                                            settingsDefaults.coloredThresholdPercentage = Convert.ToString(readerXMLgame.Value.Trim());
                                        }
                                    break;
                                case "outOfBoundsPenaltyMultiplier":
                                    if (readerXMLgame.Read() && !String.IsNullOrWhiteSpace(readerXMLgame.Value))
                                        if (readerXMLgame.Value.Trim() != "\n") {
                                            settingsDefaults.outOfBoundsPenaltyMultiplier = Convert.ToString(readerXMLgame.Value.Trim());
                                        }
                                    break;
                                case "startInFullScreen":
                                    if (readerXMLgame.Read() && !String.IsNullOrWhiteSpace(readerXMLgame.Value))
                                        if (readerXMLgame.Value.Trim() != "\n") {
                                            settingsDefaults.startInFullScreen = Convert.ToString(readerXMLgame.Value.Trim());
                                        }
                                    break;
                                case "gameOverTimeOutTimeInSeconds":
                                    if (readerXMLgame.Read() && !String.IsNullOrWhiteSpace(readerXMLgame.Value))
                                        if (readerXMLgame.Value.Trim() != "\n") {
                                            settingsDefaults.gameOverTimeOutTimeInSeconds = Convert.ToString(readerXMLgame.Value.Trim());
                                        }
                                    break;
                                case "outputScoreURL":
                                    if (readerXMLgame.Read() && !String.IsNullOrWhiteSpace(readerXMLgame.Value))
                                        if (readerXMLgame.Value.Trim() != "\n") {
                                            settingsDefaults.outputScoreURL = Convert.ToString(readerXMLgame.Value.Trim());
                                        }
                                    break;
                            }
                        }
                    }
                } catch (Exception exGameXML) {
                    System.Windows.MessageBox.Show(exGameXML.Message + exGameXML.InnerException);
                }
                MainWindow.AppSet = settingsDefaults;
                //
                // Forked from saving StrokesScores as XML on Desktop to use the new WorkingDirectory instead
                //
                // Changed obsolete Namespace ConfigurationSettings to ConfigurationManager
                //TimeofResultScreen = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["TimeofResultScreen"]);
                //playAgainTimer1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["playAgainTimer"]);
                //RatioConfig = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["ratio"]);
                //
                // Forked from XML .NET ConfigurationManager to use this ApplicationSettings JSON Class instead
                //TimeofResultScreen = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimeofResultScreen"]);
                //playAgainTimer1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["playAgainTimer"]);
                //RatioConfig = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["ratio"]);
                //settings.strFSOpathSettings = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + settings.StrokesJSONfilename.ToString();
                //scorePath = BaseWorkingDirectory + @"\StrokesScores.xml";
                //
                // Object => settingsDefaults Properties 
                //settingsDefaults.scorePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\StrokesScores.xml";
                //settingsDefaults.strFSOpathGameExe = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\Strokes.exe";
                ////
                //// Setting Default Values:
                //settingsDefaults.TimeofResultScreen = 5;
                //TimeofResultScreen = Convert.ToInt32(settingsDefaults.TimeofResultScreen);
                //settingsDefaults.firsttime = false;
                //firsttime = Convert.ToBoolean(settingsDefaults.firsttime);
                //settingsDefaults.boolPlayAgain = true;
                //boolPlayAgain = Convert.ToBoolean(settingsDefaults.boolPlayAgain);
                //settingsDefaults._isSerialAlive = false;
                //_isSerialAlive = Convert.ToBoolean(settingsDefaults._isSerialAlive);
                //settingsDefaults.resultTimer = 7;
                //resultTimer = Convert.ToInt32(settingsDefaults.resultTimer);
                //settingsDefaults.cycle = 4;
                //cycle = Convert.ToInt32(settingsDefaults.cycle);
                //settingsDefaults.StrokesJSONfilename = @"\Strokes.json";
                //StrokesJSONfilename = Convert.ToString(settingsDefaults.StrokesJSONfilename);
                //settingsDefaults.strFSOpathSettings = strFSOpathSettings;
                //strFSOpathSettings = Convert.ToString(settingsDefaults.strFSOpathSettings);
                //settingsDefaults.playAgainTimer1 = 7;
                //playAgainTimer1 = Convert.ToInt32(settingsDefaults.playAgainTimer1);
                //settingsDefaults.RatioConfig = 0.002;
                //RatioConfig = Convert.ToDouble(settingsDefaults.RatioConfig);
                //settingsDefaults.Ratio = 0.002;
                //Ratio = Convert.ToDouble(settingsDefaults.Ratio);
                //settingsDefaults.isOn = false;
                //isOn = Convert.ToBoolean(settingsDefaults.isOn);
                //settingsDefaults.StrokesWorkingDirectory = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek";
                //settingsDefaults.BaseWorkingDirectory = strFSOpathSettings;
                ////
                //// Check if file exists before jumping to define XML FSOs
                //// This area should be given lots of attention for encoding, security and exploit vulnerability
                //if (File.Exists(settingsDefaults.scorePath)) {
                //    settingsDefaults.strFSOpathScore = settingsDefaults.scorePath.ToString();
                //    scorePath = Convert.ToString(settingsDefaults.scorePath);
                //} else {
                //    System.Windows.Forms.MessageBox.Show("XML File Does Not Exist:" + scorePath.ToString(), "ERROR: Strokes Score Path");
                //}
                ////
                //// Check if file exists before jumping to define EXE FSOs
                //// This area should be given lots of attention for encoding, security and exploit vulnerability
                //if (File.Exists(settingsDefaults.strFSOpathGameExe)) {
                //    settingsDefaults.strFSOpathGameExe = settingsDefaults.strFSOpathGameExe.ToString();
                //    strFSOpathGameExe = Convert.ToString(settingsDefaults.strFSOpathGameExe);
                //} else {
                //    System.Windows.Forms.MessageBox.Show("EXE File Does Not Exist:" + settingsDefaults.strFSOpathGameExe.ToString(), "ERROR: Strokes Score Path");
                //}
                settingsDefaults.scorePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\StrokesScores.xml";
                settingsDefaults.scorePath = scorePath;
                strFSOpathGameExe = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\Strokes.exe";
                //
                // Setting Default Values:
                //TimeofResultScreen = 5;
                settingsDefaults.TimeofResultScreen = Convert.ToInt32(TimeofResultScreen);
                //firsttime = false;
                settingsDefaults.firsttime = Convert.ToBoolean(firsttime);
                //boolPlayAgain = true;
                settingsDefaults.boolPlayAgain = Convert.ToBoolean(boolPlayAgain);
                //_isSerialAlive = false;
                settingsDefaults._isSerialAlive = Convert.ToBoolean(_isSerialAlive);
                //resultTimer = 7;
                settingsDefaults.resultTimer = Convert.ToInt32(resultTimer);
                //cycle = 4;
                settingsDefaults.cycle = Convert.ToInt32(cycle);
                settingsDefaults.StrokesJSONfilename = @"\Strokes.json";
                settingsDefaults.StrokesJSONfilename = Convert.ToString(StrokesJSONfilename);
                settingsDefaults.strFSOpathSettings = Convert.ToString(strFSOpathSettings);
                //playAgainTimer1 = 7;
                settingsDefaults.playAgainTimer1 = Convert.ToInt32(playAgainTimer1);
                settingsDefaults.RatioConfig = 0.002;
                settingsDefaults.RatioConfig = Convert.ToDouble(RatioConfig);
                settingsDefaults.Ratio = 0.002;
                settingsDefaults.Ratio = Convert.ToDouble(Ratio);
                //isOn = false;
                settingsDefaults.isOn = Convert.ToBoolean(isOn);
                // StrokesWorkingDirectory = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek";
                // BaseWorkingDirectory = Convert.ToString(strFSOpathSettings);
                // ExtraFileExtensions = Convert.ToString("");
                // GalleryCreationSubCategory = Convert.ToString("");
                // GameNameFormat = Convert.ToString("");
                // NextUpdateCheck = new DateTime();
                //
                // Check if file exists before jumping to define XML FSOs
                // This area should be given lots of attention for encoding, security and exploit vulnerability
                if (File.Exists(settingsDefaults.scorePath)) {
                    settingsDefaults.strFSOpathScore = settingsDefaults.scorePath.ToString();
                    settingsDefaults.scorePath = Convert.ToString(settingsDefaults.scorePath);
                } else {
                    //System.Windows.Forms.MessageBox.Show("XML File Does Not Exist:" + scorePath.ToString(), "ERROR: Strokes Score Path");
                }
                //
                // Check if file exists before jumping to define EXE FSOs
                // This area should be given lots of attention for encoding, security and exploit vulnerability
                if (File.Exists(settingsDefaults.strFSOpathGameExe)) {
                    settingsDefaults.strFSOpathGameExe = settingsDefaults.strFSOpathGameExe.ToString();
                    settingsDefaults.strFSOpathGameExe = Convert.ToString(settingsDefaults.strFSOpathGameExe);
                } else {
                    //System.Windows.Forms.MessageBox.Show("EXE File Does Not Exist:" + settingsDefaults.strFSOpathGameExe.ToString(), "ERROR: Strokes Score Path");
                }
            }
        }// End Public Partial Class MainWindow
        #endregion

        #region Main
        /// <summary>
        /// MainWindow Initialization
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            MainWindow.AppSet = new ApplicationSettings();
            ApplicationSettings AppSet = new ApplicationSettings();
            DataContext = new CreditsViewModel();
            CreditsViewModel CreditsViewM = new CreditsViewModel();
            //gameProSerial.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceivedHandler);
            
            //DataContext = new ApplicationSettings();
            //MainWindow.AppSet.SetDefaults(AppSet);
            //AppSet.SetDefaults(MainWindow.AppSet);
            var attrs1 = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            strCompany = ((AssemblyCompanyAttribute)attrs1[0]).Company.ToString();
            attrs1 = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            strProduct = ((AssemblyProductAttribute)attrs1[0]).Product.ToString();
            attrs1 = null;
            strVersion = @"v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
            strVersion = strVersion + "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
            strVersion = strVersion + "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
            strVersion = strVersion + "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();
            if (!String.IsNullOrWhiteSpace(strVersion)) {
                lbVersion.Content = Convert.ToString(strCompany + " " + strProduct + @" Demo  " + strVersion);
            } else {
                lbVersion.Content = Convert.ToString(@" Demo " + "Something Weird Happened!");
            }
            //DataContext = new PriatekGameViewModel();
            if (!String.IsNullOrWhiteSpace(strFSOpathSettings)) {
                AppSet.strFSOpathSettings = strFSOpathSettings;
                Load(strFSOpathSettings);
            } else {
                //string strDrectStrokesJSON = GetSettingsDirectory();
                strFSOpathSettings = @"\Strokes.json";
                AppSet.strFSOpathSettings = strFSOpathSettings;
                Load(MainWindow.AppSet.strFSOpathSettings);
            }
            //
            //11
            AppSet.isOn = false;
            isOn = AppSet.isOn;
            AppSet.boolPrint = false;
            // Moved to ApplicationSettings Class
            //
            // Sync Manually the ResultScreen Timer
            //TimeofResultScreen = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimeofResultScreen"]);
            TimeofResultScreen = AppSet.TimeofResultScreen;
            AppSet.resultTimer = TimeofResultScreen;
            resultTimer = AppSet.resultTimer;
            //
            // Sync Manually the PlayAgainTimer
            //playAgainTimer1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["playAgainTimer"]);
            playAgainTimer = AppSet.playAgainTimer;
            playAgainTimer1 = AppSet.playAgainTimer1;
            playAgainTimer = playAgainTimer1;
            //
            // Sync Manually Score Ratio
            //RatioConfig = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["ratio"]);
            RatioConfig = AppSet.RatioConfig;
            if (String.IsNullOrWhiteSpace(AppSet.strFSOpathGameExe)) {
                AppSet.strFSOpathGameExe = @System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Strokes_Priatek\Strokes.exe";
                strFSOpathGameExe = AppSet.strFSOpathGameExe;
            }
            try {
                gameProSerial.PortName = "COM1";
                gameProSerial.BaudRate = 9600;
                gameProSerial.Parity = System.IO.Ports.Parity.None;
                gameProSerial.DataBits = 8;
                gameProSerial.StopBits = System.IO.Ports.StopBits.One;
                gameProSerial.Handshake = System.IO.Ports.Handshake.None;
                gameProSerial.ReadTimeout = SerialPort.InfiniteTimeout;
                gameProSerial.WriteTimeout = 1000;
                //
                // Create Eventhandler for Coin Input
                gameProSerial.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceivedHandler);
                gameProSerial.Open();
                _isSerialAlive = true;
            } catch (Exception e) {
                //MessageBox.Show("Message: " + e.Message + "\r\nStackTrace: " + e.StackTrace, "GamePro Serial Exception");
                //gameProSerial = null;
                string strT = e.Message;
            }
            AppSet.firsttime = true;
            firsttime = AppSet.firsttime;

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerAsync();

            //
            // Handle Game Startup UI
            mpStartScreenMusic.Cursor = Cursors.None;                                // Turns Off StartGame Soundtrack Cursor Displayed (Doesn't display anyway)
            mpStartScreenMusic.Position = TimeSpan.Zero;
            mpStartScreenMusic.LoadedBehavior = MediaState.Stop;
            mpStartScreenMusic.Visibility = Visibility.Collapsed;
            mpStartScreen.Cursor = Cursors.None;                                // Turns Off StartGameBackground Cursor Displayed
            mpStartScreen.LoadedBehavior = MediaState.Stop;
            mpStartScreen.Position = TimeSpan.Zero;
            mpStartScreen.Visibility = Visibility.Collapsed;
            imgStartGameBtn.Cursor = Cursors.None;                          // Turns Off ImgStartGameBtn Cursor Displayed over StartGameImage
            imgStartGameBtn.Visibility = Visibility.Collapsed;
            imgInsertCreditBtn.Visibility = Visibility.Collapsed;
            imgInsertCreditBtn.Cursor = Cursors.None;
            //imgStartGameBtn.Visibility = Visibility.Hidden;                 // No Affect
            imgResultScreenBg.Visibility = Visibility.Collapsed;
            imgResultScreenBg.Cursor = Cursors.None;
            //imgResultScreenBg.Visibility = Visibility.Hidden;             // Stops AttractMode from Load
            imgPlayAgainBg.Visibility = Visibility.Collapsed;
            //imgPlayAgainBg.Visibility = Visibility.Hidden;                // Stops AttractMode from Load
            imgTip1.Visibility = Visibility.Collapsed;
            //imgTip1.Visibility = Visibility.Hidden;                       // Makes UI Window Expand
            //lbTickets.Visibility = Visibility.Collapsed;
            lbTickets.Visibility = Visibility.Hidden;                       // No Affect
            //lbScore.Visibility = Visibility.Collapsed;
            lbScore.Visibility = Visibility.Hidden;                         // No Affect
            lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
            //V1.0.0.2 Code
            //mePlayer3.LoadedBehavior = MediaState.Stop;
            //mePlayer3.Visibility = Visibility.Collapsed;
            //imgStartGameBtn.Visibility = Visibility.Collapsed;
            //this.image4.Visibility = Visibility.Collapsed;
            //imgResultScreenBg.Visibility = Visibility.Collapsed;
            //imgPlayAgainBg.Visibility = Visibility.Collapsed;
            //imgTip1.Visibility = Visibility.Collapsed;
            //lbTickets.Visibility = Visibility.Collapsed;
            //lbScore.Visibility = Visibility.Collapsed;
            //V1.0.0.2 EndCode
            //
            // Configure Background Worker and Invoke New bw.DoWork
            //AppSet.PropertyChanged += new PropertyChangedEventHandler(ApplicationSettings());
        }
        
        /// <summary>
        /// Localized Object for ApplicationSettings Class 
        /// </summary>
        public static ApplicationSettings AppSet { get; set; }
        #endregion

        #region New Game UI Based on Switch Case Architecture
        public Image CreditImage = new Image();
        public Image DebitImage = new Image();

        /// <summary>
        /// Empty Event Handler for the Game Mode Click event.
        /// Used to launch the Next Game Mode XAML UI Transition.
        /// TempTesting mpAttractMode_MouseDown replacement for mpAttractMode
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void btnGameMode_Click(object sender, EventArgs e) {
            try {
                BitmapImage imgCI = new BitmapImage();
                imgCI.BeginInit();
                imgCI.UriSource = new Uri(@"Insert Credit Button.png", UriKind.Relative);
                imgCI.EndInit();
                CreditImage.Stretch = Stretch.Fill;
                CreditImage.Source = imgCI;
                BitmapImage imgDI = new BitmapImage();
                imgDI.BeginInit();
                imgDI.UriSource = new Uri(@"Start Game Button.png", UriKind.Relative);
                imgDI.EndInit();
                DebitImage.Stretch = Stretch.Fill;
                DebitImage.Source = imgDI;
            } catch (Exception eIm) {
                MessageBox.Show("Credit Image: " + "\r\nDebit Image: " + eIm.Message, "Game Button Images Do Not Exist In Context");
            }

            switch (boolCreditMode) {
                case false:
                    if (AppSet.intCredits > AppSet.intTokensMin) {
                        lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                        AppSet.IsAttractMode = true;
                        AppSet.IsRewardsMode = true;
                        AppSet.IsPlayAgainMode = true;
                        goto case true;
                    }
                    if (AppSet.intCredits < 1) {
                        lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                        AppSet.IsAttractMode = true;
                        AppSet.IsRewardsMode = false;
                        AppSet.IsPlayAgainMode = false;
                        imgStartGameBtn.Source = CreditImage.Source;
                        //btnMonitorMode.ForeColor = Color.Black;
                        //bContinueCapturing = false;
                        if (!String.IsNullOrWhiteSpace(AppSet.strFSOpathGameExe)) {
                            lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                            AppSet.IsAttractMode = false;
                            imgStartGameBtn.Source = CreditImage.Source;
                            imgStartGameBtn.Visibility = Visibility.Visible;
                            mpAttractMode.LoadedBehavior = MediaState.Stop;
                            mpStartScreen.LoadedBehavior = MediaState.Play;
                            mpStartScreenMusic.LoadedBehavior = MediaState.Play;
                            //mainSocket.Close();
                        }
                        lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                        AppSet.IsAttractMode = true;
                        imgStartGameBtn.Source = CreditImage.Source;
                        imgStartGameBtn.Visibility = Visibility.Hidden;
                        mpAttractMode.LoadedBehavior = MediaState.Play;
                        mpStartScreen.LoadedBehavior = MediaState.Stop;
                        mpStartScreenMusic.LoadedBehavior = MediaState.Stop;
                        break;
                    }
                    if (AppSet.intCredits >= AppSet.intTokensMin) {
                        lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                        AppSet.IsAttractMode = false;
                        imgStartGameBtn.Source = CreditImage.Source;
                        imgStartGameBtn.Visibility = Visibility.Visible;
                        mpAttractMode.LoadedBehavior = MediaState.Stop;
                        mpStartScreen.LoadedBehavior = MediaState.Play;
                        mpStartScreenMusic.LoadedBehavior = MediaState.Play;
                        break;
                    }
                    break;
                case true:
                    if (AppSet.intTokensMin > AppSet.intCredits) {
                        lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                        AppSet.IsAttractMode = true;
                        AppSet.IsRewardsMode = false;
                        AppSet.IsPlayAgainMode = false;
                        goto case false;
                    }
                    if (AppSet.intCredits > AppSet.intTokensMin) {
                        lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                        AppSet.IsAttractMode = false;
                        mpAttractMode.LoadedBehavior = MediaState.Stop;
                        imgStartGameBtn.Source = DebitImage.Source;
                        mpStartScreen.LoadedBehavior = MediaState.Play;
                        mpStartScreenMusic.LoadedBehavior = MediaState.Play;
                        if (!String.IsNullOrWhiteSpace(AppSet.strFSOpathGameExe)) {					// Play Game
                            lbCredits.Content = "Credits Inserted (" + MainWindow.AppSet.intCredits.ToString() + " / " + AppSet.intTokensMin.ToString() + ")";
                            AppSet.IsAttractMode = false;
                            AppSet.intCredits = intCredits - AppSet.intTokensMin;
                            mpStartScreen.LoadedBehavior = MediaState.Stop;
                            mpStartScreenMusic.LoadedBehavior = MediaState.Stop;
                            bw.WorkerSupportsCancellation = true;
                            bw.WorkerReportsProgress = true;
                            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                            bw.RunWorkerAsync();
                            //btnMonitorMode.ForeColor = Color.Black;
                            //intMonitorClicks = 1;
                            AppSet.boolPrint ^= IsReadyPrint();
                        }
                        break;
                    }
                    //if (AppSet.intTickets == 1) {
                    //    btnMonitorMode.ForeColor = Color.Green;
                    //    intMonitorClicks = 2;
                    //    break;
                    //}
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Function for Returning Answer on If Tickets Are Ready to Print.
        /// </summary>
        /// <returns>Boolean</returns>
        private bool IsReadyPrint() {
            if (AppSet.intTickets > 0) {
                if ((!AppSet.IsAttractMode ^ AppSet.IsPlayAgainMode ^ AppSet.IsRewardsMode)) {
                    AppSet.isOn = false;
                }
                if ((!AppSet.IsAttractMode) && (!AppSet.IsPlayAgainMode) && (!AppSet.IsRewardsMode)) {
                    AppSet.isOn = false;
                }
            }
            return AppSet.isOn;
        }

        public bool boolCreditMode { get; set; }
        #endregion

    }







    #region Strokes XAML MVVM Priatek Game ViewModel Class (?NotBeingUsed?)
    /// <summary>
    /// Public Class for MVVM Priatek Game ViewModel.
    /// Implements PropertyChangedBase in Parallel with Public Class Game.
    /// Both this Class and Public Class Game both call INotifyPropertyChanged
    /// </summary>
    public class PriatekGameViewModel : PropertyChangedBase {
        /// <summary>
        /// Private List Collection of Priatek Games Available
        /// </summary>
        private List<Game> _games;

        /// <summary>
        /// Public List Collection of Priatek Games Available.
        /// For this demo only Strokes is available.
        /// Returns List Collection of Games.
        /// </summary>
        public List<Game> Games {
            get { return _games; }
        }

        /// <summary>
        /// Private System Threaded Timer
        /// </summary>
        private System.Threading.Timer timer;

        /// <summary>
        /// Priavte System Random Number Generator
        /// </summary>
        private System.Random random = new Random();

        /// <summary>
        /// Public MVVM ViewModel for Priatek Games UI
        /// </summary>
        public PriatekGameViewModel() {
            _games = Enumerable.Range(1, 9).Select(x => new Game()).ToList();
            timer = new Timer(x => RaiseRandomGame(), null, 0, 300);
        }

        /// <summary>
        /// Private Method for Raising Praitek Game into View.
        /// Comes from a Random Selection in the List Collection of Games.
        /// Can be modified to suit the needs of Priatek Gaming Products.
        /// ################# Make any changes here ######################
        /// </summary>
        private void RaiseRandomGame() {
            if (random.Next(1, 10) > 5) {                                           // If random number is < 5 skip this interaction
                return;
            }
            var game = Games[random.Next(0, 8)];                                    // Choose a random game. If only one exists in List Collection....
            if (game.IsUp) {                                                        // If already raised, do nothing
                return;
            }
            game.IsUp = true;                                                       // Raise it
            //
            // Get it down somewhere between 1 and 2 seconds after
            Task.Factory.StartNew(() => Thread.Sleep(random.Next(1000, 2000))).ContinueWith(x => game.IsUp = false);
        }
    #endregion
    }   // End Public Class PriatekGameViewModel

    #region Strokes XAML MVVM Priatek Game isUp Class (?NotBeingUsed?)
    /// <summary>
    /// Public Class Controller for Priatek Games.
    /// DataBound to MVVM StoryBoard Properties.
    /// Implements PropertyChangedBase Class which also implements INotifyPropertyChanged.
    /// </summary>
    public class Game : PropertyChangedBase {
        /// <summary>
        /// Private Boolean for Public Class Priatek Game isUp
        /// </summary>
        private bool _isUp;
        /// <summary>
        /// Public Boolean for Interface to Public Class Priatek Game isUp.
        /// Returns _isUp.
        /// Sets _isUp and calls Event Handler for Game Property Changed OnPropertyChanged passing string reference to property/caller.
        /// </summary>
        public bool IsUp {
            get { return _isUp; }
            set {
                _isUp = value;
                OnPropertyChanged("IsUp");
            }
        }
    #endregion
    }   // End Public Class Game
    
    #region Strokes XAML MVVM Priatek Game PropertyChangedBase Class (?NotBeingUsed?)
    /// <summary>
    /// Public Class for Game Properties.  
    /// Implements INotifyPropertyChanged.
    /// </summary>
    public class PropertyChangedBase : INotifyPropertyChanged {
        /// <summary>
        /// Public Event for Game Property Changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Event Handler for Game Property Changed.
        /// </summary>
        /// <param name="propertyName">String</param>
        protected virtual void OnPropertyChanged(string propertyName) {
            // Make sure our Public Event PropertyChangedGame is not null reference
            if (PropertyChanged != null) {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                    // Grab property event and push to a new threaded handler to invoke whatever we want to do now
                    PropertyChangedEventHandler handler = PropertyChanged;
                    // Make sure our new handler is not null reference
                    if (handler != null) {
                        // Hit the Application with the notification that a property has changed
                        handler(this, new PropertyChangedEventArgs(propertyName));
                        //
                        // Add in other actions here for anything else you want to do when certain/specific property changes occur

                    }
                }));
            }
        }
    #endregion
    }   // End Public Class PropertyChangedBase
}