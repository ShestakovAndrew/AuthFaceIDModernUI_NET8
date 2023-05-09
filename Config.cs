using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AuthFaceIDModernUI
{
    public static class Config
    {
        public static int CountFacesToLearn = 200;
        public static string DataBaseName = "users.db";
        public static string HaarCascadePath = "D:\\CourseWork\\FaceID\\haarcascade_frontalface_default.xml";
        public static string UserPhotosPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string DataBasePath = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DataBaseName);
    }
}
