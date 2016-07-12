using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Strokes {
    public class CreditsViewModel : INotifyPropertyChanged {
        private String _creditsOutput;
        public String CreditsOutput {
            get { return _creditsOutput; }
            set {
                _creditsOutput = MainWindow.AppSet.intCredits.ToString();
                NotifyPropertyChanged();
            }
        }
     

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
