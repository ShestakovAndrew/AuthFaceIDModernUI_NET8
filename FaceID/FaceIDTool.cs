using AuthFaceIDModernUI.API;
using AuthFaceIDModernUI.DataBase;
using Emgu.CV;
using System.Text.RegularExpressions;
using System.Windows;

namespace AuthFaceIDModernUI.FaceID
{
    public static class FaceIDTool
    {
        public static async Task<bool> DeletPersonID(int personID)
        {
            try
            {
                DeleteUserFaceResponse? response = await VisionAPIService.Delete(personID);
                return (response != null) && (response.status == 200) && (response.body.objects![0].status == 0);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static async Task<int> RecornizeIDOnImage(Mat imageFace)
        {
            try
            {
                string filePath = DirectoryTool.SaveTempImage(imageFace);
                RecognizeUserFaceResponse? response = await VisionAPIService.Recognize(filePath);
                DirectoryTool.DeleteTempImage(filePath);

                if (
                    (response != null) &&
                    (response.status == 200) &&
                    (response.body.objects![0].status == 0) &&
                    (response.body.objects![0].persons[0].tag != "undefined")
                )
                {
                    return Int32.Parse(Regex.Match(response.body.objects[0].persons[0].tag, @"\d+").Value);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return -1;
        }

        public static async Task<bool> SetPersonID(Mat imageFace, int personID)
        {
            try
            {
                string filePath = DirectoryTool.SaveTempImage(imageFace);
                SetUserFaceResponse? response = await VisionAPIService.Set(filePath, personID);
                DirectoryTool.DeleteTempImage(filePath);

                return (response != null) && (response.status == 200) && (response.body.objects![0].status == 0);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static async Task ResetSpaceWithFaces()
        {
            try
            {
                await VisionAPIService.Truncate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}