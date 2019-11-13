namespace M.Core.Utils
{
    class RegexConstCharater
    {
        /// <summary>
        /// 只能输入数字： 
        /// </summary>
        public const string RegexNumber = "^[0-9]*$";
        /// <summary>
        /// 只能输入n位的数字：
        /// </summary>
        public const string RegexNumberN = @"^\d{n}$";
        /// <summary>
        /// 只能输入至少n位的数字：。
        /// </summary>
        public const string RegexNumberNL = @"^\d{n,}$";
        /// <summary>
        /// 只能输入m~n位的数字："^\d{m,n}$"。
        /// </summary>
        public const string RegexNumberNM = @"^\d{m,n}$";
        /// <summary>
        /// 只能输入零和非零开头的数字
        /// </summary>
        public const string RegexNumberZero = @"^(0|[1-9][0-9]*)$";
        /// <summary>
        /// 只能输入有两位小数的正实数
        /// </summary>
        public const string RegexNumberTwoDecimal = @"^[0-9]+(.[0-9]{2})?$";
        /// <summary>
        /// 只能输入有1~3位小数的正实数
        /// </summary>
        public const string RegexNumberOneToThreeDecimal = @"^[0-9]+(.[0-9]{1,3})?$";
        /// <summary>
        /// 只能输入非零的正整数
        /// </summary>
        public const string RegexNonZero = @"^\-[1-9][0-9]*$";
        /// <summary>
        /// 只能输入长度为3的字符
        /// </summary>
        public const string RegexThreeLength = @"^.{3}$";
        /// <summary>
        /// 只能输入由26个英文字母组成的字符串
        /// </summary>
        public const string RegexEnglishLetteraToZ = @"^[A-Za-z]+$";
        /// <summary>
        /// 只能输入由26个大写英文字母组成的字符串
        /// </summary>
        public const string RegexEnglishLetterAToZ = @"^[A-Z]+$";
        /// <summary>
        /// 只能输入由26个小写英文字母组成的字符串
        /// </summary>
        public const string RegexEnglishLetteraToz = @"^[a-z]+$";
        /// <summary>
        /// 只能输入由数字和26个英文字母组成的字符串
        /// </summary>
        public const string RegexNumberAndEnglishLetter = @"^[A-Za-z0-9]+$";
        /// <summary>
        /// 只能输入由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        public const string RegexNumberAndEnglishLetterAnd_ = @"^\w+$";
        /// <summary>
        /// 验证用户密码："^[a-zA-Z]\w{5,17}$"正确格式为：以字母开头，长度在6~18之间，只能包含字符、数字和下划线。
        /// </summary>
        public const string RegexPassword = @"^[a-zA-Z]\w{5,17}$";
        /// <summary>
        /// 验证是否含有。
        /// </summary>
        public const string RegexSpecialCharacter = @"[^%&’,;=?$\x22]+";
        /// <summary>
        /// 只能输入汉字
        /// </summary>
        public const string RegexChinese = @"^[\u4e00-\u9fa5]{0,}$";
        /// <summary>
        /// 验证Email地址
        /// </summary>
        public const string RegexEmail = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        /// <summary>
        /// 验证InternetURL
        /// </summary>
        public const string RegexInternetURL = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";
        /// <summary>
        /// 提取图片地址
        /// </summary>
        public const string RegexInternetImage = @"/(http(s)?\:\/\/)?(www\.)?(\w+\:\d+)?(\/\w+)+\.(png|gif|jpg|bmp|jpeg)/gi";
        /// <summary>
        /// 验证电话号码 正确格式为："XXX-XXXXXXX"、"XXXX-XXXXXXXX"、"XXX-XXXXXXX"、"XXX-XXXXXXXX"、"XXXXXXX"和"XXXXXXXX"。
        /// </summary>
        public const string RegexTelNumber = @"^(\(\d{3,4}-)|\d{3.4}-)?\d{7,8}$";
        /// <summary>
        /// 验证身份证号(15位或18位数字)
        /// </summary>
        public const string RegexCardNo = @"^\d{15}|\d{18}$";
        /// <summary>
        /// 验证一年的12个月 正确格式为："01"～"09"和"1"～"12"
        /// </summary>
        public const string RegexMonth = @"^(0?[1-9]|1[0-2])$";
        /// <summary>
        /// 验证一个月的31天 正确格式为;"01"～"09"和"1"～"31"
        /// </summary>
        public const string RegexDay = @"^((0?[1-9])|((1|2)[0-9])|30|31)$";
    }
}
