# BetterIOT
一个适合在arm和X86边缘计算设备上运行的工业设备数据采集系统

助力AIOT事业，让C#写的采集程序借助于netCore可以运行在基于ubuntu的arm设备上及运行以及X86架构的windows和ubuntu上，可以局域网、互联网使用。
借助于dathlin/HslCommunication这个项目来实现部分设备的访问。借助于MQTT来实现设备数据的对外通讯部分。
目前集成的采集驱动有：
modbusrtu,DLT645 2007,GBT26875.3 城市消防远程监控系统 报警传输网络通信协议 中的部分数据
西门子 S1200,S400,S300,S1500,S200,S200Smart,FetchWrite,MPI,PPI,PPIOverTcp

协议将不断完善中。
