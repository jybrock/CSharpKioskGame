using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;


namespace Strokes {

    class StrokesSettings : DependencyObject {

        //
        // Strokes MainWindow Application Properties
        //public int instructionsTimer = 25;                                                    // Not being used
        //public bool firsttime = true;                                                         // Not being used

        [JsonProperty]
        public int intCredits { get { return (int)GetValue(intCreditsProperty); } set {SetValue(intCreditsProperty, value); }}
        public static readonly DependencyProperty intCreditsProperty = DependencyProperty.Register("intCredits", typeof(int),typeof(StrokesSettings), new PropertyMetadata(0));

        [JsonProperty]
        public int intTickets { get { return (int)GetValue(intTicketsProperty); } set {SetValue(intTicketsProperty, value); }}
        public static readonly DependencyProperty intTicketsProperty = DependencyProperty.Register("intTickets", typeof(int),typeof(StrokesSettings), new PropertyMetadata(0));

        [JsonProperty]
        public int intScore { get { return (int)GetValue(intScoreProperty); } set {SetValue(intScoreProperty, value); }}
        public static readonly DependencyProperty intScoreProperty = DependencyProperty.Register("intScore", typeof(int),typeof(StrokesSettings), new PropertyMetadata(0));

        [JsonProperty]
        public int intTokensMin { get { return (int)GetValue(intTokensMinProperty); } set {SetValue(intTokensMinProperty, value); }}
        public static readonly DependencyProperty intTokensMinProperty = DependencyProperty.Register("intTokensMin", typeof(int),typeof(StrokesSettings), new PropertyMetadata(0));


        // Strokes ApplicationSettings Property for #1 Ticket Dispence based on Score
        private static int _intScoreTickets01;
        [JsonProperty("intScoreTickets01")]
        public int intScoreTickets01 { get { return _intScoreTickets01; } set { if (_intScoreTickets01 != value) { _intScoreTickets01 = value; RaisePropertyChanged("intScoreTickets01"); } } }
        // Strokes ApplicationSettings Property for #2 Ticket Dispence based on Score
        private static int _intScoreTickets02;
        [JsonProperty("intScoreTickets02")]
        public int intScoreTickets02 { get { return _intScoreTickets02; } set { if (_intScoreTickets02 != value) { _intScoreTickets02 = value; RaisePropertyChanged("intScoreTickets02"); } } }
        // Strokes ApplicationSettings Property for #3 Ticket Dispence based on Score
        private static int _intScoreTickets03;
        [JsonProperty("intScoreTickets03")]
        public int intScoreTickets03 { get { return _intScoreTickets03; } set { if (_intScoreTickets03 != value) { _intScoreTickets03 = value; RaisePropertyChanged("intScoreTickets03"); } } }
        // Strokes ApplicationSettings Property for #4 Ticket Dispence based on Score
        private static int _intScoreTickets04;
        [JsonProperty("intScoreTickets04")]
        public int intScoreTickets04 { get { return _intScoreTickets04; } set { if (_intScoreTickets04 != value) { _intScoreTickets04 = value; RaisePropertyChanged("intScoreTickets04"); } } }
        // Strokes ApplicationSettings Property for #5 Ticket Dispence based on Score
        private static int _intScoreTickets05;
        [JsonProperty("intScoreTickets05")]
        public int intScoreTickets05 { get { return _intScoreTickets05; } set { if (_intScoreTickets05 != value) { _intScoreTickets05 = value; RaisePropertyChanged("intScoreTickets05"); } } }
        // Strokes ApplicationSettings Property for #6 Ticket Dispence based on Score
        private static int _intScoreTickets06;
        [JsonProperty("intScoreTickets06")]
        public int intScoreTickets06 { get { return _intScoreTickets06; } set { if (_intScoreTickets06 != value) { _intScoreTickets06 = value; RaisePropertyChanged("intScoreTickets06"); } } }
        // Strokes ApplicationSettings Property for #7 Ticket Dispence based on Score
        private static int _intScoreTickets07;
        [JsonProperty("intScoreTickets07")]
        public int intScoreTickets07 { get { return _intScoreTickets07; } set { if (_intScoreTickets07 != value) { _intScoreTickets07 = value; RaisePropertyChanged("intScoreTickets07"); } } }
        // Strokes ApplicationSettings Property for #8 Ticket Dispence based on Score
        private static int _intScoreTickets08;
        [JsonProperty("intScoreTickets08")]
        public int intScoreTickets08 { get { return _intScoreTickets08; } set { if (_intScoreTickets08 != value) { _intScoreTickets08 = value; RaisePropertyChanged("intScoreTickets08"); } } }
        // Strokes ApplicationSettings Property for #9 Ticket Dispence based on Score
        private static int _intScoreTickets09;
        [JsonProperty("intScoreTickets09")]
        public int intScoreTickets09 { get { return _intScoreTickets09; } set { if (_intScoreTickets09 != value) { _intScoreTickets09 = value; RaisePropertyChanged("intScoreTickets09"); } } }
        // Strokes ApplicationSettings Property for #10 Ticket Dispence based on Score
        private static int _intScoreTickets10;
        [JsonProperty("intScoreTickets10")]
        public int intScoreTickets10 { get { return _intScoreTickets10; } set { if (_intScoreTickets10 != value) { _intScoreTickets10 = value; RaisePropertyChanged("intScoreTickets10"); } } }



