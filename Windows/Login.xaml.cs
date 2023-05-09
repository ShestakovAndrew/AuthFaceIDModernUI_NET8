using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AuthFaceIDModernUI.DataBase;
using AuthFaceIDModernUI.Windows;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Face;
using AuthFaceIDModernUI.FaceID;

namespace ModernLoginWindow
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void LoginToPersonalArea(string login)
        {
            PersonalArea personalArea = new(login);
            personalArea.Show();
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UsersDataBase db = new();
            if (!db.IsLoginExistInDB(LoginTextBox.Text))
            {
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            if (!db.IsPasswordCorrectForUser(LoginTextBox.Text, PasswordBox.Password))
            {
                PasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            LoginToPersonalArea(LoginTextBox.Text);
        }

        private void FaceIDButton_Click(object sender, RoutedEventArgs e)
        {
            UsersDataBase db = new();

            if (LoginTextBox.Text.Length == 0 || !db.IsLoginExistInDB(LoginTextBox.Text))
            {
                LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }
            
            if (!db.IsExistFaceIDByLogin(LoginTextBox.Text))
            {
                FaceIDButton.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Для данного пользователя не настроен FaceID.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                FaceCamera faceCamera = new();
                faceCamera.TurnOn();

                EigenFaceRecognizer faceRecognition = FacesRecognizerTool.GetRecognizerByLogin(LoginTextBox.Text);
                List<Mat> facesToCheck = faceCamera.m_last10UserFaces.ToList();

                List<FaceRecognizer.PredictionResult> predictionResults = new();
                foreach (Mat face in facesToCheck)
                {
                    CvInvoke.CvtColor(face, face, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray, 0);
                    predictionResults.Add(faceRecognition.Predict(face));
                }

                double sumDistance = 0;
                foreach (FaceRecognizer.PredictionResult result in predictionResults)
                {
                    sumDistance += result.Distance;
                }
                sumDistance /= 30;

                faceCamera.TurnOff();

                if (sumDistance <= 3000)
                {
                    LoginToPersonalArea(LoginTextBox.Text);
                }
                else
                {
                    MessageBox.Show("Ошибка распознования. Повторите попытку.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new();
            registration.Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginTextBox.BorderBrush = new SolidColorBrush(Colors.Gray);
            FaceIDButton.BorderBrush = new SolidColorBrush(Colors.Black);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox.BorderBrush = new SolidColorBrush(Colors.Gray);
        }
    }
}