using System.Globalization;

namespace StajyerTakip.Services
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            //CultureInfo language = new CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");
        }
    }
}
