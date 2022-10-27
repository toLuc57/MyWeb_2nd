using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MyWeb.Library
{
    public static class XString
    {
        private static string SignedToUnsigned(string s)
        {
            string signChars = "ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ";
            string unsignChars = "aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD";
            string result = "";
            foreach (var index in s)
            {
                var indexSign = signChars.IndexOf(index);
                if (indexSign > -1)
                {
                    result += unsignChars.ElementAt(indexSign);
                }
                else
                {
                    result += index;
                }
            }
            return result;
        }
        public static string Str_Slug(string s)
        {             
            string result = SignedToUnsigned(s).ToLower();
            result = Regex.Replace(result, "[\\s'\";,]", "-");
            return result;
        }
    }
}