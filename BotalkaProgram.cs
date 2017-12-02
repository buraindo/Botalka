using System;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Net;
using System.Text;

namespace Botalka
{
    public class BotalkaProgram
    {
        private DateTime startTime;
        private DateTime time;
        public WebClient client = new WebClient();
        public Form1 form;

        public void Init()
        {
            if (File.Exists("timeBackup"))
                File.Delete("timeBackup");
            File.Copy("time", "timeBackup");
            if (File.Exists("backup"))
                File.Delete("backup");
            File.Copy("C:\\Windows\\System32\\drivers\\etc\\hosts", "backup");
        }
        public void Run()
        {
            startTime = DateTime.Now;
            time = startTime;
            var writer = new StreamWriter("time");
            writer.Write(time);
            writer.Close();
            switch (form.comboBox1.SelectedItem)
            {
                case "1 час":
                    time = startTime.AddHours(1);
                    break;
                case "2 часа":
                    time = startTime.AddHours(2);
                    break;
                case "3 часа":
                    time = startTime.AddHours(3);
                    break;
                case "4 часа":
                    time = startTime.AddHours(4);
                    break;
                case "test":
                    time = startTime.AddSeconds(24);
                    break;
                default:
                    MessageBox.Show("Выбери время работы");
                    break;
            }
            if (time != startTime)
            {
                MessageBox.Show(
                    new StringBuilder(
                            $"Боталка включена, тебе предстоит {time.Hour * 60 - startTime.Hour * 60} минут ада")
                        .ToString());
                var linkFile = new StreamReader("link");
                var link = linkFile.ReadLine();
                if (File.Exists("forbidden"))
                    File.Delete("forbidden");
                client.DownloadFile(link, "forbidden");
                linkFile.Close();
                foreach (var line in File.ReadAllLines("forbidden"))
                {
                    File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", "127.0.0.1 " + line + "\n");
                }
                form.button1.Enabled = false;
                while (true)
                {
                    var diff = (time.Hour * 60 + time.Minute) * 60 + time.Second -
                               ((DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second);
                    var diffHr = diff / 3600;
                    var diffMin = (diff - diffHr * 3600) / 60;
                    var diffSec = diff - diffHr * 3600 - diffMin * 60;
                    form.textBox2.Text = diffHr + " : " +
                                         diffMin + " : " + diffSec;
                    if (time <= DateTime.Now)
                    {
                        File.Open("C:\\Windows\\System32\\drivers\\etc\\hosts", FileMode.Truncate).Dispose();
                        form.button1.Enabled = true;
                        MessageBox.Show(
                            "Мое время работы истекло: можешь отвлечься от учебы или повторно запустить меня");
                        File.AppendAllLines("C:\\Windows\\System32\\drivers\\etc\\hosts", File.ReadAllLines("backup"));
                        break;
                    }
                }
            }
        }
    }
}

