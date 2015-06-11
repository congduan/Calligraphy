using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Aspose.Words;
using MessageBox = System.Windows.MessageBox;

namespace 书法字库软件
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window, INotifyPropertyChanged
    {
        #region 字段
        private string _currentDir = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
        private float defaultFont = 50f;//默认字体和图片大小
        private float _imageWidth, _imageHeight;
        #endregion

        /// <summary>
        /// 默认字库目录
        /// </summary>
        public string CurrentDir
        {
            set
            {
                _currentDir = value;
                ExtensionMethods.WriteToAppConfig("CurrentDir",value);
                OnPropertyChanged("CurrentDir");
            }
            get { return _currentDir; }
        }

        public float ImageWidth
        {
            set
            {
                _imageWidth = value;
                //ExtensionMethods.WriteToAppConfig("ImageWidth", value.ToString());
                //OnPropertyChanged("ImageWidth");
            }
            get { return _imageWidth; }
        }

        public float ImageHeight
        {
            set
            {
                _imageHeight = value;
                //ExtensionMethods.WriteToAppConfig("ImageHeight", value.ToString());
                //OnPropertyChanged("ImageWidth");
            }
            get { return _imageHeight; }
        }

        /// <summary>
        /// 写入图片大小配置
        /// </summary>
        private void WriteImageWidthAndHeight()
        {           
            if (!string.IsNullOrEmpty(imgHeight.Text))
            {
                ExtensionMethods.WriteToAppConfig("ImageHeight", imgHeight.Text);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// 清空文字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            calligraphyText.Text = "";            
        }

        
        /// <summary>
        /// 生成带图片Word文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doBtn_Click(object sender, RoutedEventArgs e)
        {    
            DocumentBuilder builder = new DocumentBuilder();        
            try
            {                                   
                //分析字符串插入图片
                string strPictureName_bmp,strPictureName_jpg,strPictureName_png;
                foreach (var hanzi in calligraphyText.Text)
                {
                    strPictureName_bmp = CurrentDir + hanzi.ToString() + ".bmp";
                    strPictureName_jpg = CurrentDir + hanzi.ToString() + ".jpg";
 					strPictureName_png = CurrentDir + hanzi.ToString() + ".png";
                    if (strPictureName_bmp != "" && System.IO.File.Exists(strPictureName_bmp))
                    {
                        builder.InsertImage(strPictureName_bmp, 
    	                    imgHeight.Text == ""
    	                    	? ExtensionMethods.GetImageSizeProportion(strPictureName_bmp)*defaultFont
    	                    	: ExtensionMethods.GetImageSizeProportion(strPictureName_bmp)*float.Parse(imgHeight.Text),
                            imgHeight.Text == ""? defaultFont: float.Parse(imgHeight.Text));
                    }
                    else if (strPictureName_png != "" && System.IO.File.Exists(strPictureName_png))
                    {
                        builder.InsertImage(strPictureName_png,
                             imgHeight.Text == ""
    	                    	? ExtensionMethods.GetImageSizeProportion(strPictureName_bmp)*defaultFont
    	                    	: ExtensionMethods.GetImageSizeProportion(strPictureName_bmp)*float.Parse(imgHeight.Text),
                            imgHeight.Text == "" ? defaultFont : float.Parse(imgHeight.Text));
                    }
                 	else if (strPictureName_jpg != "" && System.IO.File.Exists(strPictureName_jpg))
                    {
                        builder.InsertImage(strPictureName_jpg,
                             imgHeight.Text == ""
    	                    	? ExtensionMethods.GetImageSizeProportion(strPictureName_bmp)*defaultFont
    	                    	: ExtensionMethods.GetImageSizeProportion(strPictureName_bmp)*float.Parse(imgHeight.Text),
                            imgHeight.Text == "" ? defaultFont : float.Parse(imgHeight.Text));
                    }
                    else
                    {
                        builder.Font.Size = (imgHeight.Text == ""? defaultFont: float.Parse(imgHeight.Text));
                        builder.Write(hanzi.ToString());
                    }
                }
                WriteImageWidthAndHeight();

                var dialog = new SaveFileDialog
                                 {
                                     Filter = "Word97-2003文件(*.doc)|*.doc|所有文件(*.*)|*.*",
                                     DefaultExt = "doc",
                                     Title = "创建要保存的Word文档",
                                     AddExtension = true
                                 };
                DialogResult result = dialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;               
                builder.Document.Save(dialog.FileName);
                MessageBox.Show("操作成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        /// <summary>
        /// 选择目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //MessageBox.Show(folderDlg.SelectedPath);
                CurrentDir = folderDlg.SelectedPath+@"\";
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members

        private void window_Closing(object sender, CancelEventArgs e)
        {
            
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentDir = ExtensionMethods.ReadFromAppConfig("CurrentDir");
            ImageHeight = float.Parse(ExtensionMethods.ReadFromAppConfig("ImageHeight"));
            ImageWidth = float.Parse(ExtensionMethods.ReadFromAppConfig("ImageWidth"));
            imgHeight.Text = ImageHeight.ToString();            
        }
    }
}