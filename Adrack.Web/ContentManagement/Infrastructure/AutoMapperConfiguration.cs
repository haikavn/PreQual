// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AutoMapperConfiguration.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Audit;
using Adrack.Core.Domain.Message;
using Adrack.Web.ContentManagement.Models.Audit;
using Adrack.Web.ContentManagement.Models.Message;
using AutoMapper;

namespace Adrack.Web.ContentManagement.Infrastructure
{
    /// <summary>
    /// Represents a Auto Mapper Configuration
    /// </summary>
    public static class AutoMapperConfiguration
    {
        #region Fields

        /// <summary>
        /// Mapper Configuration
        /// </summary>
        private static MapperConfiguration _mapperConfiguration;

        /// <summary>
        /// Mapper
        /// </summary>
        private static IMapper _mapper;

        #endregion Fields



        #region Methods

        /// <summary>
        /// Init
        /// </summary>
        public static void Init()
        {
            _mapperConfiguration = new MapperConfiguration(mapperConfiguration =>
            {
                #region Audit

                mapperConfiguration.CreateMap<Log, LogModel>()
                    .ForMember(destination => destination.CreatedOn, x => x.Ignore())
                    .ForMember(destination => destination.CustomProperties, x => x.Ignore());

                mapperConfiguration.CreateMap<LogModel, Log>()
                    .ForMember(destination => destination.CreatedOn, x => x.Ignore());

                #endregion Audit

                #region Message

                mapperConfiguration.CreateMap<SmtpAccount, SmtpAccountModel>()
                    .ForMember(destination => destination.SendEmailTo, x => x.Ignore())
                    .ForMember(destination => destination.CustomProperties, x => x.Ignore());

                mapperConfiguration.CreateMap<SmtpAccountModel, SmtpAccount>();

                #endregion Message
            });

            // Create Mapper
            _mapper = _mapperConfiguration.CreateMapper();
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Mapper
        /// </summary>
        /// <value>The mapper.</value>
        public static IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        /// <summary>
        /// Mapper Configuration
        /// </summary>
        /// <value>The mapper configuration.</value>
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }

        #endregion Properties
    }
}