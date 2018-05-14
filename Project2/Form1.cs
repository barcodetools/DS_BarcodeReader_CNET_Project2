using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BarcodeReaderLib.BarcodeDecoder barcodeDecoder = new BarcodeReaderLib.BarcodeDecoder();
            barcodeDecoder.LinearFindBarcodes = 7;
            barcodeDecoder.DecodeFile(textBox1.Text);

            //show results
            BarcodeReaderLib.BarcodeList BarcodeList = barcodeDecoder.Barcodes;
            for (int i = 0; i < BarcodeList.length; i++)
            {
                BarcodeReaderLib.Barcode barcode = BarcodeList.item(i);
                string sResult = string.Format("{0}\n{1}\n({2},{3}),({4},{5}),({6},{7}),({8},{9})\nPage: {10}", barcode.BarcodeType.ToString(), barcode.Text, barcode.x1, barcode.y1, barcode.x2, barcode.y2, barcode.x3, barcode.y3, barcode.x4, barcode.y4, barcode.PageNum);
                MessageBox.Show(sResult);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BarcodeReaderLib.BarcodeDecoder barcodeDecoder = new BarcodeReaderLib.BarcodeDecoder();
            barcodeDecoder.AboutBox();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BarcodeReaderLib.BarcodeDecoder barcodeDecoder = new BarcodeReaderLib.BarcodeDecoder();

            Bitmap bmp = new Bitmap(textBox1.Text);
            int w = bmp.Width, h = bmp.Height, s = w * h;
            byte[] ba = new byte[s];

            Rectangle rect = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bitmapData = new System.Drawing.Imaging.BitmapData();
            bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb, bitmapData);
            int bytes24 = bitmapData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes24];
            //Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, rgbValues, 0, bytes24);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int index = ((y * bmp.Width + x) * 3);
                    ba[x + y * w] = (byte)((299 * rgbValues[index + 2] + 587 * rgbValues[index + 1] + 114 * rgbValues[index]) / 1000);
                }
            }

            // Unlock the bits.
            bmp.UnlockBits(bitmapData);

            bmp.Dispose();

            object obj = (object)ba;
            barcodeDecoder.DecodeGrayMap(obj, w, h);

            //show results
            BarcodeReaderLib.BarcodeList BarcodeList = barcodeDecoder.Barcodes;
            for (int i = 0; i < BarcodeList.length; i++)
            {
                BarcodeReaderLib.Barcode barcode = BarcodeList.item(i);
                string sResult = string.Format("{0}\n{1}\n({2},{3}),({4},{5}),({6},{7}),({8},{9})\nPage: {10}", barcode.BarcodeType.ToString(), barcode.Text, barcode.x1, barcode.y1, barcode.x2, barcode.y2, barcode.x3, barcode.y3, barcode.x4, barcode.y4, barcode.PageNum);
                MessageBox.Show(sResult);
            }
        }
    }
}