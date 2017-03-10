// Copyright (c) 2008-2017, Hazelcast, Inc. All Rights Reserved.
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

using System.Collections.Generic;
using Hazelcast.Client.Protocol.Util;
using Hazelcast.IO;
using Hazelcast.IO.Serialization;

namespace Hazelcast.Client.Protocol.Codec
{
    internal sealed class QueueDrainToMaxSizeCodec
    {
        public const int ResponseType = 106;
        public const bool Retryable = false;

        public static readonly QueueMessageType RequestType = QueueMessageType.QueueDrainToMaxSize;

        public static ResponseParameters DecodeResponse(IClientMessage clientMessage)
        {
            var parameters = new ResponseParameters();
            IList<IData> list = null;
            var list_size = clientMessage.GetInt();
            list = new List<IData>();
            for (var list_index = 0; list_index < list_size; list_index++)
            {
                IData list_item;
                list_item = clientMessage.GetData();
                list.Add(list_item);
            }
            parameters.list = list;
            return parameters;
        }

        public static ClientMessage EncodeRequest(string name, int maxSize)
        {
            var requiredDataSize = RequestParameters.CalculateDataSize(name, maxSize);
            var clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
            clientMessage.SetMessageType((int) RequestType);
            clientMessage.SetRetryable(Retryable);
            clientMessage.Set(name);
            clientMessage.Set(maxSize);
            clientMessage.UpdateFrameLength();
            return clientMessage;
        }

        //************************ REQUEST *************************//

        public class RequestParameters
        {
            public static readonly QueueMessageType TYPE = RequestType;
            public int maxSize;
            public string name;

            public static int CalculateDataSize(string name, int maxSize)
            {
                var dataSize = ClientMessage.HeaderSize;
                dataSize += ParameterUtil.CalculateDataSize(name);
                dataSize += Bits.IntSizeInBytes;
                return dataSize;
            }
        }

        //************************ RESPONSE *************************//


        public class ResponseParameters
        {
            public IList<IData> list;
        }
    }
}