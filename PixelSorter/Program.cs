using System;
using System.Windows.Forms;

namespace PixelSorter {
    static class Program {
        [STAThread]
        static void Main( string[] args ) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new PixelSorter( args ) );
        }
    }
}
