using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private IntPtr handle;
        private Process process;
        public Form1()
        {
            InitializeComponent();
            process = Process.Start("cmd");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process currentProcess = Process.GetCurrentProcess();
            handle = currentProcess.MainWindowHandle;
            SetForegroundWindow(handle);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            handle = process.MainWindowHandle;
            SetForegroundWindow(handle);
        }
    }
}
