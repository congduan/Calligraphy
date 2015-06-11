using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace 书法字库软件
{    
    /// <summary>
    /// 只能输入正负小数的文本输入框
    /// </summary>
    public class NumbersOnlyTextBox : TextBox
    {
        /// <summary>
        /// 依赖属性：是否允许输入负数，默认为允许
        /// </summary>
        public static readonly DependencyProperty AllowNegativeNumbersProperty =
            DependencyProperty.Register("AllowNegativeNumbers", typeof(bool), typeof(NumbersOnlyTextBox), new UIPropertyMetadata(true));

        public bool AllowNegativeNumbers
        {
            get { return (bool)GetValue(AllowNegativeNumbersProperty); }
            set { SetValue(AllowNegativeNumbersProperty, value); }
        }   

        public NumbersOnlyTextBox()
        {
            this.TextChanged += new TextChangedEventHandler(txtInput_TextChanged);
        }
        /// <summary>
        /// 正则表达式验证规则
        /// </summary>        
        private string pattern = @"^[\-]?[0-9]*([.]{1}[0-9]*){0,1}$";//匹配正负小数或整数//^[0-9]*$
        private string temp = String.Empty;
        /// <summary>
        /// 输入时进行验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            if(!AllowNegativeNumbers)
                pattern = @"^[0-9]*([.]{1}[0-9]*){0,1}$";
            Match m = Regex.Match(this.Text, pattern);   // 匹配正则表达式
            if (!m.Success)   // 输入的不是数字
            {
                this.Text = temp;   // textBox内容不变
                // 将光标定位到文本框的最后
                this.SelectionStart = this.Text.Length;
            }
            else   // 输入的是数字或其他特殊情况
            {                                
                temp = this.Text;   // 将现在textBox的值保存下来
            }
        }
    }
}