        // Strokes ApplicationSettings Property for UI Tutorial Screen Inactivity Timeout
        private static int _intTutorialTimeout;
        [JsonProperty("intTutorialTimeout")]
        public int intTutorialTimeout { get { return _intTutorialTimeout; } set { if (_intTutorialTimeout != value) { _intTutorialTimeout = value; RaisePropertyChanged("intTutorialTimeout"); } } }

        // Strokes ApplicationSettings Property for UI Leaderboard Screen Inactivity Timeout
        private static int _intLeaderboardTimeout;
        [JsonProperty("intLeaderboardTimeout")]
        public int intLeaderboardTimeout { get { return _intLeaderboardTimeout; } set { if (_intLeaderboardTimeout != value) { _intLeaderboardTimeout = value; RaisePropertyChanged("intLeaderboardTimeout"); } } }

        // Strokes ApplicationSettings Property for UI Brush Size
        // Between 1 and 5
        private static int _intGameBrushSize;
        [JsonProperty("intGameBrushSize")]
        public int intGameBrushSize { get { return _intGameBrushSize; } set { if (_intGameBrushSize != value) { _intGameBrushSize = value; RaisePropertyChanged("intGameBrushSize"); } } }


        // Strokes ApplicationSettings Property for UI Completion Percentage
        // Between 10% and 100%
        private static int _intGameCompletePercent;
        [JsonProperty("intGameCompletePercent")]
        public int intGameCompletePercent { get { return _intGameCompletePercent; } set { if (_intGameCompletePercent != value) { _intGameCompletePercent = value; RaisePropertyChanged("intGameCompletePercent"); } } }

        // Strokes ApplicationSettings Property for UI OutOfBounds Penalty Adjustment
        // Between 1 and 5
        private static int _intOutOfBoundsPenalty;
        [JsonProperty("intOutOfBoundsPenalty")]
        public int intOutOfBoundsPenalty { get { return _intOutOfBoundsPenalty; } set { if (_intOutOfBoundsPenalty != value) { _intOutOfBoundsPenalty = value; RaisePropertyChanged("intOutOfBoundsPenalty"); } } }

