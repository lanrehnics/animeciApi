using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace AnimeciBackend
{
    public static class RSAKeyUtils
    {
        //public static RSAParameters GetRandomKey()
        //{
        //    using (var rsa = new RSACryptoServiceProvider(2048))
        //    {
        //        try
        //        {
        //            return rsa.ExportParameters(true);
        //        }
        //        finally
        //        {
        //            rsa.PersistKeyInCsp = false;
        //        }
        //    }
        //}

        //public static void GenerateKeyAndSave(string file)
        //{
        //    var p = GetRandomKey();
        //    RSAParametersWithPrivate t = new RSAParametersWithPrivate();
        //    t.SetParameters(p);
        //    File.WriteAllText(file, JsonConvert.SerializeObject(t));
        //}

        /// <summary>
        /// This expects a file in the format: 
        /// {
        ///  "Modulus": "z7eXmrs9z3Xm7VXwYIdziDYzXGfi3XQiozIRa58m3ApeLVDcsDeq6Iv8C5zJ2DHydDyc0x6o5dtTRIb23r5/ZRj4I/UwbgrwMk5iHA0bVsXVPBDSWsrVcPDGafr6YbUNQnNWIF8xOqgpeTwxrqGiCJMUjuKyUx01PBzpBxjpnQ++Ryz6Y7MLqKHxBkDiOw5wk9cxO8/IMspSNJJosOtRXFTR74+bj+pvNBa8IJ+5Jf/UfJEEjk+qC+pohCAryRk0ziXcPdxXEv5KGT4zf3LdtHy1YwsaGLnTb62vgbdqqCJaVyHWOoXsDTQBLjxNl9o9CzP6CrfBGK6JV8pA/xfQlw==",
        ///  "Exponent": "AQAB",
        ///  "P": "+VsETS2exORYlg2CxaRMzyG60dTfHSuv0CsfmO3PFv8mcYxglGa6bUV5VGtB6Pd1HdtV/iau1WR/hYXQphCP99Pu803NZvFvVi34alTFbh0LMfZ+2iQ9toGzVfO8Qdbj7go4TWoHNzCpG4UCx/9wicVIWJsNzkppSEcXYigADMM=",
        ///  "Q": "1UCJ2WAHasiCdwJtV2Ep0VCK3Z4rVFLWg3q1v5OoOU1CkX5/QAcrr6bX6zOdHR1bDCPsH1n1E9cCMvwakgi9M4Ch0dYF5CxDKtlx+IGsZJL0gB6HhcEsHat+yXUtOAlS4YB82G1hZqiDw+Q0O8LGyu/gLDPB+bn0HmbkUC2kP50=",
        ///  "DP": "CBqvLxr2eAu73VSfFXFblbfQ7JTwk3AiDK/6HOxNuL+eLj6TvP8BvB9v7BB4WewBAHFqgBIdyI21n09UErGjHDjlIT88F8ZtCe4AjuQmboe/H2aVhN18q/vXKkn7qmAjlE78uXdiuKZ6OIzAJGPm8nNZAJg5gKTmexTka6pFJiU=",
        ///  "DQ": "ND6zhwX3yzmEfROjJh0v2ZAZ9WGiy+3fkCaoEF9kf2VmQa70DgOzuDzv+TeT7mYawEasuqGXYVzztPn+qHhrogqJmpcMqnINopnTSka6rYkzTZAtM5+35yz0yvZiNbBTFdwcuglSK4xte7iU828stNs/2JR1mXDtVeVvWhVUgCE=",
        ///  "InverseQ": "Heo0BHv685rvWreFcI5MXSy3AN0Zs0YbwAYtZZd1K/OzFdYVdOnqw+Dg3wGU9yFD7h4icJFwZUBGOZ0ww/gZX/5ZgJK35/YY/DeV+qfZmywKauUzC6+DPsrDdW1uf1eAety6/huRZTduBFTwIOlPdZ+PY49j6S38DjPFNImn0cU=",
        ///  "D": "IvjMI5cGzxkQqkDf2cC0aOiHOTWccqCM/GD/odkH1+A+/u4wWdLliYWYB/R731R5d6yE0t7EnP6SRGVcxx/XnxPXI2ayorRgwHeF+ScTxUZFonlKkVK5IOzI2ysQYMb01o1IoOamCTQq12iVDMvV1g+9VFlCoM+4GMjdSv6cxn6ELabuD4nWt8tCskPjECThO+WdrknbUTppb2rRgMvNKfsPuF0H7+g+WisbzVS+UVRvJe3U5O5X5j7Z82Uq6hw2NCwv2YhQZRo/XisFZI7yZe0OU2JkXyNG3NCk8CgsM9yqX8Sk5esXMZdJzjwXtEpbR7FiKZXiz9LhPSmzxz/VsQ=="
        /// }
        /// 
        /// Generate 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static RSAParameters GetKeyParameters(string file)
        {
            if (!File.Exists(file)) throw new FileNotFoundException("Check configuration - cannot find auth key file: " + file);
            var keyParams = JsonConvert.DeserializeObject<RSAParametersWithPrivate>(File.ReadAllText(file));
            return keyParams.ToRSAParameters();
        }

        static byte[] GetStrBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static RSAParameters AnimeciKey()
        {
            var keyParams = new RSAParametersWithPrivate();
            keyParams.D = GetStrBytes("GUCTc82wxFito0VvPQmXnpxZvj0PBz0jF/1mH63BpIdc/0tFuCfqbPYcy6fxJ7YsNnWF+CUZ86CrcOrD6MGbreCwUe+VruAqJd0Vn9emQfD7obXuIIasbJGdSoovCicYkRJGsOBC40VDHdFncSoOokLND2Pc3QsFPVc1mT/M+c9e5AKa4T4M2rtAIFfWe9UbOg83Kogi0XDTisUxM+W7IVX1m6KnvJE4sKOLpawYqp2AdfGh19TGqXrQRoGV204VbBXzNBtHT3GANDqcPRr4d44v01jWMXTwvhI734V268Omdkco4yyMjt7oR2PjBx6Vq8fbbIPBDuTWy3NzCtRAAQ==");
            keyParams.DP = GetStrBytes("rg6EXA9T+muJjAsbIFamDpjS3D0DXWBhU03SJUiCtd93XimWdcqlCi7WpXCGr9uJgOMRng6fjWw8amoulP2vzUQM4Rf2e+9bMX+tB/JvsA6Zyi5TVa2o2wvPmMbBg/zKYYQVmTGve7on4jNOAD0PywAZvQR97BvPf6J1VkopthE=");
            keyParams.DQ = GetStrBytes("EiZX52P3qSzUSLdvx5R6ocZ9sqKEMHfXlhiWr0s6eZfc4JEhMhxpJVoH46zbtZc6Gf1lhOhX9y5i6B6l6UBvzj581prFxEBnxO2Lc9JPzm0v8p5++QrkuExyIOuhMbL7GMnfYEEMfkcOKMaVdSfVFXJGYZiH3r8uFkUagswdIz0=");
            keyParams.Exponent = GetStrBytes("AQAB");
            keyParams.InverseQ = GetStrBytes("J58MzZ56XwXENQNTqbE+ckNFJO6rR0bHHIbr1BHn4thG4DYK9Y/2w2FHJenI8pAQfGqHszDexpS8rfguryNr8hUJnWDNQL0w9MXfzLzm73lB3NHZ4xr7FpGdtGrhziQv5oKeQ8s99e4RUOYeOeKTdKgd1fYxojeskVq/A05NrAI=");
            keyParams.Modulus = GetStrBytes("qh9+Ao5voe7J/6jofHFivf/MfTtL5w6Wc2GicwyqliG5cDe1/CX2cbLId7QgaXS8HzQd1VgOH9vD1Mt+1FsQaFNSLKEK6CdlNhXGp+2iW48YEiV4K36oFdfWorkYHTbO86dctut3rAECm3zFHsa7RYLt89BSjNbMAm7QbuEBhB7su+xAZKcM31UUKQLbbelbGck89joStZNAmhdVwUQsgvaA7qPGCxwoJGI5U0Anx4Qf/nIUYcttYyh8JigtbgtArpBsZ9QS8pRy7Ch7BQVE6t6w5NDjid/YahS2p731S8FxMk+qoMJmaTc2rm2Z2iIndkk1s1v9VKE7dkK1U9f7uw==");
            keyParams.P = GetStrBytes("uCrlH/XABMJjRFwqJ8JtPjHCoLxV3NOG0PUqNSeEO1gAwC0vhAL4k4SfRjSdaBF2vdXpHZTij2SR7QX1vPyvNLvM4vXPZxEBDJjedVYi6x/6Jpho/0wfGbChcPaS2AN3Dh/tvKs6k3Nwoe5zokpfAPAR6MGjuhd3UwvSDz/C5bE=");
            keyParams.Q = GetStrBytes("7HpAa0eCc0AZhqcpyQwFnIHbNadiLMnz4klT+FyDwdLuaY5KEF44EuQSNI+x81LxTW+jSRPkskA7xquEOBqE3qDHxCDLh2Ny/OlSAohg8OJL7MjPOC5NUZewECwUoQoeHi3QRGyLnt68tW0hRZZ0603cKeG9zNp7cUAQsDqglys=");

            return keyParams.ToRSAParameters();
        }

        /// <summary>
        /// Util class to allow restoring RSA parameters from JSON as the normal
        /// RSA parameters class won't restore private key info.
        /// </summary>
        private class RSAParametersWithPrivate
        {
            public byte[] D { get; set; }
            public byte[] DP { get; set; }
            public byte[] DQ { get; set; }
            public byte[] Exponent { get; set; }
            public byte[] InverseQ { get; set; }
            public byte[] Modulus { get; set; }
            public byte[] P { get; set; }
            public byte[] Q { get; set; }

            public void SetParameters(RSAParameters p)
            {
                D = p.D;
                DP = p.DP;
                DQ = p.DQ;
                Exponent = p.Exponent;
                InverseQ = p.InverseQ;
                Modulus = p.Modulus;
                P = p.P;
                Q = p.Q;
            }
            public RSAParameters ToRSAParameters()
            {
                return new RSAParameters()
                {
                    D = this.D,
                    DP = this.DP,
                    DQ = this.DQ,
                    Exponent = this.Exponent,
                    InverseQ = this.InverseQ,
                    Modulus = this.Modulus,
                    P = this.P,
                    Q = this.Q

                };
            }
        }
    }
}