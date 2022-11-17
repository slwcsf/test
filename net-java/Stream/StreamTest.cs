using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using System.Net.Http;
using System.IO.Pipelines;

namespace com.sunlw.net.Tcp
{
    /// <summary>
    /// 流测试 具体参考https://www.cnblogs.com/crazytomato/p/8274838.html
    /// </summary>
    public class StreamTest
    {
        public static void TestTcp()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
            EndPoint point = new IPEndPoint(ipaddress, 9090);
            socket.Connect(point);//通过IP和端口号来定位一个所要连接的服务器端
            Stream netstream = new NetworkStream(socket);
            var _reader = new BufferedStream(netstream, 65535);
            var _writer = new BufferedStream(netstream, 65535);

            byte[] writeBytes = new byte[] { 1, 1, 1, 1 };
            _writer.Write(writeBytes, 0, 4);
            _writer.Flush();

            Thread.Sleep(500);
            byte[] readBytes = new byte[255];
            _reader.Read(readBytes, 0, 255);
            _reader.Flush();

            var srt = Encoding.UTF8.GetString(readBytes, 0, 255);
            FileStream fs = new FileStream("D:\\123.txt", FileMode.OpenOrCreate);
            var bs = new BufferedStream(fs);
            bs.Write(readBytes);
            bs.Write(Encoding.UTF8.GetBytes("\n"));
            bs.Write(Encoding.UTF8.GetBytes("你好"));
            bs.Flush();

            //FileStream fs2 = new FileStream("D:\\1234.txt", FileMode.OpenOrCreate);
            var sw = new StreamWriter("D:\\1234.txt", true);
            sw.WriteLine("你好");
            sw.WriteLine("你好");
            sw.Flush();
            Console.WriteLine(srt);
        }

        public static void TestTcp2()
        {
            //for (int i = 0; i < 1000; i++)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
            EndPoint point = new IPEndPoint(ipaddress, 4040);
            socket.Connect(point);
            //通过IP和端口号来定位一个所要连接的服务器端
            byte[] data = new byte[1024];
            //传递一个byte数组，用于接收数据。length表示接收了多少字节的数据
            socket.Send(Encoding.UTF8.GetBytes("add 1\r\n"));
            //socket.Send(Encoding.UTF8.GetBytes("\r\n"));
            var srt = Encoding.UTF8.GetString(data, 0, 255);
            //Stream netstream = new NetworkStream(socket);
            //var _reader = new BufferedStream(netstream, 2);
            //byte[] readBytes = new byte[2];
            //_reader.Read(readBytes, 0, 2);
            //var srt = Encoding.UTF8.GetString(readBytes, 0, 2);
            //Console.WriteLine(srt);
            //    });
            //}
        }

        public static void TestStream()
        {
            byte[] buffer = null;

            string testString = "Stream!Hello world";
            char[] readCharArray = null;
            byte[] readBuffer = null;
            string readString = string.Empty;
            //关于MemoryStream 我会在后续章节详细阐述
            using (MemoryStream stream = new MemoryStream())
            {
                Console.WriteLine("初始字符串为：{0}", testString);
                //如果该流可写
                if (stream.CanWrite)
                {
                    //首先我们尝试将testString写入流中
                    //关于Encoding我会在另一篇文章中详细说明，暂且通过它实现string->byte[]的转换
                    buffer = Encoding.Default.GetBytes(testString);
                    //我们从该数组的第一个位置开始写，长度为3，写完之后 stream中便有了数据
                    //对于新手来说很难理解的就是数据是什么时候写入到流中，在冗长的项目代码面前，我碰见过很
                    //多新手都会有这种经历，我希望能够用如此简单的代码让新手或者老手们在温故下基础
                    stream.Write(buffer, 0, 3);//Str

                    Console.WriteLine("现在Stream.Postion在第{0}位置", stream.Position + 1);

                    //从刚才结束的位置（当前位置）往后移3位，到第7位
                    long newPositionInStream = stream.CanSeek ? stream.Seek(3, SeekOrigin.Current) : 0;

                    Console.WriteLine("重新定位后Stream.Postion在第{0}位置", newPositionInStream + 1);
                    if (newPositionInStream < buffer.Length)
                    {
                        //将从新位置（第7位）一直写到buffer的末尾，注意下stream已经写入了3个数据“Str”
                        // stream.Write(buffer, (int)newPositionInStream, buffer.Length - (int)newPositionInStream);
                        stream.Write(buffer, 0, buffer.Length);
                    }

                    //写完后将stream的Position属性设置成0，开始读流中的数据
                    stream.Position = 0;

                    // 设置一个空的盒子来接收流中的数据，长度根据stream的长度来决定
                    readBuffer = new byte[stream.Length];

                    //设置stream总的读取数量 ，
                    //注意！这时候流已经把数据读到了readBuffer中
                    int count = stream.CanRead ? stream.Read(readBuffer, 0, readBuffer.Length) : 0;
                    //读完之后再读就读不到了
                    int count2 = stream.CanRead ? stream.Read(readBuffer, 0, readBuffer.Length) : 0;

                    //由于刚开始时我们使用加密Encoding的方式,所以我们必须解密将readBuffer转化成Char数组，这样才能重新拼接成string

                    //首先通过流读出的readBuffer的数据求出从相应Char的数量
                    int charCount = Encoding.Default.GetCharCount(readBuffer, 0, count);
                    //通过该Char的数量 设定一个新的readCharArray数组
                    readCharArray = new char[charCount];
                    //Encoding 类的强悍之处就是不仅包含加密的方法，甚至将解密者都能创建出来（GetDecoder()），
                    //解密者便会将readCharArray填充（通过GetChars方法，把readBuffer 逐个转化将byte转化成char，并且按一致顺序填充到readCharArray中）
                    Encoding.Default.GetDecoder().GetChars(readBuffer, 0, count, readCharArray, 0);
                    for (int i = 0; i < readCharArray.Length; i++)
                    {
                        readString += readCharArray[i];
                    }
                    Console.WriteLine("读取的字符串为：{0}", readString);
                }

                stream.Close();
            }

            Console.ReadLine();
        }

