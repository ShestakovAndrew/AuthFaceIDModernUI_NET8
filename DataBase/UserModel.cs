using Emgu.CV;

namespace ModernLoginWindow;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public bool isExistFaceID { get; set; }
    public byte[]? faceID { get; set; } 
}