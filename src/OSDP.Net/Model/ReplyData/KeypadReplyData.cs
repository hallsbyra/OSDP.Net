﻿using System;
using System.Linq;
using System.Text;

namespace OSDP.Net.Model.ReplyData
{
    /// <summary>
    /// Keypad data entered on PD
    /// </summary>
    public class KeypadReplyData
    {
        private const int ReplyKeyPadDataLength = 2;

        /// <summary>
        /// Reader number 0=First Reader 1=Second Reader
        /// </summary>
        public byte ReaderNumber { get; private set; }

        /// <summary>
        /// Number of digits in the return data
        /// </summary>
        public ushort DigitCount { get; private set; }

        /// <summary>
        /// Data returned from keypad
        /// 
        /// The key encoding uses the following data representation:
        /// Digits 0 through 9 are reported as ASCII characters 0x30 through 0x39
        /// The clear/delete/'*' key is reported as ASCII DELETE, 0x7F
        /// The enter/'#' key is reported as ASCII return, 0x0D
        /// Special/function keys are reported as upper case ASCII:
        /// A or F1 = 0x41, B or F2 = 0x42, C or F3 = 0x43, D or F4 = 0x44
        /// F1 & F2 = 0x45, F2 & F3 = 0x46, F3 & F4 = 0x47, F1 & F4 = 0x48
        /// </summary>
        public byte[] Data { get; private set; }

        internal static KeypadReplyData ParseData(ReadOnlySpan<byte> data)
        {
            var dataArray = data.ToArray();
            if (dataArray.Length < ReplyKeyPadDataLength)
            {
                throw new Exception("Invalid size for the data");
            }

            var keypadReplyData = new KeypadReplyData
            {
                ReaderNumber = dataArray[0],
                DigitCount = dataArray[1],
                Data = dataArray.Skip(ReplyKeyPadDataLength).Take(dataArray.Length - ReplyKeyPadDataLength).ToArray()
            };

            return keypadReplyData;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var build = new StringBuilder();
            build.AppendLine($"Reader Number: {ReaderNumber}");
            build.AppendLine($"  Digit Count: {DigitCount}");
            build.AppendLine($"         Data: {Encoding.Default.GetString(Data)}");
            return build.ToString();
        }
    }
}
