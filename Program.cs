using Terminal.Gui;

namespace RetroChat
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

            var mainWindow = new Window("Retro chat")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[]{
                    new MenuItem("_Quit", "", () => Application.RequestStop())
                }), // end of file menu
                new MenuBarItem("_Help", new MenuItem[]{
                    new MenuItem("_About", "", ()
                                => MessageBox.Query(15, 5, "About", "Written by Ali Bahraminezhad\nVersion: 0.0001", "Ok"))
                }) // end of the help menu
            });

            // login window will be appear on the center screen
            var loginWindow = new LoginWindow(null);
            loginWindow.OnExit = () => Application.RequestStop();

            loginWindow.OnLogin = (loginData) =>
            {
                mainWindow.Add(menu);
                Application.Run(top);
            };

            top.Add(mainWindow);

            // run login-window-first
            Application.Run(loginWindow);
        }
    }
}
