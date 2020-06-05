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

        /// <summary>
        /// 设备心跳
        /// </summary>
        public const string DRIVE_HEARTBEAT = "drive/heartbeat";
    }
}
