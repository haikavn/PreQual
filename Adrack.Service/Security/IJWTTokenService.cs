// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAclService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Security
{
    public enum JWTVerficationResults : short
    {
        None = 0,
        Valid = 1,
        Invalid = 2,
        Expired = 3
    }

    /// <summary>
    /// Represents a Access Control List Service
    /// </summary>
    public partial interface IJWTTokenService
    {
        #region Methods

        string GenerateAccessToken(long userId, string userName);

        string GenerateRefreshToken(long userId, string userName);


        string GenerateAccessToken(Dictionary<string, object> claims);

        long VerifyUser(string token);


        long VerifyUser(string token, out JWTVerficationResults jWTVerficationResult);

        bool Verify(string token, long userId = 0); 

        JWTVerficationResults GetLastVerivicationResult();

        /// <summary>
        /// Get All Refresh Tokens
        /// </summary>
        /// <returns>All tokens</returns>
        Dictionary<long, string> GetAllRefreshTokens();
       
        /// <summary>
        /// Insert Refresh Token in Memory Cache
        /// </summary>
        /// <param name="refreshTokens"></param>
        void InsertRefreshToken(Dictionary<long, string> refreshTokens);

        #endregion Methods
    }
}