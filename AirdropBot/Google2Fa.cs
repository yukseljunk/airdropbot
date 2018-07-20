using System;
using System.Security.Cryptography;

namespace AirdropBot
{
    public class Google2Fa
    {

        /// <summary>
        /// The algorithm to use for the TOTP.
        /// </summary>

        public
            Algorithm Algorithm { get; private set; }

        /// <summary>
        /// Gets the period that a TOTP code will be valid for, in seconds.
        /// </summary>
        /// <remarks>The period may be ignored by some 2FA client applications.</remarks>
        /// <see cref="DEFAULTPERIOD"/>
        public int Period { get; private set; }

        /// <summary>
        /// Gets the number of digits to display to the user.
        /// </summary>
        /// <see cref="DEFAULTDIGITS"/>
        public int Digits { get; private set; }

        /// <summary>
        /// Defines the default number of digits used when this number is unspecified.
        /// </summary>
        public const int DEFAULTDIGITS = 6;

        /// <summary>
        /// Defines the default period used when the period is unspecified.
        /// </summary>
        public const int DEFAULTPERIOD = 30;
        private readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public Google2Fa()
        {
            Period = DEFAULTPERIOD;
            Digits = DEFAULTDIGITS;
            Algorithm = Algorithm.SHA1;
        }
        private long DateTimeToTimestamp(DateTime value)
        {
            return (long)(value.ToUniversalTime() - EPOCH).TotalSeconds;
        }

        public string GetCode(string secret)
        {
            return GetCode(secret, DateTimeToTimestamp(DateTime.Now));
        }

        public string GetCode(string secret, long timestamp)
        {
            using (var algo = (KeyedHashAlgorithm)CryptoConfig.CreateFromName("HMAC" + Enum.GetName(typeof(Algorithm), Algorithm)))
            {
                algo.Key = Base32.Decode(secret);
                var ts = BitConverter.GetBytes(GetTimeSlice(timestamp, 0));
                var hashhmac = algo.ComputeHash(new byte[] { 0, 0, 0, 0, ts[3], ts[2], ts[1], ts[0] });
                var offset = hashhmac[hashhmac.Length - 1] & 0x0F;
                return (((
                             hashhmac[offset + 0] << 24 |
                             hashhmac[offset + 1] << 16 |
                             hashhmac[offset + 2] << 8 |
                             hashhmac[offset + 3]
                         ) & 0x7FFFFFFF) % (long)Math.Pow(10, Digits)).ToString().PadLeft(Digits, '0');
            }
        }
        private long GetTimeSlice(long timestamp, int offset)
        {
            return (timestamp / Period) + (offset * Period);
        }
    }
}