using System;

namespace Drive.GBxfxy
{
    class Program
    {
        //是一个国标的消防协议针对消防控制柜
        static void Main(string[] args)
        {
            GBxfxyDrive gBxfxy = new GBxfxyDrive();
            gBxfxy.Start(args[0]);
        }
    }
}