        public static void TestTestReader()
        {
            string text = "abc\nabc";

            using (TextReader reader = new StringReader(text))
            {
                while (reader.Peek() != -1)
                {
                    Console.WriteLine("Peek = {0}", (char)reader.Peek());
                    Console.WriteLine("Read = {0}", (char)reader.Read());
                }
                reader.Close();
            }

            using (TextReader reader = new StringReader(text))
            {
                char[] charBuffer = new char[3];
                int data = reader.ReadBlock(charBuffer, 0, 3);
                for (int i = 0; i < charBuffer.Length; i++)
                {
                    Console.WriteLine("通过readBlock读出的数据：{0}", charBuffer[i]);
                }
                reader.Close();
            }

            using (TextReader reader = new StringReader(text))
            {
                string lineData = reader.ReadLine();
                Console.WriteLine("第一行的数据为:{0}", lineData);
                reader.Close();
            }

            using (TextReader reader = new StringReader(text))
            {
                string allData = reader.ReadToEnd();
                Console.WriteLine("全部的数据为:{0}", allData);
                reader.Close();
            }

            Console.ReadLine();
        }

        public static void TestStreamReader()
        {
            //文件地址
            string txtFilePath = "D:\\TextReader.txt";
            //定义char数组
            char[] charBuffer2 = new char[3];

            //利用FileStream类将文件文本数据变成流然后放入StreamReader构造函数中
            using (FileStream stream = File.OpenRead(txtFilePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    //StreamReader.Read()方法
                    DisplayResultStringByUsingRead(reader);
                }
            }

            using (FileStream stream = File.OpenRead(txtFilePath))
            {
                //使用Encoding.ASCII来尝试下
                using (StreamReader reader = new StreamReader(stream, Encoding.ASCII, false))
                {
                    //StreamReader.ReadBlock()方法
                    DisplayResultStringByUsingReadBlock(reader);
                }
            }

            //尝试用文件定位直接得到StreamReader，顺便使用 Encoding.Default
            using (StreamReader reader = new StreamReader(txtFilePath, Encoding.Default, false, 123))
            {
                //StreamReader.ReadLine()方法
                DisplayResultStringByUsingReadLine(reader);
            }

            //也可以通过File.OpenText方法直接获取到StreamReader对象
            using (StreamReader reader = File.OpenText(txtFilePath))
            {
                //StreamReader.ReadLine()方法
                DisplayResultStringByUsingReadLine(reader);
            }
        }

        public static void TestStreamWriter()
        {
            NumberFormatInfo numberFomatProvider = new NumberFormatInfo();
            //将小数 “.”换成"?"
            numberFomatProvider.PercentDecimalSeparator = "?";
            StreamWriterTest test = new StreamWriterTest(Encoding.Default, "", numberFomatProvider);
            //StreamWriter
            test.WriteSomthingToFile();
            //TextWriter
            test.WriteSomthingToFileByUsingTextWriter();
            Console.ReadLine();
        }