        // Strokes ApplicationSettings Property for File System Path to Fully Qualified JSON File
        private static string _strFSOpathSettings;
        [JsonProperty("strFSOpathSettings")]
        public string strFSOpathSettings { get { return _strFSOpathSettings; } set { if (_strFSOpathSettings != value) { _strFSOpathSettings = value; RaisePropertyChanged("strFSOpathSettings"); } } }
        // Strokes ApplicationSettings Property for File System Path to XML Game Score File
        private static string _strFSOpathScore;
        [JsonProperty("strFSOpathScore")]
        public string strFSOpathScore { get { return _strFSOpathScore; } set { if (_strFSOpathScore != value) { _strFSOpathScore = value; RaisePropertyChanged("strFSOpathScore"); } } }
        //
        // Strokes ApplicationSettings Property for File System Path to Windows Shell Executible 
        private static string _strFSOpathGameExe;
        [JsonProperty("strFSOpathGameExe")]
        public string strFSOpathGameExe { get { return _strFSOpathGameExe; } set { if (_strFSOpathGameExe != value) { _strFSOpathGameExe = value; RaisePropertyChanged("strFSOpathGameExe"); } } }
        //
        // Strokes ApplicationSettings Property for File System Path to XML Scorecard saved/created by Windows Shell Executible
        private static string _scorePath;
        [JsonProperty("scorePath")]
        public string scorePath { get { return _scorePath; } set { if (_scorePath != value) { _scorePath = value; RaisePropertyChanged("scorePath"); } } }
        //
        // Strokes ApplicationSettings Property for Game Rewards Results Ratio
        private static double _Ratio;
        [JsonProperty("Ratio")]
        public double Ratio { get { return _Ratio; } set { if (_Ratio != value) { _Ratio = value; RaisePropertyChanged("Ratio"); } } }
        //
        // Strokes ApplicationSettings Property if Windows Shell Executible is Running/On
        private static bool _isOn;
        [JsonProperty("isOn")]
        public bool isOn { get { return _isOn; } set { if (_isOn != value) { _isOn = value; RaisePropertyChanged("isOn"); } } }
        //
        // Strokes ApplicationSettings Property of first instance for ResultTimer Timeout
        private static int _resultTimer;
        [JsonProperty("resultTimer")]
        public int resultTimer { get { return _resultTimer; } set { if (_resultTimer != value) { _resultTimer = value; RaisePropertyChanged("resultTimer"); } } }
        //
        // Strokes ApplicationSettings Property of first instance for PlayAgain Timeout
        private static int _playAgainTimer;
        [JsonProperty("playAgainTimer")]
        public int playAgainTimer { get { return _playAgainTimer; } set { if (_playAgainTimer != value) { _playAgainTimer = value; RaisePropertyChanged("playAgainTimer"); } } }
        //
        // Strokes ApplicationSettings Property for if Session Loop for Game is first run/cycle
        private static bool _firsttime;
        [JsonProperty("firsttime")]
        public bool firsttime { get { return _firsttime; } set { if (_firsttime != value) { _firsttime = value; RaisePropertyChanged("firsttime"); } } }
        //
        // Strokes ApplicationSettings Property for if PlayAgain
        private static bool _boolPlayAgain;
        [JsonProperty("boolPlayAgain")]
        public bool boolPlayAgain { get { return _boolPlayAgain; } set { if (_boolPlayAgain != value) { _boolPlayAgain = value; RaisePropertyChanged("boolPlayAgain"); } } }
        //
        // Strokes ApplicationSettings Property for Cycle Counter
        private static int _cycle;
        [JsonProperty("cycle")]
        public int cycle { get { return _cycle; } set { if (_cycle != value) { _cycle = value; RaisePropertyChanged("cycle"); } } }
        //
        // Strokes ApplicationSettings Property for if Serial Connection is Alive
        private static bool __isSerialAlive;
        [JsonProperty("_isSerialAlive")]
        public bool _isSerialAlive { get { return __isSerialAlive; } set { if (__isSerialAlive != value) { __isSerialAlive = value; RaisePropertyChanged("_isSerialAlive"); } } }
        //
        // Strokes ApplicationSettings Property for ResultScreen Timeout
        private static int _timeofResultScreen;
        [JsonProperty("TimeofResultScreen")]
        public int TimeofResultScreen { get { return _timeofResultScreen; } set { if (_timeofResultScreen != value) { _timeofResultScreen = value; RaisePropertyChanged("TimeofResultScreen"); } } }
        //
        // Strokes ApplicationSettings Property for PlayAgain Timeout
        private static int _playAgainTimer1;
        [JsonProperty("playAgainTimer1")]
        public int playAgainTimer1 { get { return _playAgainTimer1; } set { if (_playAgainTimer1 != value) { _playAgainTimer1 = value; RaisePropertyChanged("playAgainTimer1"); } } }
        //
        // Strokes ApplicationSettings Property for Rewards Ratio
        private static double _ratioConfig;
        [JsonProperty("RatioConfig")]
        public double RatioConfig { get { return _ratioConfig; } set { if (_ratioConfig != value) { _ratioConfig = value; RaisePropertyChanged("RatioConfig"); } } }
        //
        // Strokes ApplicationSettings Property for "Base" Working Directory
        private static string _baseWorkingDirectory;
        [JsonProperty("BaseWorkingDirectory")]
        public string BaseWorkingDirectory { get { return _baseWorkingDirectory; } set { if (_baseWorkingDirectory != value) { _baseWorkingDirectory = value; RaisePropertyChanged("BaseWorkingDirectory"); } } }
        //
        // Strokes ApplicationSettings Property for "Strokes" Working Directory
        private static string _strokesWorkingDirectory;
        [JsonProperty("StrokesWorkingDirectory")]
        public string StrokesWorkingDirectory { get { return _strokesWorkingDirectory; } set { if (_strokesWorkingDirectory != value) { _strokesWorkingDirectory = value; RaisePropertyChanged("StrokesWorkingDirectory"); } } }
        //
        // Strokes ApplicationSettings Property for "Strokes" Working Directory
        private static string _strokesJSONfilename;
        [JsonProperty("StrokesJSONfilename")]
        public string StrokesJSONfilename { get { return _strokesJSONfilename; } set { if (_strokesJSONfilename != value) { _strokesJSONfilename = value; RaisePropertyChanged("StrokesJSONfilename"); } } }

