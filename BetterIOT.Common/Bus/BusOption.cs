namespace BetterIOT.Common.Bus
{
    public class BusOption
    {
        /*内部配置地址*/
        public const string PublisherAddress = "tcp://127.0.0.1:1200";
        public const string SubscriberAddress = "tcp://127.0.0.1:1201";

        /*发布/订阅主题*/

        /// <summary>
        /// 数据采集输出
        /// </summary>
        public const string DATA_OUTPUT = "data/output";

        /// <summary>
        /// 控制命令输入
        /// </summary>
        public const string CMD_INPUT = "cmd/input";

        /// <summary>
        /// 配置数据变更
        /// </summary>
        public const string CONFIG_CHANGE = "config/change";

        //外部控制命令
        public const string CTRL_START = "ctrl/start";   //通知管理器启动某驱动
        public const string CTRL_STOP = "ctrl/stop";    //通知管理器停止某驱动
    }
}
