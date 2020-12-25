using System;
using ZstdNet;

namespace Lidgren.Network.Compression
{
    public class NetZstdCompression : NetCompression
    {
        private readonly Compressor _compressor = new Compressor();
        private readonly Decompressor _decompressor = new Decompressor();

        public override bool Compress(NetOutgoingMessage msg)
        {
            var span = msg.Data.AsSpan().Slice(msg.PositionInBytes, msg.LengthBytes);

            var data = _compressor.Wrap(span);

            msg.Reset();
            msg.Write(data);
            return true;
        }

        public override bool Decompress(NetIncomingMessage msg)
        {
            var span = msg.Data.AsSpan().Slice(msg.PositionInBytes, msg.LengthBytes);

            var data = _decompressor.Unwrap(span);
            msg.Reset();
            msg.Write(data);
            return true;
        }
    }
}
