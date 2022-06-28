// ***********************************************************************
// Assembly         : Adrack.Web.Framework
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 05-11-2020
// ***********************************************************************
// <copyright file="AppDependency.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Fakes;
using Adrack.Core.Infrastructure;
using Adrack.Core.Infrastructure.ApplicationDependency;
using Adrack.Core.Infrastructure.Configuration;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Accounting;
using Adrack.Service.Agent;
using Adrack.Service.Audit;
using Adrack.Service.Click;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Infrastructure;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Service.Seo;
using Adrack.Web.Framework.Mvc.Route;
using Adrack.Web.Framework.UI;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adrack.Web.Framework
{
    /// <summary>
    /// Represents a Application Dependency
    /// Implements the <see cref="Adrack.Core.Infrastructure.ApplicationDependency.IAppDependency" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.ApplicationDependency.IAppDependency" />
    public class AppDependency : IAppDependency
    {
        #region Methods

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="containerBuilder">Container Builder</param>
        /// <param name="typeFinder">Type Finder</param>
        /// <param name="appConfiguration">Application Configuration</param>
        public virtual void Register(ContainerBuilder containerBuilder, ITypeFinder typeFinder, AppConfiguration appConfiguration)
        {
            #region HTTP Context

            containerBuilder.Register(x => HttpContext.Current != null ? (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) : (new FakeHttpContext("~/") as HttpContextBase)).As<HttpContextBase>().InstancePerLifetimeScope();
            containerBuilder.Register(x => x.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
            containerBuilder.Register(x => x.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerLifetimeScope();
            containerBuilder.Register(x => x.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerLifetimeScope();
            containerBuilder.Register(x => x.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerLifetimeScope();

            #endregion HTTP Context

            #region Helper

            containerBuilder.RegisterType<CommonHelper>().As<ICommonHelper>().InstancePerLifetimeScope();

            #endregion Helper

            #region Controllers

            containerBuilder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            #endregion Controllers

            #region Data

            var dataSettingManager = new DataSettingManager();
            var dataProviderSetting = dataSettingManager.LoadSetting();

            containerBuilder.Register(c => dataSettingManager.LoadSetting()).As<DataSetting>();
            containerBuilder.Register(x => new EfDataProviderManager(x.Resolve<DataSetting>())).As<BaseDataProviderManager>().InstancePerLifetimeScope();

            containerBuilder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerLifetimeScope();

            var efDataProviderManager = new EfDataProviderManager(dataSettingManager.LoadSetting());
            var dataProvider = efDataProviderManager.LoadDataProvider();

            dataProvider.InitializeConnectionFactory();

            containerBuilder.Register<IDbClientContext>(c => new AppObjectClientContext(dataProviderSetting.DataConnectionString.Replace("=dev-lead-distribution", "=a"))).InstancePerRequest();


            /*if (dataProviderSetting != null && dataProviderSetting.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingManager.LoadSetting());
                var dataProvider = efDataProviderManager.LoadDataProvider();

                dataProvider.InitializeConnectionFactory();

                containerBuilder.Register<IDbContext>(c => new AppObjectContext(dataProviderSetting.DataConnectionString)).InstancePerLifetimeScope();
                containerBuilder.Register<IDbClientContext>(c => new AppObjectClientContext(dataProviderSetting.DataConnectionString)).InstancePerLifetimeScope();

            }
            else
            {
                containerBuilder.Register<IDbContext>(c => new AppObjectContext(dataSettingManager.LoadSetting().DataConnectionString)).InstancePerLifetimeScope();
                containerBuilder.Register<IDbClientContext>(c => new AppObjectClientContext(dataSettingManager.LoadSetting().DataConnectionString)).InstancePerLifetimeScope();
            }*/

            containerBuilder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerRequest();

            containerBuilder.RegisterType<DbContextService>().As<IDbContextService>().SingleInstance();

            #endregion Data

            #region Cache Manager

            if (appConfiguration.RedisCachingEnabled)
            {
                containerBuilder.RegisterType<RedisConnectionWrapper>().As<IRedisConnectionWrapper>().SingleInstance();
                containerBuilder.RegisterType<RedisCacheManager>().As<ICacheManager>().Named<ICacheManager>("Application.Cache.Manager_Static").InstancePerLifetimeScope();
            }
            else
            {
                containerBuilder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("Application.Cache.Manager_Static").SingleInstance();
            }

            containerBuilder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("Application.Cache.Manager_Per.Request").InstancePerLifetimeScope();

            #endregion Cache Manager

            #region Application Context

            containerBuilder.RegisterType<WebAppContext>().As<IAppContext>().InstancePerLifetimeScope();

            #endregion Application Context

            #region Web Farm

            if (appConfiguration.WebFarmRunOnAzureWebsites)
            {
                containerBuilder.RegisterType<AzureMachineName>().As<IMachineName>().SingleInstance();
            }
            else
            {
                containerBuilder.RegisterType<LocalMachineName>().As<IMachineName>().SingleInstance();
            }

            #endregion Web Farm

            #region Framework

            #region MVC Route

            containerBuilder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();

            #endregion MVC Route

            #region UI

            containerBuilder.RegisterType<PageLayoutBuilder>().As<IPageLayoutBuilder>().InstancePerLifetimeScope();

            #endregion UI

            #endregion Framework

            #region Domain

            #region Agent

            containerBuilder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();

            #endregion Agent

            #region Application Events

            containerBuilder.RegisterType<AppEventPublisher>().As<IAppEventPublisher>().SingleInstance();
            containerBuilder.RegisterType<AppSubscriptionService>().As<IAppSubscriptionService>().SingleInstance();

            var subscriber = typeFinder.FindClassesOfType(typeof(IAppSubscriber<>)).ToList();

            foreach (var value in subscriber)
            {
                containerBuilder.RegisterType(value).As(value.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());

                    return isMatch;
                }, typeof(IAppSubscriber<>))).InstancePerLifetimeScope();
            }

            #endregion Application Events

            #region Audit

            containerBuilder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();

            #endregion Audit

            #region Common

            containerBuilder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<NavigationService>().As<INavigationService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<GlobalAttributeService>().As<IGlobalAttributeService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();

            #endregion Common

            #region Configuration

            containerBuilder.RegisterSource(new SettingSource());
            containerBuilder.RegisterType<SettingService>().As<ISettingService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();

            #endregion Configuration

            #region Content

            containerBuilder.RegisterType<AddressService>().As<IAddressService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<AccountingService>().As<IAccountingService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<SupportTicketsService>().As<ISupportTicketsService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<SupportTicketsMessagesService>().As<ISupportTicketsMessagesService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<DocumentService>().As<IDocumentService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<EntityChangeHistoryService>().As<IEntityChangeHistoryService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PaymentService>().As<IPaymentService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<StorageService>().As<IStorageService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            #endregion Content

            #region Directory

            containerBuilder.RegisterType<CountryService>().As<ICountryService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<CurrencyService>().As<ICurrencyService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<StateProvinceService>().As<IStateProvinceService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<UserTypeService>().As<IUserTypeService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();

            #endregion Directory

            #region Localization

            containerBuilder.RegisterType<LanguageService>().As<ILanguageService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<LocalizedPropertyService>().As<ILocalizedPropertyService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<LocalizedStringService>().As<ILocalizedStringService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();

            #endregion Localization

            #region Membership

            containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<UserRegistrationService>().As<IUserRegistrationService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ProfileService>().As<IProfileService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<JWTTokenService>().As<IJWTTokenService>().InstancePerLifetimeScope();

            #endregion Membership

            #region Lead

            containerBuilder.RegisterType<AffiliateService>().As<IAffiliateService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AffiliateResponseService>().As<IAffiliateResponseService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PostedDataService>().As<IPostedDataService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AffiliateNoteService>().As<IAffiliateNoteService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<BuyerService>().As<IBuyerService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<BuyerResponseService>().As<IBuyerResponseService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CampaignService>().As<ICampaignService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CampaignTemplateService>().As<ICampaignTemplateService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DepartmentService>().As<IDepartmentService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<PaymentMethodService>().As<IPaymentMethodService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<AffiliateChannelService>().As<IAffiliateChannelService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AffiliateChannelTemplateService>().As<IAffiliateChannelTemplateService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<BuyerChannelService>().As<IBuyerChannelService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<BuyerChannelTemplateService>().As<IBuyerChannelTemplateService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<FormTemplateService>().As<IFormTemplateService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<VerticalService>().As<IVerticalService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<BlackListService>().As<IBlackListService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<LeadMainService>().As<ILeadMainService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<NoteTitleService>().As<INoteTitleService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<LeadMainResponseService>().As<ILeadMainResponseService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LeadContentDublicateService>().As<ILeadContentDublicateService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LeadContentService>().As<ILeadContentService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LeadFieldsContentService>().As<ILeadFieldsContentService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<BuyerChannelScheduleService>().As<IBuyerChannelScheduleService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<HistoryService>().As<IHistoryService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ReportService>().As<IReportService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<FilterService>().As<IFilterService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<BuyerChannelFilterConditionService>().As<IBuyerChannelFilterConditionService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AffiliateChannelFilterConditionService>().As<IAffiliateChannelFilterConditionService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<RedirectUrlService>().As<IRedirectUrlService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ZipCodeRedirectService>().As<IZipCodeRedirectService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ProcessingLogService>().As<IProcessingLogService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<BuyerChannelTemplateMatchingService>().As<IBuyerChannelTemplateMatchingService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<RegistrationRequestService>().As<IRegistrationRequestService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<UserSubscribtionService>().As<IUserSubscribtionService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<LeadSensitiveDataService>().As<ILeadSensitiveDataService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<DoNotPresentService>().As<IDoNotPresentService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SubIdWhiteListService>().As<ISubIdWhiteListService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CachedUrlService>().As<ICachedUrlService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<GeoZipService>().As<IGeoZipService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<Service.Notification.NotificationService>().As<Service.Notification.INotificationService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ClickService>().As<IClickService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CouponService>().As<ICouponService>().InstancePerLifetimeScope();
            #endregion Lead

            #region Message

            containerBuilder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<EmailTokenDictionary>().As<IEmailTokenDictionary>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<EmailTokenizer>().As<IEmailTokenizer>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<EmailQueueService>().As<IEmailQueueService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<EmailSubscriptionService>().As<IEmailSubscriptionService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<EmailTemplateService>().As<IEmailTemplateService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<SmtpAccountService>().As<ISmtpAccountService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<CouponService>().As<ICouponService>().InstancePerLifetimeScope();
            #endregion Message

            #region Security

            containerBuilder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AclService>().As<IAclService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<RoleService>().As<IRoleService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();
            containerBuilder.RegisterType<PermissionService>().As<IPermissionService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();

            #endregion Security

            #region SEO

            containerBuilder.RegisterType<PageSlugService>().As<IPageSlugService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("Application.Cache.Manager_Static")).InstancePerLifetimeScope();

            #endregion SEO

            #endregion Domain
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Application Dependency Order
        /// </summary>
        /// <value>The order.</value>
        public int Order
        {
            get { return 0; }
        }

        #endregion Properties
    }

    /// <summary>
    /// Represents a Setting Source
    /// Implements the <see cref="Autofac.Core.IRegistrationSource" />
    /// </summary>
    /// <seealso cref="Autofac.Core.IRegistrationSource" />
    public class SettingSource : IRegistrationSource
    {
        #region Fields

        /// <summary>
        /// Method Info
        /// </summary>
        private static readonly MethodInfo BuildMethod = typeof(SettingSource).GetMethod("BuildRegistration", BindingFlags.Static | BindingFlags.NonPublic);

        #endregion Fields

        #region Utilities

        /// <summary>
        /// Build Registration
        /// </summary>
        /// <typeparam name="TSetting">Type Setting</typeparam>
        /// <returns>Component Registration Collection Item</returns>
        private static IComponentRegistration BuildRegistration<TSetting>() where TSetting : ISetting, new()
        {
            return RegistrationBuilder.ForDelegate((x, p) => { return x.Resolve<ISettingService>().LoadSetting<TSetting>(); }).InstancePerLifetimeScope().CreateRegistration();
        }

        #endregion Utilities



        #region Methods

        /// <summary>
        /// Registrations For
        /// </summary>
        /// <param name="service">Service</param>
        /// <param name="registrations">Registratoins</param>
        /// <returns>IEnumerable&lt;IComponentRegistration&gt;.</returns>
        public IEnumerable<IComponentRegistration> RegistrationsFor(Autofac.Core.Service service, Func<Autofac.Core.Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var typedService = service as TypedService;

            if (typedService != null && typeof(ISetting).IsAssignableFrom(typedService.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(typedService.ServiceType);

                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Is Adapter For Individual Components
        /// </summary>
        /// <value><c>true</c> if this instance is adapter for individual components; otherwise, <c>false</c>.</value>
        public bool IsAdapterForIndividualComponents { get { return false; } }

        #endregion Properties
    }
}