        public static void TestMemoryStream()
        {
            //测试byte数组 假设该数组容量是256M
            byte[] testBytes = new byte[256 * 1024 * 1024];
            MemoryStream ms = new MemoryStream();
            using (ms)
            {
                for (int i = 0; i < 1000; i++)
                {
                    try
                    {
                        ms.Write(testBytes, 0, testBytes.Length);
                    }
                    catch
                    {
                        Console.WriteLine("该内存流已经使用了{0}M容量的内存,该内存流最大容量为{1}M,溢出时容量为{2}M",
                            GC.GetTotalMemory(false) / (1024 * 1024),//MemoryStream已经消耗内存量
                            ms.Capacity / (1024 * 1024), //MemoryStream最大的可用容量
                            ms.Length / (1024 * 1024));//MemoryStream当前流的长度（容量）
                        break;
                    }
                }
            }
            Console.ReadLine();
        }

        public static async Task Testpipe()
        {
            Pipe pipe = new Pipe();
            ReadOnlyMemory<byte> source = new ReadOnlyMemory<byte>(new byte[] { 1, 2, 3, 4, 5 });
            await pipe.Writer.WriteAsync(source);
            pipe.Writer.Advance(2);
            await pipe.Writer.WriteAsync(source);
            await pipe.Writer.FlushAsync();

            ReadResult a = await pipe.Reader.ReadAsync();

            var b = a.Buffer.GetPosition(5);
            //var b = a.Buffer;
            //var c = b.Start;
            //ReadOnlyMemory<byte> source2 = new ReadOnlyMemory<byte>();
            //b.TryGet(ref c, out source2);
            pipe.Reader.AdvanceTo(b);

            ReadResult c = await pipe.Reader.ReadAsync();

            //ReadOnlyMemory<byte> source3 = new ReadOnlyMemory<byte>();
            //b.TryGet(ref c, out source3);
        }

        /// <summary>
        /// 使用StreamReader.Read()方法
        /// </summary>
        /// <param name="reader"></param>
        private static void DisplayResultStringByUsingRead(StreamReader reader)
        {
            int readChar = 0;
            string result = string.Empty;
            while ((readChar = reader.Read()) != -1)
            {
                result += (char)readChar;
            }
            Console.WriteLine("使用StreamReader.Read()方法得到Text文件中的数据为 : {0}", result);
        }

        /// <summary>
        /// 使用StreamReader.ReadBlock()方法
        /// </summary>
        /// <param name="reader"></param>
        private static void DisplayResultStringByUsingReadBlock(StreamReader reader)
        {
            char[] charBuffer = new char[10];
            string result = string.Empty;
            reader.ReadBlock(charBuffer, 0, 10);
            for (int i = 0; i < charBuffer.Length; i++)
            {
                result += charBuffer[i];
            }
            Console.WriteLine("使用StreamReader.ReadBlock()方法得到Text文件中前10个数据为 : {0}", result);
        }

        /// <summary>
        /// 使用StreamReader.ReadLine()方法
        /// </summary>
        /// <param name="reader"></param>
        private static void DisplayResultStringByUsingReadLine(StreamReader reader)
        {
            int i = 1;
            string resultString = string.Empty;
            while ((resultString = reader.ReadLine()) != null)
            {
                Console.WriteLine("使用StreamReader.Read()方法得到Text文件中第{1}行的数据为 : {0}", resultString, i);
                i++;
            }
        }

        public static async Task TestTask()
        {
            await TestTask2();
            var t1 = Task.Run(async () =>
            {
                await Task.Delay(1000);
            });
            var t2 = Task.Run(async () =>
            {
                await Task.Delay(1000);
            });
            await Task.WhenAll(t1, t2);
        }

