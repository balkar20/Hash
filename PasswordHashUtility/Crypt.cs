using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace PasswordHashUtility;

public class Crypt
{
    private int NUM1;
    private int NUM2;
    private int NUM3;
    private byte[] Vec;
    private byte[] Y;
    private int J;
    private int K;

    public Crypt()
    {
        NUM1 = 100069;
        NUM2 = 100103;
        NUM3 = 100109;
        Vec = new byte[55];
        Y = new byte[55];
    }

    private void Usekey(string s)
    {
        int i;
        int seed = 0;

        if (s.Length == 0)
            s = "cumberland";

        for (i = 0; i < s.Length; i++)
        {
            seed += s[i];
            seed = (NUM1 + NUM2 * seed) % NUM3;
        }
        for (i = 0; i != 55; ++i)
        {
            seed = (NUM1 + NUM2 * seed) % NUM3;
            Y[i] = Vec[i] = (byte)seed;
        }
        J = 23;
        K = 54;
    }

    private byte CryptChar()
    {
        byte ch = (byte)'\0';
        Y[K] += Y[J];
        ch ^= Y[K];
        if (--J == -1)
            J = 54;
        if (--K == -1)
            K = 54;
        return ch;
    }

    public string DecryptString(string key, string str)
    {
        string rval = "";

        if (str != null && str.Length > 0)
        {
            int ch;

            Usekey(key);

            for (int i = 0; i < str.Length; i++)
            {
                ch = ((byte)str[i] - CryptChar());
                while (ch < (int)' ')
                    ch += 95;   // Loops back to the previous printable character.
                rval = rval + ((char)ch).ToString();
            }
        }
        return rval;
    }

    public string EncryptString(string key, string str)
    {
        string rval = "";

        if (str != null && str.Length > 0)
        {
            char ch;

            Usekey(key);

            for (int i = 0; i < str.Length; i++)
            {
                ch = (char)((byte)str[i] + CryptChar());
                while (ch > (byte)'~')
                    ch -= (char)95; // Loops back to the previous printable character.
                rval = rval + ((char)ch).ToString();
            }
        }
        return rval;
    }

    public string EncryptPassword(string key, string str)
    {
        if (str == null)
            return "";

        string buf = EncryptString(key.ToLower(), str);

        buf = buf.Replace("~", "\t");
        return buf + ".";
    }

    public string DecryptPassword(string key, string str)
    {
        string rval = "";
        if (str != null && str.Length > 0)
        {
            string pbuf = str.Substring(0, str.Length - 1);
            pbuf = pbuf.Replace("\t", "~");

            rval = DecryptString(key, pbuf);
        }
        return rval;
    }

    public string GenerateHashPassword(string pLoginName, string pPassword)
    {
        SHA1Managed shaobj = new SHA1Managed();
        byte[] hashValue;
        string strHex = "";
        SecureString encodedText = new SecureString();
        encodedText.AppendChar((char)0x49);
        encodedText.AppendChar((char)0x6E);
        encodedText.AppendChar((char)0x74);
        encodedText.AppendChar((char)0x65);
        encodedText.AppendChar((char)0x67);
        encodedText.AppendChar((char)0x72);
        encodedText.AppendChar((char)0x61);

        encodedText.AppendChar((char)0x79);
        encodedText.AppendChar((char)0x72);
        encodedText.AppendChar((char)0x75);
        encodedText.AppendChar((char)0x63);
        encodedText.AppendChar((char)0x72);
        encodedText.AppendChar((char)0x65);
        encodedText.AppendChar((char)0x4D);
        string tstr = pLoginName.Trim() + pPassword.Trim() +
                        ((char)0x49).ToString() +
                        ((char)0x6E).ToString() +
                        ((char)0x74).ToString() +
                        ((char)0x65).ToString() +
                        ((char)0x67).ToString() +
                        ((char)0x72).ToString() +
                        ((char)0x61).ToString() +
                        ((char)0x79).ToString() +
                        ((char)0x72).ToString() +
                        ((char)0x75).ToString() +
                        ((char)0x63).ToString() +
                        ((char)0x72).ToString() +
                        ((char)0x65).ToString() +
                        ((char)0x4D).ToString();

        //tstr = pLoginName.Trim() + pPassword.Trim() + encodedText;

        encodedText.MakeReadOnly();
        hashValue = shaobj.ComputeHash(Encoding.ASCII.GetBytes(
            pLoginName.Trim() + pPassword.Trim() + tstr));
        foreach (byte b in hashValue)
        {
            strHex += String.Format("{0:x2}", b);
        }

        return strHex;

    }
}