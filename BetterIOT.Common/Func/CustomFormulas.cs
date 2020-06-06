using Jint;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetterIOT.Common.Func
{
    public class CustomFormulas
    {
        Jint.Engine js = new Engine();

        public CustomFormulas()
        {

        }
        /// <summary>
        /// 自定义公式运算
        /// </summary>
        /// <param name="strFormulas">自定义的算式字符串</param>
        /// <returns>运算结果</returns>
        public string Computing(string strFormulas)
        {
            string strResult = "";
            try
            {

                //////////临时处理//////////
                strFormulas = strFormulas.Replace("-*", "*");
                strResult = js.Execute(strFormulas).Invoke("jsfun").ToString();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return strResult;
        }
    }
}
