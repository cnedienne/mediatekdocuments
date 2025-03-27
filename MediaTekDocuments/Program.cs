using MediaTekDocuments.view;
using System;
using System.Windows.Forms;

namespace MediaTekDocuments
{
    /// <summary>
    /// Namespace for the MediaTekDocuments application.
    /// This namespace contains the main entry point for the application.
    /// </summary>
    internal class NamespaceDoc
    {
    
    }
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmAuthentification());
        }
    }
}
