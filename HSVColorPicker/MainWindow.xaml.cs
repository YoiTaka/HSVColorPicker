using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HSVColorPicker
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        //初期値
        private int H_MinValue = 89;
        private int S_MinValue = 197;
        private int V_MinValue = 144;
        private int H_MaxValue = 107;
        private int S_MaxValue = 255;
        private int V_MaxValue = 246;

        Scalar lowerColor;
        Scalar upperColor;
        Mat dstImg;
        Mat hsvImg;

        public MainWindow()
        {
            InitializeComponent();
            H_Min_Slider.Value = H_MinValue;
            S_Min_Slider.Value = S_MinValue;
            V_Min_Slider.Value = V_MinValue;
            H_Max_Slider.Value = H_MaxValue;
            S_Max_Slider.Value = S_MaxValue;
            V_Max_Slider.Value = V_MaxValue;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "image files (*.jpg, *.png)|*.jpg;*.png|All files (*.*)|*.*";
            //"1"ならば、image filesが設定される
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;


            if (openFileDialog.ShowDialog() == true)
            {
                Mat srcImg = new Mat(openFileDialog.FileName);

                hsvImg = new Mat();
                ////BGR -> HSV
                Cv2.CvtColor(srcImg, hsvImg, ColorConversionCodes.BGR2HSV);

                dstImg = new Mat();

                lowerColor = new Scalar(H_MinValue, S_MinValue, V_MinValue);
                upperColor = new Scalar(H_MaxValue, S_MaxValue, V_MaxValue);

                Cv2.InRange(hsvImg, lowerColor, upperColor, dstImg);

                srcImage.Source = srcImg.ToWriteableBitmap();
                dstImage.Source = dstImg.ToWriteableBitmap();
            }
        }

        private void Slider_HandValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider selectSlider = (Slider)sender;
            string tagname = selectSlider.Tag.ToString();

            switch (tagname)
            {
                case "H_Min":
                    H_MinValue = (int)H_Min_Slider.Value;
                    break;
                case "S_Min":
                    S_MinValue = (int)S_Min_Slider.Value;

                    break;
                case "V_Min":
                    V_MinValue = (int)V_Min_Slider.Value;
                    break;
                case "H_Max":
                    H_MaxValue = (int)H_Max_Slider.Value;
                    break;
                case "S_Max":
                    S_MaxValue = (int)S_Max_Slider.Value;
                    break;
                case "V_Max":
                    V_MaxValue = (int)V_Max_Slider.Value;
                    break;
            }

            HValueRange.Content = "H: " + H_MinValue + "～" + H_MaxValue;
            SValueRange.Content = "S: " + S_MinValue + "～" + S_MaxValue;
            VValueRange.Content = "V: " + V_MinValue + "～" + V_MaxValue;

            lowerColor = new Scalar(H_MinValue, S_MinValue, V_MinValue);
            upperColor = new Scalar(H_MaxValue, S_MaxValue, V_MaxValue);


            if (hsvImg == null) return;
            Cv2.InRange(hsvImg, lowerColor, upperColor, dstImg);
            dstImage.Source = dstImg.ToWriteableBitmap();
        }

    }
}
