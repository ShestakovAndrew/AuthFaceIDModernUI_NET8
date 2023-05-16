using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.FaceID;
using ModernLoginWindow;
using System.Windows;
using System.Windows.Input;

namespace AuthFaceIDModernUI.Windows
{
    public partial class PersonalArea : Window
    {
        private bool m_isWindowLoading { get; set; }
        private string m_userLogin { get; set; }

        public PersonalArea(string userLogin)
        {
            InitializeComponent();

            m_isWindowLoading = true;

            m_userLogin = userLogin;
            TitleTextBlock.Text += userLogin;

            SetFaceIDToggleButton();

            m_isWindowLoading = false;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private async void FaceIDToggleButton_CheckedAsync(object sender, RoutedEventArgs e)
        {
            if (!m_isWindowLoading)
            {
                IsEnabled = false;
                SetUserFaceID userFaceID = new();

                if (userFaceID.ShowDialog() == true)
                {
                    UsersDataBase db = new();
                    if (await FaceIDTool.SetPersonID(userFaceID.faceToCheck!, db.GetIDByLogin(m_userLogin)))
                    {
                        db.AddFaceByLogin(m_userLogin);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка в привязке для ID пользователя лица. Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Лицо для распознания не выбрано.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                IsEnabled = true;
            }

            SetFaceIDToggleButton();
        }

        private async void FaceIDToggleButton_UncheckedAsync(object sender, RoutedEventArgs e)
        {
            UsersDataBase db = new();
            db.DeleteFaceByLogin(m_userLogin);

            if (await FaceIDTool.DeletPersonID(db.GetIDByLogin(m_userLogin)))
            {
                FaceIDButtonsDisable();
            }
            else
            {
                MessageBox.Show("Ошибка в удалении привязки ID пользователя и его лица. Повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteFaceIDButton_Click(object sender, RoutedEventArgs e)
        {
            FaceIDToggleButton.IsChecked = false;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePassword = new(m_userLogin);
            changePassword.Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Login login = new();
            login.Show();
            Close();
        }

        private void SetFaceIDToggleButton()
        {
            UsersDataBase db = new();

            if (db.IsExistFaceIDByLogin(m_userLogin))
            {
                FaceIDButtonsEnable();
            }
            else
            {
                FaceIDButtonsDisable();
            }
        }

        private void FaceIDButtonsDisable()
        {
            FaceIDToggleButton.IsChecked = false;
            DeleteFaceIDButton.IsEnabled = false;
        }

        private void FaceIDButtonsEnable()
        {
            FaceIDToggleButton.IsChecked = true;
            DeleteFaceIDButton.IsEnabled = true;
        }
    }
}