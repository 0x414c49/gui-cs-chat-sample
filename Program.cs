using Terminal.Gui;

namespace RetroChat
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

            var mainWindow = new Window("Retro Chat")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            // login window will be appear on the center screen
            var loginWindow = new LoginWindow(mainWindow);
            mainWindow.Add(loginWindow);

            Application.Run();
        }
    }
}
