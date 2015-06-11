using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Threading;

namespace 书法字库软件
{
    class ExtensionMethods
    {
        /// <summary>
        /// 将值写入配置文件app.config
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">配置值</param>
        public static void WriteToAppConfig(string key, string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!cfa.AppSettings.Settings.AllKeys.Contains(key))
                cfa.AppSettings.Settings.Add(key, value);
            else
                cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();
        }

        /// <summary>
        /// 从app.config配置文件读取对应键的值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>配置值</returns>
        public static string ReadFromAppConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 获取图片尺寸大小
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public static Point GetImageSize(string strFilePath)
        {
        	Image pic=Image.FromFile(strFilePath);//strFilePath是该图片的绝对路径
			int intWidth=pic.Width;//长度像素值
			int intHeight=pic.Height;//高度像素值
			return new Point(intWidth,intHeight);
        }
        
        /// <summary>
        /// 获取图片的
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static double GetImageSizeProportion(string strFilePath)
        {
        	Point size = GetImageSize(strFilePath);
        	return (double)size.X/(double)size.Y;
        }
    }
}
