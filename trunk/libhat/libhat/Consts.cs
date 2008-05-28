
using System;

namespace libhat {
    public class Consts {

        /// <summary>
        /// passphrase to encode/decode information sending between client and hat
        /// </summary>
        private static readonly byte[] passphrase = new byte[]{
            0x2E, 0xC7, 0xC4, 0x8D, 0xFF, 0xE5, 0x5D, 0x0B, 0xD6, 0xFF, 0x7F, 0xFF, 0xD7, 0x34, 0xD2, 0x02,
            0xE2, 0x6D, 0x9E, 0x48, 0x7B, 0xC6, 0x6A, 0xF1, 0x97, 0x73, 0x56, 0x77, 0xFA, 0x9D, 0x80, 0x00,
            0x11, 0x04, 0x29, 0x08, 0xA6, 0x8B, 0x2A, 0x47, 0x64, 0x00, 0x01, 0x38, 0x84, 0xA0, 0x40, 0x69,
            0x01, 0xF9, 0xFA, 0xBE, 0xEA, 0xFF, 0x97, 0x7B, 0xA7, 0x26, 0xED, 0xF7, 0x6B, 0x7B, 0x3B, 0x4F,
            0x44, 0x74, 0xA3, 0x09, 0x79, 0x40, 0x38, 0x3A, 0x20, 0x5D, 0xA3, 0x40, 0xC3, 0xE8, 0x7F, 0x3B
        };

        /// <summary>
        /// Example of successful response. Encoded.
        /// </summary>
        private static readonly byte[] successfulResponse =
            new byte[]
                {
                    0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xe0, 0xc3, 0xc4, 0x8d, 0xff, 0x0d, 0x5e, 0x0b, 0xd6,
                    0x9b, 0x7e, 0xff, 0xd7, 0x34
                };

        /// <summary>
        /// Available pictures for character
        /// </summary>
        private static byte[] pics = {
				0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 
				0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2F, 0x30, 0x31, 
				0x32, 0x33, 0x34, 0x05, 0x02, 0x03, 0x04, 0x01, 
				0x06, 0x07, 0x08, 0x09, 0x0A, 0x16, 0x17, 0x18, 
				0x19, 0x1F, 
				0x4F, 0x51, 0x52, 0x43, 0x42, 0x41, 0x44, 0x48, 
				0x49, 
				0x8B, 0x8C, 0x91, 0x81, 0x82, 0x83, 0x84, 0x85, 
				0x86, 0x87, 0x8A, 
				0xC6, 0xC8, 0xC9, 0xCF, 0xD0, 0xD2, 0xD4, 0xC1, 
				0xC2, 0xC3, 0xC5,
			};

        /// <summary>
        /// Tail of any packet between hat and client
        /// </summary>
        private static readonly byte[] packetEnding = new byte[] { 0x64, 0x1, 0x0,0x0,0x0 };

        /// <summary>
        /// Example of successful response. Encoded.
        /// </summary>
        public static byte[] SuccessfulResponse {
            get { return successfulResponse; }
        }

        /// <summary>
        /// passphrase to encode/decode information sending between client and hat
        /// </summary>
        public static byte[] Passphrase {
            get { return passphrase; }
        }

        /// <summary>
        /// Tail of any packet between hat and client
        /// </summary>
        public static byte[] PacketEnding {
            get { return packetEnding; }
        }

        /// <summary>
        /// maximal length of a single packet
        /// </summary>
        public static int PacketMaxLength {
            get { return 0x8E ; }
        }

        /// <summary>
        /// hat identifier
        /// </summary>
        public static Int32 HatIdentifier {
            get {
                return 0x123;
            }
        }

        /// <summary>
        /// Available pictures for character
        /// </summary>
        public static byte[] Pics {
            get { return pics; }
        }
    }
}