        private static async Task TestTask2()
        {
            var t1 = Task.Run(async () =>
            {
                await Task.Delay(1000);
            });
            var t2 = Task.Run(async () =>
            {
                await Task.Delay(1000);
            });
            await Task.WhenAll(t1, t2);
        }
    }

    public class StreamWriterTest
    {
        /// <summary>
        /// 编码
        /// </summary>
        private Encoding _encoding;

        /// <summary>
        /// IFomatProvider
        /// </summary>
        private IFormatProvider _provider;

        /// <summary>
        /// 文件路径
        /// </summary>
        private string _textFilePath;

        public StreamWriterTest(Encoding encoding, string textFilePath)
            : this(encoding, textFilePath, null)
        {
        }

        public StreamWriterTest(Encoding encoding, string textFilePath, IFormatProvider provider)
        {
            this._encoding = encoding;
            this._textFilePath = textFilePath;
            this._provider = provider;
        }

        /// <summary>
        ///  我们可以通过FileStream 或者 文件路径直接对该文件进行写操作
        /// </summary>
        public void WriteSomthingToFile()
        {
            //获取FileStream
            using (FileStream stream = File.OpenWrite(_textFilePath))
            {
                //获取StreamWriter
                using (StreamWriter writer = new StreamWriter(stream, this._encoding))
                {
                    this.WriteSomthingToFile(writer);
                }

                //也可以通过文件路径和设置bool append，编码和缓冲区来构建一个StreamWriter对象
                using (StreamWriter writer = new StreamWriter(_textFilePath, true, this._encoding, 20))
                {
                    this.WriteSomthingToFile(writer);
                }
            }
        }

        /// <summary>
        ///  具体写入文件的逻辑
        /// </summary>
        /// <param name="writer">StreamWriter对象</param>
        public void WriteSomthingToFile(StreamWriter writer)
        {
            //需要写入的数据
            string[] writeMethodOverloadType =
           {
              "1.Write(bool);",
              "2.Write(char);",
              "3.Write(Char[])",
              "4.Write(Decimal)",
              "5.Write(Double)",
              "6.Write(Int32)",
              "7.Write(Int64)",
              "8.Write(Object)",
              "9.Write(Char[])",
              "10.Write(Single)",
              "11.Write(Char[])",
              "12.Write(String)",
              "13Write(UInt32)",
              "14.Write(string format,obj)",
              "15.Write(Char[])"
           };

            //定义writer的AutoFlush属性，如果定义了该属性，就不必使用writer.Flush方法
            writer.AutoFlush = true;
            writer.WriteLine("这个StreamWriter使用了{0}编码", writer.Encoding.HeaderName);
            //这里重新定位流的位置会导致一系列的问题
            //writer.BaseStream.Seek(1, SeekOrigin.Current);
            writer.WriteLine("这里简单演示下StreamWriter.Writer方法的各种重载版本");

            writeMethodOverloadType.ToList().ForEach
                (
                    (name) => { writer.WriteLine(name); }
                );
            writer.WriteLine("StreamWriter.WriteLine()方法就是在加上行结束符，其余和上述方法是用一致");
            //writer.Flush();
            writer.Close();
        }

        public void WriteSomthingToFileByUsingTextWriter()
        {
            using (TextWriter writer = new StringWriter(_provider))
            {
                writer.WriteLine("这里简单介绍下TextWriter 怎么使用用户设置的IFomatProvider，假设用户设置了NumberFormatInfoz.PercentDecimalSeparator属性");
                writer.WriteLine("看下区别吧 {0:p10}", 0.12);
                Console.WriteLine(writer.ToString());
                writer.Flush();
                writer.Close();
            }
        }
    }

    public class Server
    {
        //端口
        private const int webPort = 80;

        //默认接收缓存大小
        private byte[] receiveBufferBytes = new byte[4096];

        //需要获取网页的url
        private string webPageURL;

        public Server(string webPageUrl)
        {
            webPageURL = webPageUrl;
        }

        /// <summary>
        ///  从该网页上获取数据
        /// </summary>
        public void FetchWebPageData()
        {
            if (!string.IsNullOrEmpty(webPageURL))
                FetchWebPageData(webPageURL);
            Console.ReadLine();
        }

        /// <summary>
        /// 从该网页上获取数据
        /// </summary>
        /// <param name="webPageURL">网页url</param>
        private void FetchWebPageData(string webPageURL)
        {
            //通过url获取主机信息
            IPHostEntry iphe = Dns.GetHostEntry(GetHostNameBystrUrl(webPageURL));
            Console.WriteLine("远程服务器名： {0}", iphe.HostName);
            //通过主机信息获取其IP
            IPAddress[] address = iphe.AddressList;
            IPEndPoint ipep = new IPEndPoint(address[0], 80);
            //实例化一个socket用于接收网页数据
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //连接
            socket.Connect(ipep);
            if (socket.Connected)
            {
                //发送头文件，这样才能下载网页数据
                socket.Send(Encoding.ASCII.GetBytes(this.GetHeader(webPageURL)));
            }
            else { return; }
            //接收头一批数据
            var count = socket.Receive(receiveBufferBytes);
            //转化成string
            var getString = Encoding.Default.GetString(receiveBufferBytes);
            //创建文件流
            FileStream fs = new FileStream(@"d:\\Test.html", FileMode.OpenOrCreate);
            //创建缓存流
            BufferedStream bs = new BufferedStream(fs);
            using (fs)
            {
                using (bs)
                {
                    byte[] finalContent = Encoding.Default.GetBytes(getString.ToCharArray());
                    //将头一批数据写入本地硬盘
                    bs.Write(finalContent, 0, finalContent.Length);
                    //循环通过socket接收数据
                    while (count > 0)
                    {
                        count = socket.Receive(receiveBufferBytes, receiveBufferBytes.Length, SocketFlags.None);
                        //直接将获取到的byte数据写入本地硬盘
                        bs.Write(receiveBufferBytes, 0, receiveBufferBytes.Length);
                        Console.WriteLine(Encoding.Default.GetString(receiveBufferBytes));
                    }
                    bs.Flush();
                    fs.Flush();
                    bs.Close();
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 得到header
        /// </summary>
        /// <param name="url">网页url</param>
        /// <returns>header字符串</returns>
        private string GetHeader(string webPageurl)
        {
            return "GET " + GetRelativeUrlBystrUrl(webPageurl) + " HTTP/1.1\r\nHost: "
                + GetHostNameBystrUrl(webPageurl) + "\r\nConnection: Close\r\n\r\n";
        }

        /// <summary>
        /// 得到相对路径
        /// </summary>
        /// <param name="strUrl">网页url</param>
        /// <returns></returns>
        private string GetRelativeUrlBystrUrl(string strUrl)
        {
            int iIndex = strUrl.IndexOf(@"//");
            if (iIndex <= 0)
                return "/";
            string strTemp = strUrl.Substring(iIndex + 2);
            iIndex = strTemp.IndexOf(@"/");
            if (iIndex > 0)
                return strTemp.Substring(iIndex);
            else
                return "/";
        }

        /// <summary>
        /// 根据Url得到host
        /// </summary>
        /// <param name="strUrl">网页url</param>
        /// <returns></returns>
        private string GetHostNameBystrUrl(string strUrl)
        {
            int iIndex = strUrl.IndexOf(@"//");
            if (iIndex <= 0)
                return "";
            string strTemp = strUrl.Substring(iIndex + 2);
            iIndex = strTemp.IndexOf(@"/");
            if (iIndex > 0)
                return strTemp.Substring(0, iIndex);
            else
                return strTemp;
        }
    }

    /// <summary>
    /// 服务端监听客户端信息，一旦有发送过来的信息，便立即处理
    /// </summary>
    internal class Program
    {
        //全局TcpClient
        private static TcpClient client;

        //文件流建立到磁盘上的读写流
        private static FileStream fs = new FileStream("E:\\abc.jpg", FileMode.Create);

        //buffer
        private static int bufferlength = 200;

        private static byte[] buffer = new byte[bufferlength];

        //网络流
        private static NetworkStream ns;

        private static void Main(string[] args)
        {
            ConnectAndListen();
        }

        private static void ConnectAndListen()
        {
            //服务端监听任何IP 但是端口号是80的连接
            TcpListener listener = new TcpListener(IPAddress.Any, 80);
            //监听对象开始监听
            listener.Start();
            while (true)
            {
                Console.WriteLine("等待连接");
                //线程会挂在这里，直到客户端发来连接请求
                client = listener.AcceptTcpClient();
                Console.WriteLine("已经连接");
                //得到从客户端传来的网络流
                ns = client.GetStream();
                //如果网络流中有数据
                if (ns.DataAvailable)
                {
                    //同步读取网络流中的byte信息
                    // do
                    //  {
                    //  ns.Read(buffer, 0, bufferlength);
                    //} while (readLength > 0);

                    //异步读取网络流中的byte信息
                    ns.BeginRead(buffer, 0, bufferlength, ReadAsyncCallBack, null);
                }
            }
        }

        /// <summary>
        /// 异步读取
        /// </summary>
        /// <param name="result"></param>
        private static void ReadAsyncCallBack(IAsyncResult result)
        {
            int readCount;
            //获得每次异步读取数量
            readCount = client.GetStream().EndRead(result);
            //如果全部读完退出，垃圾回收
            if (readCount < 1)
            {
                client.Close();
                ns.Dispose();
                fs.Dispose();
                return;
            }
            //将网络流中的图片数据片段顺序写入本地
            fs.Write(buffer, 0, 200);
            //再次异步读取
            ns.BeginRead(buffer, 0, 200, ReadAsyncCallBack, null);
        }
    }
}