        //
        // Strokes ApplicationSettings Properties 
        private static bool _debugMode;
        [JsonProperty("DebugMode")]
        public bool DebugMode { get { return _debugMode; } set { if (_debugMode != value) { _debugMode = value; RaisePropertyChanged("DebugMode"); } } }
        private static bool _attractMode;
        [JsonProperty("AttractMode")]
        public bool AttractMode { get { return _attractMode; } set { if (_attractMode != value) { _attractMode = value; RaisePropertyChanged("AttractMode"); } } }
        private static bool _tokenReady;
        [JsonProperty("TokenReady")]
        public bool TokenReady { get { return _tokenReady; } set { if (_tokenReady != value) { _tokenReady = value; RaisePropertyChanged("TokenReady"); } } }
        private static string _gameNameFormat;
        [JsonProperty("GameNameFormat")]
        public string GameNameFormat { get { return _gameNameFormat; } set { if (_gameNameFormat != value) { _gameNameFormat = value; RaisePropertyChanged("GameNameFormat"); } } }
        private static string _extraFileExtensions;
        [JsonProperty("ExtraFileExtensions")]
        public string ExtraFileExtensions { get { return _extraFileExtensions; } set { if (_extraFileExtensions != value) { _extraFileExtensions = value; RaisePropertyChanged("ExtraFileExtensions"); } } }
        private static string _strGameFSOfile;
        [JsonProperty("strGameFSOfile")]
        public string strGameFSOfile { get { return _strGameFSOfile; } set { if (_strGameFSOfile != value) { _strGameFSOfile = value; RaisePropertyChanged("strGameFSOfile"); } } }
        private static bool _automaticRun;
        [JsonProperty("AutomaticRun")]
        public bool AutomaticRun { get { return _automaticRun; } set { if (_automaticRun != value) { _automaticRun = value; RaisePropertyChanged("AutomaticRun"); } } }
        private static string _galleryCreationSubCategory;
        [JsonProperty("GalleryCreationSubCategory")]
        public string GalleryCreationSubCategory { get { return _galleryCreationSubCategory; } set { if (_galleryCreationSubCategory != value) { _galleryCreationSubCategory = value; RaisePropertyChanged("GalleryCreationSubCategory"); } } }
        private static bool _filenameOnlyCheck;
        [JsonProperty("FilenameOnlyCheck")]
        public bool FilenameOnlyCheck { get { return _filenameOnlyCheck; } set { if (_filenameOnlyCheck != value) { _filenameOnlyCheck = value; RaisePropertyChanged("FilenameOnlyCheck"); } } }
        private static bool _boolPrint;
        [JsonProperty("boolPrint")]
        public bool boolPrint { get { return _boolPrint; } set { if (_boolPrint != value) { _boolPrint = value; RaisePropertyChanged("boolPrint"); } } }

