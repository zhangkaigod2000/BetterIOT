using System;

namespace Drive.Mitsubishi.PLC.MelsecFx_Links
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecLinksDrive melsecLinks = new MelsecLinksDrive();
            melsecLinks.Start(args[0]);
        }
    }
}
