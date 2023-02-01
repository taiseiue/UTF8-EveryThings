using Hnx8.ReadJEnc;
using System.Reflection;
using System.Text;

namespace UTF8_EveryThings
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arg = new ParsedArguments(args);
            if(arg.Values.Keys.Contains("enc"))
            {
                OutputEncording = Encoding.GetEncoding(arg.Values["-enc"]);
            }
            SilentMode = arg.Flags.Contains("silent");

            Print("UTF-EveryThings バージョン"+Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Print("(c)Copylight 2022 WSOFT All rights reserved.");

            if (arg.Flags.Contains("analyze"))
            {
                //文字コード判定モード
                AnalyzeFile(arg.Files[0]);
                return;
            }
            if (arg.Flags.Contains("cat"))
            {
                //ファイル出力モード
                AnalyzeFile(arg.Files[0],true);
                return;
            }

            if (arg.Files.Count() > 1)
            {
                //入力と出力は異なるファイル
                if (SaveUTF8TextFile(arg.Files[0], arg.Files[1]))
                {
                    Print("[成功]変換済みのファイルを" + arg.Files[1] + "に書き出しました。");
                }
                else
                {
                    Print("[失敗]ファイルの変換に失敗しました。");
                }
            }
            else if(arg.Files.Count > 0)
            {
                //入力と出力は同じファイル
                if (SaveUTF8TextFile(arg.Files[0], arg.Files[0]))
                {
                    Print("[成功]変換済みのファイルで" + arg.Files[0]+"を置換しました。");
                }
                else
                {
                    Print("[失敗]ファイルの変換に失敗しました。");
                }
            }
            else
            {
                Console.WriteLine("UTF8-EveryThings 使用法");
                Console.WriteLine("utf8 ファイル名  : ファイルをUTF8形式のテキストファイルで置換します。");
                Console.WriteLine("utf8 ファイル名 出力先  : ファイルを読み込み、UTF8形式のテキストファイルに出力します。");
                Console.WriteLine("utf8 ファイル名  -enc:shift-jis : 指定した文字コードでテキストファイルを置換します。");
                Console.WriteLine("utf8 ファイル名  -cat : 指定したファイルを読み込み、内容を出力します。");
                Console.WriteLine("utf8 ファイル名  -analyze : 指定したファイルを読み込み、文字コードを出力します。");
                Console.WriteLine("utf8 ファイル名  -silent : 操作時に余分な出力をしません。");
            }
        }
        private static Encoding OutputEncording { get; set; } = Encoding.UTF8;
        private  static void AnalyzeFile(string target,bool cat=false)
        {
            var file = new FileInfo(target);
            if (file.Exists)
            {
                using (FileReader reader = new FileReader(file))
                {
                    Hnx8.ReadJEnc.CharCode c = reader.Read(file);
                    Print("検出された文字コードまたはファイル種別 : " + c.Name);
                    string name = c.Name;
                    string text = reader.Text;
                    if (cat && text != null)
                    {
                        Print("-----------------------------");
                        Console.WriteLine(text);
                    }
                }
            }
        }
        private static bool SaveUTF8TextFile(string from, string to)
        {
            var file = new FileInfo(from);
            if (file.Exists)
            {
                using (FileReader reader = new FileReader(file))
                {
                    Hnx8.ReadJEnc.CharCode c = reader.Read(file);
                    Print("検出された文字コードまたはファイル種別 : " + c.Name);
                    string name = c.Name;
                    string text = reader.Text;
                    if (text != null)
                    {
                        File.WriteAllText(to, text,OutputEncording);
                        return true;
                    }
                }
            }
            return false;
        }
        private static  bool SilentMode { get; set; }
        private  static void Print(string text)
        {
            if (!SilentMode)
            {
                Console.WriteLine(text);
            }
        }
    }
}