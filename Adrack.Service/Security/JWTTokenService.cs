// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AclService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.Core.Helpers;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.Data;
using Adrack.Service.Audit;
using Adrack.Service.Helpers;
using Adrack.Service.Membership;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Adrack.Service.Security
{
    /// <summary>
    /// Represents a Access Control List Service
    /// Implements the <see cref="Adrack.Service.Security.IAclService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Security.IAclService" />
    public partial class JWTTokenService : IJWTTokenService
    {
        #region Enums

        #endregion

        #region Constants

        const string secret = "f7bz6awucKOeUPNviUAzz9d2bZiFWa826JDjlVsXSbjx4bRP90";
        private const int expirationTime = 43200;
        private const string CACHE_RefreshToken_ALL_KEY = "App.Cache.RefreshToken.All";

        #endregion Constants

        #region Fields

        protected JWTVerficationResults _lastVerificationResult = JWTVerficationResults.None;

        private readonly IUserService _userService;

        private readonly ICacheManager _cacheManager;

        #endregion Fields

        #region Constructor

        public JWTTokenService(IUserService userService,
                               ICacheManager cacheManager)
        {
            _userService = userService;
            _cacheManager = cacheManager;
        }

        #endregion Constructor

        #region Methods

        public string GenerateAccessToken(long userId, string userName)
        {
            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                { "UserId", userId },
                { "UserName", userName },
                { "exp", DateTimeOffset.UtcNow.AddMinutes(expirationTime).ToUnixTimeSeconds()}
            };

            string token = GenerateAccessToken(claims);

            //InsertTokenExpiration(token);

            return token;
        }

        public string GenerateAccessToken(Dictionary<string, object> claims)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(claims, secret);
        }

        public long VerifyUser(string token)
        {
            JWTVerficationResults jWTVerficationResult = JWTVerficationResults.None;
            return VerifyUser(token, out jWTVerficationResult);
        }


        public long VerifyUser(string token, out JWTVerficationResults jWTVerficationResult)
        {
            var logService = AppEngineContext.Current.Resolve<ILogService>();

            jWTVerficationResult = JWTVerficationResults.None;

            try
            {
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJsonSerializer serializer = new JsonNetSerializer();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                var claims = new JwtBuilder()
                    .WithSecret(secret)
                    .WithUrlEncoder(urlEncoder)
                    .WithSerializer(serializer)
                    .WithAlgorithm(algorithm)
                    .WithEncoder(encoder)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);

                /*if (claims.ContainsKey("ExpirationTime") &&
                   (DateTime)claims["ExpirationTime"] < DateTime.UtcNow)
               {
                   jWTVerficationResult = JWTVerficationResults.Expired;
                   logService.InsertLog(Core.Domain.Audit.LogLevel.Debug, 
                       "JWTValidationFail-" + jWTVerficationResult.ToString() + 
                       "-" + 
                       claims["ExpirationTime"].ToString() +
                       "-" + DateTime.UtcNow.ToString());

                   return 0;
               }*/

                if (claims.ContainsKey("UserId") && claims["UserId"] is long)
                {
                    jWTVerficationResult = JWTVerficationResults.Valid;
                    return (long)claims["UserId"];
                }
            }
            catch (TokenExpiredException ex)
            {
                jWTVerficationResult = JWTVerficationResults.Expired;
                string domain = WebHelper.GetSubdomain() + "-" + WebHelper.GetCurrentUserId();
                logService.InsertLog(Core.Domain.Audit.LogLevel.Debug,
                    "JWTTokenExpiredException-" + jWTVerficationResult.ToString() +
                    "-" + ex.Message +
                    "-" + DateTime.UtcNow.ToString() + "-" + domain + "-" + token);
            }
            catch (SignatureVerificationException)
            {
                jWTVerficationResult = JWTVerficationResults.Invalid;
                logService.InsertLog(Core.Domain.Audit.LogLevel.Debug,
                    "JWTSignatureVerificationException-" + jWTVerficationResult.ToString());
            }
            catch (Exception)
            {
                jWTVerficationResult = JWTVerficationResults.Invalid;
                logService.InsertLog(Core.Domain.Audit.LogLevel.Debug,
                    "JWTException-" + jWTVerficationResult.ToString());
            }

            return 0;
        }

        public bool Verify(string token, long userId = 0)
        {
            JWTVerficationResults jWTVerficationResult = JWTVerficationResults.None;

            if (userId == 0)
            {
                userId = VerifyUser(token, out jWTVerficationResult);
            }

            /*DateTime? expirationDate = GetTokenExpiration(token);
            if (!expirationDate.HasValue || (expirationDate.HasValue && DateTime.UtcNow > expirationDate.Value))
            {
                jWTVerficationResult = JWTVerficationResults.Expired;
                RemoveTokenExpiration(token);
                string domain = WebHelper.GetSubdomain() + "-" + userId.ToString();
                logService.InsertLog(Core.Domain.Audit.LogLevel.Debug,
                    "JWTExpired-" + jWTVerficationResult.ToString() +
                    "-" + (expirationDate.HasValue ? "-" + expirationDate.Value.ToString() : "") +
                    "-" + DateTime.UtcNow.ToString() + "-" + domain);

                return false;
            }

            RemoveTokenExpiration(token);
            InsertTokenExpiration(token);*/

            return true;
        }

        public JWTVerficationResults GetLastVerivicationResult()
        {
            return _lastVerificationResult;
        }
        /// <summary>
        /// Generate refresh token
        /// </summary>
        /// <returns>refreshToken</returns>
        public string GenerateRefreshToken(long userId, string userName)
        {
            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                { "UserId", userId },
                { "UserName", userName },
                { "exp", DateTimeOffset.UtcNow.AddMinutes(expirationTime + 1).ToUnixTimeSeconds()}
            };

            string token = GenerateAccessToken(claims);

            //InsertTokenExpiration(token);

            return token;
        }

        /// <summary>
        /// Get All Refresh Tokens
        /// </summary>
        /// <returns>tokens</returns>
        public virtual Dictionary<long, string> GetAllRefreshTokens()
        {
            const string key = CACHE_RefreshToken_ALL_KEY;
            return _cacheManager.Get<Dictionary<long, string>>(key);
        }

        /// <summary>
        /// Insert RefreshTokens In Memory Cache
        /// </summary>
        /// <param name="refreshTokens"></param>
        public virtual void InsertRefreshToken(Dictionary<long, string> refreshTokens)
        {
            _cacheManager.RemoveByPattern(CACHE_RefreshToken_ALL_KEY);
            _cacheManager.Set(CACHE_RefreshToken_ALL_KEY,refreshTokens, 108);
        }

        #endregion Methods
    }
}