// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RequestResult.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Xml;

namespace Adrack.Managers
{
    /// <summary>
    ///     Class RequestResult.
    /// </summary>
    public class RequestResult
    {
        #region Enums

        /// <summary>
        ///     Enum StatusTypes
        /// </summary>
        public enum StatusTypes
        {
            /// <summary>
            ///     The inactive
            /// </summary>
            Inactive = 0,

            /// <summary>
            ///     The active
            /// </summary>
            Active = 1,

            /// <summary>
            ///     The test
            /// </summary>
            Test = 2
        }

        /// <summary>
        ///     Enum ResultTypes
        /// </summary>
        public enum ResultTypes
        {
            /// <summary>
            ///     The test
            /// </summary>
            Test = 0,

            /// <summary>
            ///     The success
            /// </summary>
            Success = 1,

            /// <summary>
            ///     The error
            /// </summary>
            Error = 2,

            /// <summary>
            ///     The reject
            /// </summary>
            Reject = 3,

            /// <summary>
            ///     The processing
            /// </summary>
            Processing = 4,

            /// <summary>
            ///     The filter error
            /// </summary>
            FilterError = 5,

            /// <summary>
            ///     The minimum price error
            /// </summary>
            MinPriceError = 6,

            /// <summary>
            ///     The schedule error
            /// </summary>
            ScheduleError = 7
        }

        /// <summary>
        ///     Enum ErrorTypes
        /// </summary>
        public enum ErrorTypes
        {
            /// <summary>
            ///     The none
            /// </summary>
            None = 0,

            /// <summary>
            ///     The unknown
            /// </summary>
            Unknown = 1,

            /// <summary>
            ///     The no data
            /// </summary>
            NoData = 2,

            /// <summary>
            ///     The invalid data
            /// </summary>
            InvalidData = 3,

            /// <summary>
            ///     The unknown database field
            /// </summary>
            UnknownDBField = 4,

            /// <summary>
            ///     The missing value
            /// </summary>
            MissingValue = 5,

            /// <summary>
            ///     The missing field
            /// </summary>
            MissingField = 6,

            /// <summary>
            ///     The not existing database record
            /// </summary>
            NotExistingDBRecord = 7,

            /// <summary>
            ///     The dropped
            /// </summary>
            Dropped = 8,

            /// <summary>
            ///     The daily cap reached
            /// </summary>
            DailyCapReached = 9,

            /// <summary>
            ///     The integration error
            /// </summary>
            IntegrationError = 10,

            /// <summary>
            ///     The filter error
            /// </summary>
            FilterError = 11,

            /// <summary>
            ///     The not enough balance
            /// </summary>
            NotEnoughBalance = 12,

            /// <summary>
            ///     The schedule cap limit
            /// </summary>
            ScheduleCapLimit = 13,

            /// <summary>
            ///     The minimum price error
            /// </summary>
            MinPriceError = 14,

            /// <summary>
            ///     The system on hold
            /// </summary>
            SystemOnHold = 15,

            /// <summary>
            ///     Buyer channel response error
            /// </summary>
            BuyerChannelResponseError = 16,

            RequestTimeoutError = 17,

            RequestTimeoutPausedError = 18,

            AfterTimeout = 19
        }

        #endregion Enums

        #region Private properties

        /// <summary>
        ///     The result type
        /// </summary>
        private ResultTypes resultType = ResultTypes.Test;

        /// <summary>
        ///     The error type
        /// </summary>
        private ErrorTypes errorType = ErrorTypes.None;

        /// <summary>
        ///     The message
        /// </summary>
        private string message = "";

        /// <summary>
        ///     The price
        /// </summary>
        private string price = "";

        /// <summary>
        ///     The redirect
        /// </summary>
        private string redirect = "";

        /// <summary>
        ///     The redirect no c data
        /// </summary>
        private string redirectNoCData = "";

        /// <summary>
        ///     The internal URL
        /// </summary>
        private string internalUrl = "";

        /// <summary>
        ///     The title
        /// </summary>
        private string title = "";

        /// <summary>
        ///     The description
        /// </summary>
        private string description = "";

        /// <summary>
        ///     The address
        /// </summary>
        private string address = "";

        /// <summary>
        ///     The zip code
        /// </summary>
        private string zipCode = "";

        /// <summary>
        ///     The additional XML
        /// </summary>
        private XmlElement additionalXml;

        #endregion Private properties

        #region Public properties

        /// <summary>
        ///     Gets or sets the type of the result.
        /// </summary>
        /// <value>The type of the result.</value>
        public ResultTypes ResultType
        {
            get => resultType;
            set => resultType = value;
        }

        /// <summary>
        ///     Gets or sets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public ErrorTypes ErrorType
        {
            get => errorType;
            set => errorType = value;
        }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get => message;
            set => message = value;
        }

        /// <summary>
        ///     Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public string Price
        {
            get => price;
            set => price = value;
        }

        /// <summary>
        ///     Gets or sets the internal URL.
        /// </summary>
        /// <value>The internal URL.</value>
        public string InternalUrl
        {
            get => internalUrl;
            set => internalUrl = value;
        }

        /// <summary>
        ///     Gets or sets the redirect.
        /// </summary>
        /// <value>The redirect.</value>
        public string Redirect
        {
            get => redirect;
            set => redirect = value;
        }

        /// <summary>
        ///     Gets or sets the redirect no c data.
        /// </summary>
        /// <value>The redirect no c data.</value>
        public string RedirectNoCData
        {
            get => redirectNoCData;
            set => redirectNoCData = value;
        }

        /// <summary>
        ///     Gets or sets the additional XML.
        /// </summary>
        /// <value>The additional XML.</value>
        public XmlElement AdditionalXml
        {
            get => additionalXml;
            set => additionalXml = value;
        }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => title;
            set => title = value;
        }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get => description;
            set => description = value;
        }

        /// <summary>
        ///     Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address
        {
            get => address;
            set => address = value;
        }

        /// <summary>
        ///     Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode
        {
            get => zipCode;
            set => zipCode = value;
        }

        /// <summary>
        ///     Gets or sets the validator.
        /// </summary>
        /// <value>The validator.</value>
        public short Validator { get; set; }

        #endregion Public properties

        #region Public methods

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestResult" /> class.
        /// </summary>
        public RequestResult()
        {
            Validator = 0;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestResult" /> class.
        /// </summary>
        /// <param name="rt">The rt.</param>
        /// <param name="et">The et.</param>
        /// <param name="msg">The MSG.</param>
        public RequestResult(ResultTypes rt, ErrorTypes et, string msg)
        {
            Set(rt, et, msg);
        }

        /// <summary>
        ///     The lock objet
        /// </summary>
        private readonly object lockObjet = new object();

        /// <summary>
        ///     Sets the specified rt.
        /// </summary>
        /// <param name="rt">The rt.</param>
        /// <param name="et">The et.</param>
        /// <param name="msg">The MSG.</param>
        public void Set(ResultTypes rt, ErrorTypes et, string msg)
        {
            lock (lockObjet)
            {
                resultType = rt;
                errorType = et;
                message = msg;
            }
        }

        #endregion Public methods
    }
}