﻿// Copyright (c) 2008-2023, Hazelcast, Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// <auto-generated>
//   This code was generated by a tool.
//   Hazelcast Client Protocol Code Generator @8aed6958e
//   https://github.com/hazelcast/hazelcast-client-protocol
//   Change to this file will be lost if the code is regenerated.
// </auto-generated>

#pragma warning disable IDE0051 // Remove unused private members
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hazelcast.Protocol.BuiltInCodecs;
using Hazelcast.Protocol.CustomCodecs;
using Hazelcast.Core;
using Hazelcast.Messaging;
using Hazelcast.Clustering;
using Hazelcast.Serialization;
using Microsoft.Extensions.Logging;

namespace Hazelcast.Protocol.Codecs
{
    /// <summary>
    /// Returns the entries for the given keys. If any keys are not present in the Map, it will call loadAll The returned
    /// map is NOT backed by the original map, so changes to the original map are NOT reflected in the returned map, and vice-versa.
    /// Please note that all the keys in the request should belong to the partition id to which this request is being sent, all keys
    /// matching to a different partition id shall be ignored. The API implementation using this request may need to send multiple
    /// of these request messages for filling a request for a key set if the keys belong to different partitions.
    ///</summary>
#if SERVER_CODEC
    internal static class MapGetAllServerCodec
#else
    internal static class MapGetAllCodec
#endif
    {
        public const int RequestMessageType = 74496; // 0x012300
        public const int ResponseMessageType = 74497; // 0x012301
        private const int RequestInitialFrameSize = Messaging.FrameFields.Offset.PartitionId + BytesExtensions.SizeOfInt;
        private const int ResponseInitialFrameSize = Messaging.FrameFields.Offset.ResponseBackupAcks + BytesExtensions.SizeOfByte;

#if SERVER_CODEC
        public sealed class RequestParameters
        {

            /// <summary>
            /// name of map
            ///</summary>
            public string Name { get; set; }

            /// <summary>
            /// keys to get
            ///</summary>
            public IList<IData> Keys { get; set; }
        }
#endif

        public static ClientMessage EncodeRequest(string name, ICollection<IData> keys)
        {
            var clientMessage = new ClientMessage
            {
                IsRetryable = false,
                OperationName = "Map.GetAll"
            };
            var initialFrame = new Frame(new byte[RequestInitialFrameSize], (FrameFlags) ClientMessageFlags.Unfragmented);
            initialFrame.Bytes.WriteIntL(Messaging.FrameFields.Offset.MessageType, RequestMessageType);
            initialFrame.Bytes.WriteIntL(Messaging.FrameFields.Offset.PartitionId, -1);
            clientMessage.Append(initialFrame);
            StringCodec.Encode(clientMessage, name);
            ListMultiFrameCodec.Encode(clientMessage, keys, DataCodec.Encode);
            return clientMessage;
        }

#if SERVER_CODEC
        public static RequestParameters DecodeRequest(ClientMessage clientMessage)
        {
            using var iterator = clientMessage.GetEnumerator();
            var request = new RequestParameters();
            iterator.Take(); // empty initial frame
            request.Name = StringCodec.Decode(iterator);
            request.Keys = ListMultiFrameCodec.Decode(iterator, DataCodec.Decode);
            return request;
        }
#endif

        public sealed class ResponseParameters
        {

            /// <summary>
            /// values for the provided keys.
            ///</summary>
            public IList<KeyValuePair<IData, IData>> Response { get; set; }
        }

#if SERVER_CODEC
        public static ClientMessage EncodeResponse(ICollection<KeyValuePair<IData, IData>> response)
        {
            var clientMessage = new ClientMessage();
            var initialFrame = new Frame(new byte[ResponseInitialFrameSize], (FrameFlags) ClientMessageFlags.Unfragmented);
            initialFrame.Bytes.WriteIntL(Messaging.FrameFields.Offset.MessageType, ResponseMessageType);
            clientMessage.Append(initialFrame);
            EntryListCodec.Encode(clientMessage, response, DataCodec.Encode, DataCodec.Encode);
            return clientMessage;
        }
#endif

        public static ResponseParameters DecodeResponse(ClientMessage clientMessage)
        {
            using var iterator = clientMessage.GetEnumerator();
            var response = new ResponseParameters();
            iterator.Take(); // empty initial frame
            response.Response = EntryListCodec.Decode(iterator, DataCodec.Decode, DataCodec.Decode);
            return response;
        }

    }
}
