using System;
using System.Collections.Generic;
using System.Globalization;

namespace NModbus4.Message
{
    public class SlaveExceptionResponse : AbstractModbusMessage, IModbusMessage
    {
        private static readonly Dictionary<byte, string> _exceptionMessages = CreateExceptionMessages();
        private static readonly Dictionary<byte, string> _exceptionMessagesKeys = CreateExceptionMessagesKeys();

        public SlaveExceptionResponse()
        {
        }

        public SlaveExceptionResponse(byte slaveAddress, byte functionCode, byte exceptionCode)
            : base(slaveAddress, functionCode)
        {
            SlaveExceptionCode = exceptionCode;
        }

        public override int MinimumFrameSize
        {
            get { return 3; }
        }

        public byte SlaveExceptionCode
        {
            get { return MessageImpl.ExceptionCode.Value; }
            set { MessageImpl.ExceptionCode = value; }
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            string msg = _exceptionMessages.ContainsKey(SlaveExceptionCode)
                ? _exceptionMessages[SlaveExceptionCode]
                : Resources.Unknown;

            return string.Format(
                CultureInfo.InvariantCulture,
                Resources.SlaveExceptionResponseFormat,
                Environment.NewLine,
                FunctionCode,
                SlaveExceptionCode,
                msg);
        }


        public string MessageKey
        {
            get
            {
                string msg = _exceptionMessagesKeys.ContainsKey(SlaveExceptionCode)
                    ? _exceptionMessagesKeys[SlaveExceptionCode]
                    : Resources.Unknown;
                return msg;
            }
        }

        internal static Dictionary<byte, string> CreateExceptionMessages()
        {
            Dictionary<byte, string> messages = new Dictionary<byte, string>(9);

            messages.Add(1, Resources.IllegalFunction);
            messages.Add(2, Resources.IllegalDataAddress);
            messages.Add(3, Resources.IllegalDataValue);
            messages.Add(4, Resources.SlaveDeviceFailure);
            messages.Add(5, Resources.Acknowlege);
            messages.Add(6, Resources.SlaveDeviceBusy);
            messages.Add(8, Resources.MemoryParityError);
            messages.Add(10, Resources.GatewayPathUnavailable);
            messages.Add(11, Resources.GatewayTargetDeviceFailedToRespond);

            return messages;
        }
        internal static Dictionary<byte, string> CreateExceptionMessagesKeys()
        {
            Dictionary<byte, string> messagesKeys = new Dictionary<byte, string>(9);

            messagesKeys.Add(1, Resources.Keys.IllegalFunction);
            messagesKeys.Add(2, Resources.Keys.IllegalDataAddress);
            messagesKeys.Add(3, Resources.Keys.IllegalDataValue);
            messagesKeys.Add(4, Resources.Keys.SlaveDeviceFailure);
            messagesKeys.Add(5, Resources.Keys.Acknowlege);
            messagesKeys.Add(6, Resources.Keys.SlaveDeviceBusy);
            messagesKeys.Add(8, Resources.Keys.MemoryParityError);
            messagesKeys.Add(10, Resources.Keys.GatewayPathUnavailable);
            messagesKeys.Add(11, Resources.Keys.GatewayTargetDeviceFailedToRespond);

            return messagesKeys;
        }
        protected override void InitializeUnique(byte[] frame)
        {
            if (FunctionCode <= Modbus.ExceptionOffset)
            {
                throw new FormatException(Resources.SlaveExceptionResponseInvalidFunctionCode);
            }

            SlaveExceptionCode = frame[2];
        }
    }
}
