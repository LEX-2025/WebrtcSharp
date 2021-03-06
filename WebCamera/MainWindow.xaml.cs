﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using WebrtcSharp;

namespace WebCamera
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OpenCamera();
        }

        private PeerConnectionFactory factory = new PeerConnectionFactory();

        private void OpenCamera()
        {
            if (source != null) source.Frame -= Source_Frame;
            source?.Release();
            source = factory.CreateVideoSource(cameraIndex, 1600, 1200, 30);
            if (source == null) return;
            source.Frame += Source_Frame;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (source != null)
            {
                source.Frame -= Source_Frame;
                source.Release();
                source = null;
            }
            base.OnClosing(e);
        }

        /// <summary>
        /// 计算YUV420视频帧占用的内存大小
        /// </summary>
        /// <param name="strideY">Y通道宽度</param>
        /// <param name="width">视频宽度</param>
        /// <param name="height">视频高度</param>
        /// <returns></returns>
        private int GetMemeryCount(int strideY, int width, int height)
        {
            int number = width / strideY;
            if (number < 1) number = 1;
            return strideY * height / number;
        }

        private void Source_Frame(VideoFrame obj)
        {
            var lenY = GetMemeryCount(obj.StrideY, obj.Width, obj.Height);
            var lenU = GetMemeryCount(obj.StrideU, obj.Width, obj.Height);
            var lenV = GetMemeryCount(obj.StrideV, obj.Width, obj.Height);
            var lenA = 0;
            if (obj.StrideA > 0) lenA = GetMemeryCount(obj.StrideA, obj.Width, obj.Height);

            OpenCvSharp.Mat yuvImg = new OpenCvSharp.Mat();
            yuvImg.Create((obj.Height * 3 / 2), obj.Width, OpenCvSharp.MatType.CV_8UC1);

            unsafe
            {
                Buffer.MemoryCopy(obj.DataY.ToPointer(), yuvImg.Data.ToPointer(), lenY, lenY);
                Buffer.MemoryCopy(obj.DataU.ToPointer(), (byte*)yuvImg.Data.ToPointer() + lenY, lenU, lenU);
                Buffer.MemoryCopy(obj.DataV.ToPointer(), (byte*)yuvImg.Data.ToPointer() + lenY + lenU, lenV, lenV);
                if (lenA > 0) Buffer.MemoryCopy(obj.DataA.ToPointer(), (byte*)yuvImg.Data.ToPointer() + lenY + lenU + lenV, lenA, lenA);
            }
            var target = new OpenCvSharp.Mat();
            OpenCvSharp.Cv2.CvtColor(yuvImg, target, OpenCvSharp.ColorConversionCodes.YUV2BGRA_I420);
            Dispatcher.BeginInvoke((Action)(() =>
            {
                display.Source = OpenCvSharp.Extensions.BitmapSourceConverter.ToBitmapSource(target);
                target.Dispose();
            }));
        }

        private VideoSource source;

        private int cameraIndex = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var devices = PeerConnectionFactory.GetDeviceInfo();
            ++cameraIndex;
            if (cameraIndex >= devices.Length) cameraIndex = 0;
            OpenCamera();
        }
    }
}
