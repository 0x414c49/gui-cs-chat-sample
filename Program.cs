using Terminal.Gui;
using System.Collections.Generic;
using System.Threading;
using System;

namespace RetroChat
{
    internal static class Program
    {
        private static string _username;
        private static readonly List<string> _users = new List<string>();
        private static readonly List<string> _messages = new List<string>();

        private static void Main()
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

            #region top-menu
            top.Add(
                new MenuBar(new MenuBarItem[] {
                    new MenuBarItem("_File", new MenuItem[]{
                        new MenuItem("_Quit", "", Application.RequestStop)
                    }), // end of file menu
                    new MenuBarItem("_Help", new MenuItem[]{
                        new MenuItem("_About", "", ()
                                    => MessageBox.Query(50, 5, "About", "Written by Ali Bahraminezhad\nVersion: 0.0001", "Ok"))
                    }) // end of the help menu
                })
            );
            #endregion

            #region chat-view
            var chatViewFrame = new FrameView("Chats")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(75),
                Height = Dim.Percent(80),
            };

            var chatView = new ListView
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            chatViewFrame.Add(chatView);
            mainWindow.Add(chatViewFrame);
            #endregion

            #region online-user-list
            var userListFrame = new FrameView("Online Users")
            {
                X = Pos.Right(chatViewFrame),
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            var userList = new ListView(_users)
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            userListFrame.Add(userList);
            mainWindow.Add(userListFrame);
            #endregion

            #region chat-bar
            var chatBar = new FrameView(null)
            {
                X = 0,
                Y = Pos.Bottom(chatViewFrame),
                Width = chatViewFrame.Width,
                Height = Dim.Fill()
            };

            var chatMessage = new TextField("")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(75),
                Height = Dim.Fill()
            };

            var sendButton = new Button("Send", true)
            {
                X = Pos.Right(chatMessage),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            sendButton.Clicked = () =>
            {
                Application.MainLoop.Invoke(() =>
                {
                    _messages.Add($"{_username}: {chatMessage.Text}");
                    chatView.SetSource(_messages);
                    chatMessage.Text = "";
                });
            };

            chatBar.Add(chatMessage);
            chatBar.Add(sendButton);
            mainWindow.Add(chatBar);
            #endregion

            // login window will be appear on the center screen
            var loginWindow = new LoginWindow(null)
            {
                OnExit = Application.RequestStop,

                OnLogin = (loginData) =>
                {
                    // for thread-safety
                    Application.MainLoop.Invoke(() =>
                    {
                        _users.Add(loginData.name);
                        _username = loginData.name;
                        userList.SetSource(_users);
                    });
                    DummyChat.StartSimulation();
                    Application.Run(top);
                }
            };

            top.Add(mainWindow);

            DummyChat.OnUserAdded = (loginData) =>
            {
                Application.MainLoop.Invoke(() =>
                {
                    _users.Add(loginData.name);
                    userList.SetSource(_users);
                });
            };

            // run login-window-first
            Application.Run(loginWindow);
        }
    }

    public static class DummyChat
    {
        public static Action<(string name, DateTime birthday)> OnUserAdded;
        private static readonly object _mutex = new object();
        private static Thread _main;

        public static void StartSimulation()
        {
            lock (_mutex)
            {
                if (_main == null)
                {
                    _main = new Thread(new ThreadStart(Simulate));
                    _main.Start();
                }
            }
        }

        private static void Simulate()
        {
            int counter = 0;
            while (++counter <= 10)
            {
                var name = $"User {counter}";
                OnUserAdded?.Invoke((name, DateTime.Now));
                Thread.Sleep(2000);
            }

        }
    }
}