        //
        // Strokes ApplicationSettings Event Handlers
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string caller) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
        //
        // C# 5 INotifyPropertyChanged 
        //private static void RaisePropertyChanged([CallerMemberName] string caller = "") {
        //if (PropertyChanged != null) { PropertyChanged(CallConvThiscall, new PropertyChangedEventArgs(caller)); }
        //}
        //
        // Properties created the standard way for INotifyPropertyChaged:
        private static DateTime _nextUpdateCheck;
        public DateTime NextUpdateCheck { get { return _nextUpdateCheck; } set { if (_nextUpdateCheck != value) { _nextUpdateCheck = value; } RaisePropertyChanged("NextUpdateCheck"); } }
        //RaisePropertyChanged("NextUpdateCheck");
        private void AppSet_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            //Console.WriteLine("A property has changed: " + e.PropertyName);
            RaisePropertyChanged(e.PropertyName);
        }


        #region New UI Switch Case Properties
        public bool IsAttractMode { get; set; }

        public bool IsRewardsMode { get; set; }

        public bool IsPlayAgainMode { get; set; }
        #endregion


        #region Game XML Localized Properties
        private static string _strFSOpathGameXML;
        [JsonProperty("strFSOpathGameXML")]
        public string strFSOpathGameXML { get { return _strFSOpathGameXML; } set { if (_strFSOpathGameXML != value) { _strFSOpathGameXML = value; RaisePropertyChanged("strFSOpathGameXML"); } } }

        private static string _totalGameTimeInMinutes;
        [JsonProperty("totalGameTimeInMinutes")]
        public string totalGameTimeInMinutes { get { return _totalGameTimeInMinutes; } set { if (_totalGameTimeInMinutes != value) { _totalGameTimeInMinutes = value; RaisePropertyChanged("totalGameTimeInMinutes"); } } }

        private static string _highScoreTimeOut;
        [JsonProperty("highScoreTimeOut")]
        public string highScoreTimeOut { get { return _highScoreTimeOut; } set { if (_highScoreTimeOut != value) { _highScoreTimeOut = value; RaisePropertyChanged("highScoreTimeOut"); } } }

        private static string _startWithoutBgMusic;
        [JsonProperty("startWithoutBgMusic")]
        public string startWithoutBgMusic { get { return _startWithoutBgMusic; } set { if (_startWithoutBgMusic != value) { _startWithoutBgMusic = value; RaisePropertyChanged("startWithoutBgMusic"); } } }

        private static string _sizeMult;
        [JsonProperty("sizeMult")]
        public string sizeMult { get { return _sizeMult; } set { if (_sizeMult != value) { _sizeMult = value; RaisePropertyChanged("sizeMult"); } } }

        private static string _coloredThresholdPercentage;
        [JsonProperty("coloredThresholdPercentage")]
        public string coloredThresholdPercentage { get { return _coloredThresholdPercentage; } set { if (_coloredThresholdPercentage != value) { _coloredThresholdPercentage = value; RaisePropertyChanged("coloredThresholdPercentage"); } } }

        private static string _outOfBoundsPenaltyMultiplier;
        [JsonProperty("outOfBoundsPenaltyMultiplier")]
        public string outOfBoundsPenaltyMultiplier { get { return _outOfBoundsPenaltyMultiplier; } set { if (_outOfBoundsPenaltyMultiplier != value) { _outOfBoundsPenaltyMultiplier = value; RaisePropertyChanged("outOfBoundsPenaltyMultiplier"); } } }

        private static string _startInFullScreen;
        [JsonProperty("startInFullScreen")]
        public string startInFullScreen { get { return _startInFullScreen; } set { if (_startInFullScreen != value) { _startInFullScreen = value; RaisePropertyChanged("startInFullScreen"); } } }

        private static string _gameOverTimeOutTimeInSeconds;
        [JsonProperty("gameOverTimeOutTimeInSeconds")]
        public string gameOverTimeOutTimeInSeconds { get { return _gameOverTimeOutTimeInSeconds; } set { if (_gameOverTimeOutTimeInSeconds != value) { _gameOverTimeOutTimeInSeconds = value; RaisePropertyChanged("gameOverTimeOutTimeInSeconds"); } } }

        private static string _outputScoreURL;
        [JsonProperty("outputScoreURL")]
        public string outputScoreURL { get { return _outputScoreURL; } set { if (_outputScoreURL != value) { _outputScoreURL = value; RaisePropertyChanged("outputScoreURL"); } } }
        #endregion






    }





    /// <summary>
    /// Interaction logic for ApplicationSettings.xaml
    /// </summary>

    public partial class ApplicationSettings : Window {

        public ApplicationSettings() {
            InitializeComponent();
            //MainWindow.AppSet.SetDefaults(MainWindow.AppSet);
        }
        /// <summary>
        /// A starter method for saving Default Values to ApplicationSettings JSON configuration.
        /// </summary>
        /// <param name="settings">ApplicationSettings</param>
        public void SetDefaults(ApplicationSettings settingsDefaults) {
            MainWindow.AppSet = new ApplicationSettings();
            SetDefaults(MainWindow.AppSet);
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
            settingsDefaults.scorePath = BaseWorkingDirectory + @"\StrokesScores.xml";
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

            }
        } // End Public Partial Class MainWindow
        //


        // ##########################################################################################
        // The WPF applications, in the AssemblyInfo.cs file, the attributes are set as follows:
        // [assembly: AssemblyCompany("ExampleCompany")]
        // [assembly: AssemblyProduct("ExampleProduct")]
        //
        // After installation the path should point to:
        // ??:\\Users\USER\AppData\Roaming\ExampleCompany\ExampleProduct\
        // ##########################################################################################


    }
}
