// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="GlobalAttributeBuiltIn.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Common
{
    /// <summary>
    /// Represents a Global Attribute Built In
    /// </summary>
    public static partial class GlobalAttributeBuiltIn
    {
        #region Properties

        #region Site

        /// <summary>
        /// Last Visited Page
        /// </summary>
        /// <value>The last visited page.</value>
        public static string LastVisitedPage
        {
            get
            {
                return "LastVisitedPage";
            }
        }

        /// <summary>
        /// Signature
        /// </summary>
        /// <value>The signature.</value>
        public static string Signature
        {
            get
            {
                return "Signature";
            }
        }

        /// <summary>
        /// Forgot Password Token
        /// </summary>
        /// <value>The forgot password token.</value>
        public static string ForgotPasswordToken
        {
            get
            {
                return "ForgotPasswordToken";
            }
        }

        /// <summary>
        /// Forgot Password Token Requested Date
        /// </summary>
        /// <value>The forgot password token requested date.</value>
        public static string ForgotPasswordTokenRequestedDate
        {
            get
            {
                return "ForgotPasswordTokenRequested";
            }
        }



        /// <summary>
        /// First Password Token
        /// </summary>
        /// <value>The First password token.</value>
        public static string FirstPasswordToken
        {
            get
            {
                return "FirstPasswordToken";
            }
        }

        /// <summary>
        /// First Password Token Requested Date
        /// </summary>
        /// <value>The First password token requested date.</value>
        public static string FirstPasswordTokenRequestedDate
        {
            get
            {
                return "FirstPasswordTokenRequested";
            }
        }


        /// <summary>
        /// Membership Activation Token
        /// </summary>
        /// <value>The membership activation token.</value>
        public static string MembershipActivationToken
        {
            get
            {
                return "MembershipActivationToken";
            }
        }


        /// <summary>
        /// Invited User Token
        /// </summary>
        /// <value>The Invited User Token.</value>
        public static string InvitedUserToken
        {
            get
            {
                return "InvitedUserToken";
            }
        }


        /// <summary>
        /// Impersonated User Identifier
        /// </summary>
        /// <value>The impersonated user identifier.</value>
        public static string ImpersonatedUserId
        {
            get
            {
                return "ImpersonatedUserId";
            }
        }

        /// <summary>
        /// Language Automatically Detected
        /// </summary>
        /// <value>The language automatically detected.</value>
        public static string LanguageAutomaticallyDetected
        {
            get
            {
                return "LanguageAutomaticallyDetected";
            }
        }

        /// <summary>
        /// Language Identifier
        /// </summary>
        /// <value>The language identifier.</value>
        public static string LanguageId
        {
            get
            {
                return "LanguageId";
            }
        }

        /// <summary>
        /// Currency Identifier
        /// </summary>
        /// <value>The currency identifier.</value>
        public static string CurrencyId
        {
            get
            {
                return "CurrencyId";
            }
        }

        /// <summary>
        /// Time Zone Identifier
        /// </summary>
        /// <value>The time zone identifier.</value>
        public static string TimeZoneId
        {
            get
            {
                return "TimeZoneId";
            }
        }

        #endregion Site

        #endregion Properties
    }
}