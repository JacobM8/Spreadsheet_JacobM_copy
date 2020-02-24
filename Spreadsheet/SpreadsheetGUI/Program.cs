/// <summary>
///  Author:     Joe Zachary
///  Updated by: Jim de St. Germain
///  
///  Dates:      (Original) 2012-ish
///              (Updated for Core) 2020
///              
///  Target: ASP Framework (and soon ASP CORE 3.1)
///  
///  This program is an example of how to create a GUI application for 
///  a spreadsheet project.
///  
///  It relies on a working Spreadsheet Panel class, but defines other
///  GUI elements, such as the file menu (open and close operations).
///  
///  WARNING: with the current GUI designer, you will not be able to
///           use the toolbox "Drag and Drop" to update this file.
/// </summary>


using System;
using System.Windows.Forms;

namespace SpreadsheetGrid_Core
{

    class Spreadsheet_Window : ApplicationContext
    {
        /// <summary>
        ///  Number of open forms
        /// </summary>
        private int formCount = 0;

        /// <summary>
        ///  Singleton ApplicationContext
        /// </summary>
        private static Spreadsheet_Window appContext;

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static Spreadsheet_Window getAppContext()
        {
            if (appContext == null)
            {
                appContext = new Spreadsheet_Window();
            }
            return appContext;
        }

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private Spreadsheet_Window()
        {
        }

        /// <summary>
        /// Build another GUI Window
        /// </summary>
        public void RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // Assign an EVENT handler to take an action when the GUI is closed 
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }

    }

    class GUI_Application
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start an application context and run one form inside it
            Spreadsheet_Window appContext = Spreadsheet_Window.getAppContext();
            appContext.RunForm(new Form1());
            Application.Run(appContext);

        }
    }
}



/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGrid_Core
{
    static class Program
    {



        //**** orginal stuff

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
*/
