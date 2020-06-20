# BetterIOT
一个适合在arm和X86边缘计算设备上运行的工业设备数据采集系统

助力AIOT事业，让C#写的采集程序借助于netCore可以运行在基于ubuntu的arm设备上及运行以及X86架构的windows和ubuntu上，可以局域网、互联网使用。
借助于dathlin/HslCommunication这个项目来实现部分设备的访问。借助于MQTT来实现设备数据的对外通讯部分。
##### 目前集成的采集驱动有：
* modbusrtu,DLT645 2007,GBT26875.3 城市消防远程监控系统 报警传输网络通信协议 中的部分数据
* 西门子 S1200,S400,S300,S1500,S200,S200Smart,FetchWrite,MPI,PPI,PPIOverTcp
* 三菱PLC协议目前已经集成完毕（HSL中的）
* 欧姆龙PLC协议目前已经集成完毕（HSL中的）
* LSIS PLC协议目前已经集成完毕（HSL中的）
* 松下 PLC协议目前已经集成完毕（HSL中的）
* 基恩士 PLC协议目前已经集成完毕（HSL中的）
* 永宏 PLC协议目前已经集成完毕（HSL中的）
* AllenBradley PLC协议目前已经集成完毕（HSL中的）
* Fuji PLC协议目前已经集成完毕（HSL中的）
* 增加了opc ua协议的采集
* Haas CNC 的采集

  
目前正在开发设备的协议部分，协议部分集成完毕之后，开始开发web设置部分。

协议将不断完善中。

疑问和意向联系邮箱：betteriot@126.com

### 本产品的应用场景
![Image text](https://github.com/zhangkaigod2000/BetterIOT/blob/master/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20200609132812.png)  
边缘终端是先期开发的重点，发动到服务端之后，服务端的mqtt服务器来监听全部的数据，可以写入到influxdb，TDengine等时序库中，使用Grafana来进行快速的看板布置和实施数据的展示。  
中期将建立中心管理平台，针对集成到平台的终端进行一个统一的配置下发和管理，以及数据的可视化部分做一点所见即所得的管理定制界面。  
