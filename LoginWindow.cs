using System;
using Terminal.Gui;

namespace RetroChat
{
    public class LoginWindow : Window
    {
        private readonly View _parent;
        public Action<(string name, DateTime birthday)> OnLogin { get; set; }
        public Action OnExit { get; set; }

        public LoginWindow(View parent) : base("Login", 5)
        {
            _parent = parent;
            InitControls();
            InitStyle();
        }

        public void InitStyle()
        {
            X = Pos.Center();
            Width = Dim.Percent(50);
            Height = 17;
        }

        public void Close()
        {
            Application.RequestStop();
            _parent?.Remove(this);
        }

        private void InitControls()
        {
            #region nickname
            var nameLabel = new Label(0, 0, "Nickname");
            var nameText = new TextField("")
            {
                X = Pos.Left(nameLabel),
                Y = Pos.Top(nameLabel) + 1,
                Width = Dim.Fill()
            };
            Add(nameLabel);
            Add(nameText);
            #endregion

            #region birthday
            var birthLabel = new Label("Birthday")
            {
                X = Pos.Left(nameText),
                Y = Pos.Top(nameText) + 1
            };
            var birthText = new TextField("")
            {
                X = Pos.Left(birthLabel),
                Y = Pos.Top(birthLabel) + 1,
                Width = Dim.Fill()
            };
            Add(birthLabel);
            Add(birthText);
            #endregion

            #region buttons
            var loginButton = new Button("Login", true)
            {
                X = Pos.Left(birthText),
                Y = Pos.Top(birthText) + 1
            };

            var exitButton = new Button("Exit")
            {
                X = Pos.Right(loginButton) + 5,
                Y = Pos.Top(loginButton)
            };

            Add(exitButton);
            Add(loginButton);
            #endregion

            #region bind-button-events
            loginButton.Clicked = () =>
            {
                if (nameText.Text.ToString().TrimStart().Length == 0)
                {
                    MessageBox.ErrorQuery(25, 8, "Error", "Name cannot be empty.", "Ok");
                    return;
                }

                var isDateValid = DateTime.TryParse(birthText.Text.ToString(), out DateTime birthDate);

                if (string.IsNullOrEmpty(birthText.Text.ToString()) || !isDateValid)
                {
                    MessageBox.ErrorQuery(25, 8, "Error", "Date is required\nor is invalid.", "Ok");
                    return;
                }

                OnLogin?.Invoke((name: nameText.Text.ToString(), birthday: birthDate));

                Close();
            };

            exitButton.Clicked = () =>
            {
                OnExit?.Invoke();
                Close();
            };
            #endregion
        }
    }
}