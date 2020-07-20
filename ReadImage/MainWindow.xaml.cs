using HalconDotNet;
using Microsoft.Win32;
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

namespace ReadImage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public OpenFileDialog openFileDialog = null;
        public HObject ho_Image, ho_GrayImage, ho_Regions;
        public HTuple hv_WindowHandle = null;
        public MainWindow()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog();
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (hv_WindowHandle!=null)
            {
                HOperatorSet.CloseWindow(hv_WindowHandle);  //执行清空控件窗口
            }
            
            //对话框设置
            string ImagePath;
            openFileDialog.Filter = "JPEG文件|*.jpg|BMP文件|*.bmp|png文件|*.png";
            openFileDialog.RestoreDirectory = true;

            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            ho_Image.Dispose();
            if (openFileDialog.ShowDialog() == true)
            {
                //将打开的文件名赋给ImagePath字符串变量               
                ImagePath = openFileDialog.FileName;
                HOperatorSet.ReadImage(out ho_Image, ImagePath);
            }

            //打开窗口，将显示图片控件的句柄赋给窗口句柄，使窗口在控件中打开，并设置窗口宽高为显示图片控件的宽高
            HOperatorSet.OpenWindow(0, 0, HWCCameraImage.Width, HWCCameraImage.Height, HWCCameraImage.HalconWindow, "", "", out hv_WindowHandle);
            HDevWindowStack.Push(hv_WindowHandle);

            
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
            }
        }

        private void BtnGrayOP_Click(object sender, RoutedEventArgs e)
        {
            ho_GrayImage.Dispose();
            HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.DispObj(ho_GrayImage, HDevWindowStack.GetActive());
            }
            
        }

        private void BtnThresholdOP_Click(object sender, RoutedEventArgs e)
        {
            ho_Regions.Dispose();
            HOperatorSet.Threshold(ho_GrayImage, out ho_Regions, 10, 95);
            if (HDevWindowStack.IsOpen())
            {
                HOperatorSet.DispObj(ho_Regions, HDevWindowStack.GetActive());
            }
        }
    }
}
