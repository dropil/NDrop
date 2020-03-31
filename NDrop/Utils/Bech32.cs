using NBitcoin.DataEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDrop
{
    public class Bech32
    {
        protected readonly byte[] Byteset = Encoders.ASCII.DecodeData("qpzry9x8gf2tvdw0s3jn54khce6mua7l");
        private static readonly uint[] Generator = { 0x3b6a57b2U, 0x26508e6dU, 0x1ea119faU, 0x3d4233ddU, 0x2a1462b3U };

        internal Bech32(string hrp) : this(hrp == null ? null : Encoders.ASCII.DecodeData(hrp))
        {
        }

        public Bech32(byte[] hrp)
        {
            if (hrp == null)
                throw new ArgumentNullException(nameof(hrp));

            _Hrp = hrp;
            var len = hrp.Length;
            _HrpExpand = new byte[(2 * len) + 1];
            for (int i = 0; i < len; i++)
            {
                _HrpExpand[i] = (byte)(hrp[i] >> 5);
                _HrpExpand[i + len + 1] = (byte)(hrp[i] & 31);
            }
        }

        protected readonly byte[] _HrpExpand;
        protected readonly byte[] _Hrp;

        private static uint Polymod(byte[] values)
        {
            uint chk = 1;
            foreach (var value in values)
            {
                var top = chk >> 25;
                chk = value ^ ((chk & 0x1ffffff) << 5);
                foreach (var i in Enumerable.Range(0, 5))
                {
                    chk ^= ((top >> i) & 1) == 1 ? Generator[i] : 0;
                }
            }
            return chk;
        }

        private byte[] CreateChecksum(byte[] data, int offset, int count)
        {
            var values = new byte[_HrpExpand.Length + count + 6];
            var valuesOffset = 0;
            Array.Copy(_HrpExpand, 0, values, valuesOffset, _HrpExpand.Length);
            valuesOffset += _HrpExpand.Length;
            Array.Copy(data, offset, values, valuesOffset, count);
            valuesOffset += count;
            var polymod = Polymod(values) ^ 1;
            var ret = new byte[6];
            foreach (var i in Enumerable.Range(0, 6))
            {
                ret[i] = (byte)((polymod >> 5 * (5 - i)) & 31);
            }
            return ret;
        }

        public string EncodeData(byte[] data, int offset, int count)
        {
            var combined = new byte[_Hrp.Length + 1 + count + 6];
            int combinedOffset = 0;
            Array.Copy(_Hrp, 0, combined, 0, _Hrp.Length);
            combinedOffset += _Hrp.Length;
            combined[combinedOffset] = 49;
            combinedOffset++;
            Array.Copy(data, offset, combined, combinedOffset, count);
            combinedOffset += count;
            var checkSum = CreateChecksum(data, offset, count);
            Array.Copy(checkSum, 0, combined, combinedOffset, 6);
            combinedOffset += 6;
            for (int i = 0; i < count + 6; i++)
            {
                combined[_Hrp.Length + 1 + i] = Byteset[combined[_Hrp.Length + 1 + i]];
            }
            return Encoders.ASCII.EncodeData(combined);
        }

        protected virtual byte[] ConvertBits(IEnumerable<byte> data, int fromBits, int toBits, bool pad = true)
        {
            var acc = 0;
            var bits = 0;
            var maxv = (1 << toBits) - 1;
            var ret = new List<byte>();
            foreach (var value in data)
            {
                if ((value >> fromBits) > 0)
                    throw new FormatException("Invalid Bech32 string");
                acc = (acc << fromBits) | value;
                bits += fromBits;
                while (bits >= toBits)
                {
                    bits -= toBits;
                    ret.Add((byte)((acc >> bits) & maxv));
                }
            }
            if (pad)
            {
                if (bits > 0)
                {
                    ret.Add((byte)((acc << (toBits - bits)) & maxv));
                }
            }
            else if (bits >= fromBits || (byte)(((acc << (toBits - bits)) & maxv)) != 0)
            {
                throw new FormatException("Invalid Bech32 string");
            }
            return ret.ToArray();
        }

        public string Encode(byte[] pubKey)
        {
            var data = ConvertBits(pubKey, 8, 5);
            var ret = EncodeData(data, 0, data.Length);
            return ret;
        }
    }
}
