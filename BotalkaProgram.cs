using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Botalka
{
    public class BotalkaProgram
    {
        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        private DateTime startTime;
        private DateTime time;
        public WebClient client = new WebClient();
        public Form1 form;
        private StringBuilder builder = new StringBuilder(255);
        public static List<string> PhrasesList = new List<string> { "Нее, так это не работает", "Делу время, а потехе час", "Слыш работай", "Ботай, давай, может, и человеком станешь" };
        public bool canClose = true;

        public void Run()
        {
            startTime = DateTime.Now;
            time = startTime;
            var path = Environment.ExpandEnvironmentVariables("%AppData%\\Botalka");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (File.Exists(path + "\\backup"))
                File.Delete(path + "\\backup");
            File.Copy("C:\\Windows\\System32\\drivers\\etc\\hosts", path + "\\backup");
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
                var minutes = time.Hour >= startTime.Hour
                    ? time.Hour * 60 - startTime.Hour * 60
                    : (time.Hour + 24) * 60 - startTime.Hour * 60;
                MessageBox.Show(
                    new StringBuilder(
                            $"Боталка включена")
                        .ToString());
                canClose = false;
                var link = "https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1uVWZUZYrqYsD0oHLmMXF00gZ3KAt8Okr";
                if (File.Exists(path + "\\forbidden"))
                    File.Delete(path + "\\forbidden");
                client.DownloadFile(link, path + "\\forbidden");
                foreach (var line in File.ReadAllLines(path + "\\forbidden"))
                {
                    File.AppendAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", "\n" + "127.0.0.1\t" + line);
                }
                form.button1.Enabled = false;
                while (true)
                {
                    GetWindowText(GetForegroundWindow(), builder, 255);
                    if (builder.ToString().Equals("Диспетчер задач") || builder.ToString().Equals("Task manager"))
                    {
                        MessageBox.Show("Эй, нормально же общались, сверни это окошечко и продолжай ботать");
                    }
                    var diff = time.Hour >= DateTime.Now.Hour ? (time.Hour * 60 + time.Minute) * 60 + time.Second -
                               ((DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second) : ((time.Hour + 24) * 60 + time.Minute) * 60 + time.Second -
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
                        File.AppendAllLines("C:\\Windows\\System32\\drivers\\etc\\hosts", File.ReadAllLines(path + "\\backup"));
                        canClose = true;
                        break;
                    }
                }
            }
        }
    }
}

