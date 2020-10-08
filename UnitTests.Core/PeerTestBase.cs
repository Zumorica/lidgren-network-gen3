using System;
using System.Reflection;
using System.Text;
using Lidgren.Network;
using NUnit.Framework;

namespace UnitTests
{
    public abstract class PeerTestBase
    {
        protected NetPeerConfiguration Config;
        protected NetPeer Peer;

        [SetUp]
        public void Setup()
        {
            Config = new NetPeerConfiguration(GetType().Name)
            {
                EnableUPnP = true
            };
            Peer = new NetPeer(Config);
            Peer.Start();

            TestContext.Out.WriteLine("Unique identifier is " + NetUtility.ToHexString(Peer.UniqueIdentifier));
        }

        [TearDown]
        public void TearDown()
        {
            Peer.Shutdown("Unit test teardown.");
        }

        protected static NetIncomingMessage CreateIncomingMessage(byte[] fromData, int bitLength)
        {
            var inc = (NetIncomingMessage) Activator.CreateInstance(typeof(NetIncomingMessage), true);
            typeof(NetIncomingMessage).GetField("m_data", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(inc, fromData);
            typeof(NetIncomingMessage).GetField("m_bitLength", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(inc, bitLength);
            return inc;
        }

        protected static string ToBinaryString(ulong value, int bits, bool includeSpaces)
        {
            var numSpaces = Math.Max(0, bits / 8 - 1);
            if (includeSpaces == false)
            {
                numSpaces = 0;
            }

            var bdr = new StringBuilder(bits + numSpaces);
            for (var i = 0; i < bits + numSpaces; i++)
            {
                bdr.Append(' ');
            }

            for (var i = 0; i < bits; i++)
            {
                var shifted = value >> i;
                var isSet = (shifted & 1) != 0;

                var pos = bits - 1 - i;
                if (includeSpaces)
                {
                    pos += Math.Max(0, pos / 8);
                }

                bdr[pos] = isSet ? '1' : '0';
            }

            return bdr.ToString();
        }
    }
}