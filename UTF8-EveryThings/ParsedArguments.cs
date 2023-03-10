using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTF8_EveryThings
{
    class ParsedArguments
    {
        public ParsedArguments(string[] args)
        {
            m_init(args);
        }
        public ParsedArguments(string args)
        {
            m_init(args.Split(new string[] { "\r\n" }, StringSplitOptions.None));
        }
        private void m_init(string[] args)
        {
            bool aarg = false;
            foreach (string arg in args)
            {
                if (arg.ToLower() == "--arg")
                {
                    aarg = true;
                    continue;
                }
                if (aarg)
                {
                    Args.Add(arg);
                }
                else
                {
                    System.Text.RegularExpressions.MatchCollection mc =
        System.Text.RegularExpressions.Regex.Matches(
        arg, @"-.*");
                    if (mc.Count > 0)
                    {
                        foreach (System.Text.RegularExpressions.Match m in mc)
                        {
                            System.Text.RegularExpressions.MatchCollection mc2 =
            System.Text.RegularExpressions.Regex.Matches(
            arg, @"-.*:.*");
                            if (mc2.Count > 0)
                            {
                                foreach (System.Text.RegularExpressions.Match m2 in mc2)
                                {
                                    string v = m2.Value;
                                    v = v.TrimStart('-'); string[] vs = v.Split(':');
                                    Values.Add(vs[0].ToLower(), vs[1]);
                                }
                            }
                            else
                            {
                                string v = m.Value;
                                v = v.TrimStart('-');
                                Flags.Add(v.ToLower());
                            }
                        }
                    }
                    else
                    {
                        Files.Add(arg);
                    }
                }
            }
        }
        private Dictionary<string, string> m_values = new Dictionary<string, string>();
        public List<string> Args = new List<string>();
        public Dictionary<string, string> Values
        {
            get
            {
                return m_values;
            }
            set
            {
                m_values = value;
            }
        }
        private List<string> m_flags = new List<string>();
        public List<string> Flags
        {
            get
            {
                return m_flags;
            }
            set
            {
                m_flags = value;
            }
        }
        private List<string> m_files = new List<string>();
        public List<string> Files
        {
            get
            {
                return m_files;
            }
            set
            {
                m_files = value;
            }
        }
    }
}
