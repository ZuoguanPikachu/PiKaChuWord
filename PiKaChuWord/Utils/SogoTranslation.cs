using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PiKaChuWord.Utils
{
    public class SogoTranslation
    {
        private MD5 md5 = MD5.Create();
        private Session session = new();
        private string secretCode;
        private string uuid;

        private void Init()
        {
            //获取secretCode和uuid
            string url = "https://fanyi.sogou.com/text";
            Dictionary<string, string> headers = new()
            {
                {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36"},
                {"Host", "fanyi.sogou.com"}
            };
            HttpContent content = session.Get(url, headers);
            string result = content.ReadAsStringAsync().Result;

            Match matched = Regex.Match(result, "\"secretCode\":(.*?),\"uuid\":\"(.*?)\",");
            secretCode = matched.Groups[1].Value;
            uuid = matched.Groups[2].Value;
        }

        public JToken Translate(string query)
        {
            string url = "https://fanyi.sogou.com/api/transpc/text/result";
            Dictionary<string, string> headers = new()
            {
                {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36"},
                {"Host", "fanyi.sogou.com"},
                {"Origin", "https://fanyi.sogou.com"},
                {"Referer", "https://fanyi.sogou.com/text"}
            };

            string from = "en";
            string to = "zh-CHS";

            byte[] sBytes = md5.ComputeHash(Encoding.UTF8.GetBytes($"{from}{to}{query}{secretCode}"));
            string s = "";
            foreach (byte b in sBytes)
            {
                s += b.ToString("X2").ToLower();
            }

            Dictionary<string, string> data = new()
            {
                {"from", from},
                {"to", to},
                {"text", query},
                {"client", "pc"},
                {"fr", "browser_pc"},
                {"needQc", "1" },
                {"s", s},
                {"uuid", uuid},
            };

            HttpContent content = session.Post(url, data, headers);
            JToken result = JObject.Parse(content.ReadAsStringAsync().Result)["data"];

            return result;
        }

        public Dictionary<string, string> Query(string query)
        {
            if (string.IsNullOrEmpty(secretCode))
            {
                Init();
            }

            JToken result = Translate(query);
            
            string translation = "";
            if ((result as JObject).Properties().Any(item => item.Name == "kaoyan"))
            {
                translation = string.Join("，", result["kaoyan"]["exam_freq_info"].Take(5).Select(
                    item => item["chinese"].Value<string>().Replace("; ", "，")
                ));
            }
            else
            {
                translation = result["translate"]["dit"].Value<string>();
            }

            string isWord = "√";
            if(query.Contains(' '))
            {
                foreach (string word in query.Split(' '))
                {
                    if(word.Length >= 2)
                    {
                        result = Translate(word);
                        if ((result["translate"] as JObject).Properties().Any(item => item.Name == "diff_text"))
                        {
                            isWord = "×";
                            break;
                        }
                    }
                }
            }
            else
            {
                if ((result["translate"] as JObject).Properties().Any(item => item.Name == "diff_text"))
                {
                    isWord = "×";
                }
            }

            return new (){{ "word_status", isWord}, {"translation", translation}};
        }
    }
}
