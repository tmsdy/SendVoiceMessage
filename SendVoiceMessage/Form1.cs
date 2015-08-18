using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace SendVoiceMessage
{
    public partial class Form1 : Form
    {
        public double iMaxPktLenth = 900.00;
        public Form1()
        {
            InitializeComponent();
        }

        private void pringASCII(string s2)
        {
            //将ASCII转16进制
       
            byte[] ba = System.Text.ASCIIEncoding.Default.GetBytes(s2);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ba)
            {
                sb.Append(b.ToString("x"));
            }
            richTextBox1.AppendText(sb.ToString());

        }


        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //设置打开文件的格式
            openFileDialog1.Filter = "录音文件(*.amr)|*.amr";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            richTextBox1.Text = string.Empty;
            //使用“打开”对话框中选择的文件名实例化FileStream对象
            FileStream myStream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
            int iStreamLength = (int) myStream.Length;
            byte[] byteFile = new byte[iStreamLength];

            double dPktPartCount = myStream.Length / iMaxPktLenth;
            //一共有iPktPartCount个包
            int iPktPartCount = Convert.ToInt32(Math.Ceiling(dPktPartCount));
            int iPktIndex = 0;
            string sTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            int iPktLenth = (int)iMaxPktLenth;
            string sPktPrefix = string.Empty;

            myStream.Read(byteFile, 0, iStreamLength);

            //关闭当前文件流
            myStream.Close();

            int j = 0;
            for (int i = 1; i <= iPktPartCount; i++)
            {
                richTextBox1.AppendText("一共"+iPktPartCount.ToString()+"个分包，当前第"+i.ToString()+"个分包\r\n"); 
                if ((i == iPktPartCount) && (iMaxPktLenth * i != iStreamLength))
                    iPktLenth = iStreamLength - (int)(iMaxPktLenth) * (i - 1);

                 
                sPktPrefix = "SWAP46," + textBox1.Text + "," + sTime + "," + iPktPartCount.ToString() +
                    "," + i.ToString() + "," + iPktLenth.ToString() + "," ;
                //richTextBox1.AppendText(sPktPrefix );

                //richTextBox1.AppendText("\r\n");
                pringASCII(sPktPrefix);
                for (j = 0; j < iPktLenth; j++, iPktIndex++)
                {
                    richTextBox1.AppendText(byteFile[iPktIndex].ToString("x2"));
                }
                pringASCII("#");
                richTextBox1.AppendText("\r\n");
                //iPktIndex += iPktLenth;

            }


            //foreach (byte b in byteFile)
            //{
            //    richTextBox1.AppendText(b.ToString());
            //}
        }
         
    }
}
