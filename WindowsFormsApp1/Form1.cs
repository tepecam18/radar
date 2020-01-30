using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Port port = new Port();

        public string portDurum;
        string[] veri = new string[3];
        string[] satır;
        string str;


        Pen yesil = new Pen(Color.FromArgb(37, 245, 4), 3);
        Pen Bos = new Pen(Color.FromArgb(150, 37, 245, 4), 6);
        Pen Dolu = new Pen(Color.Red, 7);
        Brush fırca = new SolidBrush(Color.FromArgb(22, Color.Black));

        Graphics grafik;

        Thread th1;

        double sayı = 1;
        int hipotenüs = 700;

        private void Form1_Load(object sender, EventArgs e)
        {
            port.portName.DataSource = SerialPort.GetPortNames();
            ManuelPort();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
            try
            {
                th1.Abort();
            }
            catch (Exception) { }
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            label1.BackColor = Color.DarkRed;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.ForeColor = panel1.BackColor;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = panel1.BackColor;
            label1.BorderStyle = BorderStyle.None;
            label1.ForeColor = durdur.ForeColor;
        }

        void Başladı()
        {
            th1 = new Thread(RadarBaşlat);
            th1.Start();
        }

        void Bitti()
        {
            try
            {
                serialPort.Write("Q");
                th1.Abort();
            }
            catch (Exception)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Başladı();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitti();
        }

        void ManuelPort()
        {
            try
            {
                port.portName.SelectedIndex = 0;
            }
            catch (Exception) { }

            port.baudeRate.SelectedIndex = 11;
            portDurum = port.ShowDialog().ToString();
            if (portDurum == "Cancel")
            {
                this.Close();
            }
            else if (portDurum == "OK")
            {
                try
                {
                    serialPort.PortName = port.portName.SelectedItem.ToString();
                    serialPort.BaudRate = Convert.ToInt32(port.baudeRate.SelectedItem);
                }
                catch (Exception)
                {
                    MessageBox.Show("Eksik veya Geçersiz Veri Girişinde Bulundunuz!!!\nOturum, Otomatik Olarak Kendini Kapatacaktır. ", "Dikkat");
                    this.Close();
                }
                try
                {
                    serialPort.Open();
                }
                catch (Exception)
                {
                    MessageBox.Show("Eksik veya Geçersiz Veri Girişinde Bulundunuz!!!\nOturum, Otomatik Olarak Kendini Kapatacaktır. ", "Dikkat");
                    this.Close();
                }
            }
        }

        void RadarBaşlat()
        {
            serialPort.Write("O");
            if (serialPort.IsOpen)
            {
                while (true)
                {
                    str = serialPort.ReadLine();
                    satır = str.Split('.');
                    foreach (var data in satır)
                    {
                        try
                        {
                            if (data != "\r")
                            {
                                veri = data.Split(',');
                                RadarCiz(Convert.ToInt32(veri[0]), Convert.ToInt32(veri[1]));
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

            }
            else
            {
                if (MessageBox.Show("Hatalı port yada BaudRate girdiniz", "Dikkat", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    serialPort.Write("Q");
                    serialPort.ReadExisting();
                    RadarBaşlat();
                }
                else
                {
                    Bitti();
                }
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            canvasTriger();
        }

        void canvasTriger()

        {
            Point nokta1 = new Point(canvas.Width / 2, canvas.Height);
            grafik = canvas.CreateGraphics();
            YuvarlakCiz(0);
            YuvarlakCiz(150);
            YuvarlakCiz(300);
            YuvarlakCiz(450);
            grafik.DrawLine(yesil, 0, canvas.Height, canvas.Width, canvas.Height);
            grafik.DrawLine(yesil, nokta1, NoktaBul(30));
            grafik.DrawLine(yesil, nokta1, NoktaBul(60));
            grafik.DrawLine(yesil, nokta1, NoktaBul(90));
            grafik.DrawLine(yesil, nokta1, NoktaBul(120));
            grafik.DrawLine(yesil, nokta1, NoktaBul(150));
        }

        Point NoktaBul(int acı)
        {
            int x = Convert.ToInt32(canvas.Width / 2 + Math.Cos(DereceToRadian(acı)) * hipotenüs);
            int y = Convert.ToInt32(canvas.Height - Math.Sin(DereceToRadian(acı)) * hipotenüs);
            Point point = new Point(x, y);
            return point;

        }

        void RadarCiz(int acı, int uzaklık)
        {

            switch (uzaklık / 10)
            {
                case 0:
                    sayı = 30.0 / uzaklık;
                    break;
                case 1:
                    sayı = 35.0 / uzaklık;
                    break;
                case 2:
                    sayı = 40.0 / uzaklık;
                    break;
                case 3:
                    sayı = 42.0 / uzaklık;
                    break;
                case 4:
                    sayı = 43.0 / uzaklık;
                    break;
                default:
                    sayı = 44.0 / uzaklık;
                    break;
            }

            Point point1 = new Point(canvas.Width / 2, canvas.Height);
            Point point2 = NoktaBul(acı);
            Point point3 = new Point(
                Convert.ToInt32(canvas.Width / 2 + Math.Cos(DereceToRadian(acı)) * hipotenüs / sayı), 
                Convert.ToInt32(canvas.Height - Math.Sin(DereceToRadian(acı)) * hipotenüs / sayı));

            grafik.DrawLine(Bos, point1, point2);
            grafik.DrawLine(Dolu, point3, point2);

            canvasTriger();

            grafik.FillRectangle(fırca, 0, 0, 1350, 700);
        }

        void YuvarlakCiz(int x)
        {
            Rectangle rect = new Rectangle(23 + x, 40 + x, (1300 - (x * 2)), (630 - x) * 2);
            grafik.DrawArc(yesil, rect, 170, 200);
        }

        double DereceToRadian(double x)
        {
            return Math.PI * x / 180.0;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                serialPort.Write("QQQQQ");
                serialPort.Close();
            }
            catch (Exception) { }
        }
    }
}
