using System;

namespace Drive.Mitsubishi.PLC.MelsecFx_LinksOverTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            MelsecLinksOverTcpDrive melsecLinksOver = new MelsecLinksOverTcpDrive();
            melsecLinksOver.Start(args[0]);
        }
    }
}
