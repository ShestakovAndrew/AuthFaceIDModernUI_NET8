using Emgu.CV;

namespace ModernLoginWindow;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Mat? Face { get; set; }
}