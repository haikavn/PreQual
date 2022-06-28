using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core
{
    public enum Conditions : short
    {
        Contains = 1,
        NotContains = 2,
        StartsWith = 3,
        EndsWith = 4,
        Equals = 5,
        NotEquals = 6,
        NumberGreater = 7,
        NumberGreaterOrEqual = 8,
        NumberLess = 9,
        NumberLessOrEqual = 10,
        NumberRange = 11,
        StringByLength = 12
    }

    /// <summary>
    ///     Enum DataFormats
    /// </summary>
    /// <summary>
    ///     Enum Validators
    /// </summary>
    public enum Validators : short
    {
        /// <summary>
        ///     The none
        /// </summary>
        None = 0,

        /// <summary>
        ///     The string
        /// </summary>
        String = 1,

        /// <summary>
        ///     The number
        /// </summary>
        Number = 2,

        /// <summary>
        ///     The email
        /// </summary>
        Email = 3,

        /// <summary>
        ///     The account number
        /// </summary>
        AccountNumber = 5,

        /// <summary>
        ///     The SSN
        /// </summary>
        Ssn = 6,

        /// <summary>
        ///     The zip
        /// </summary>
        Zip = 7,

        /// <summary>
        ///     The phone
        /// </summary>
        Phone = 8,

        /// <summary>
        ///     The date time
        /// </summary>
        DateTime = 10,

        /// <summary>
        ///     The state
        /// </summary>
        State = 11,

        /// <summary>
        ///     The routing number
        /// </summary>
        RoutingNumber = 12,

        /// <summary>
        ///     The date of birth
        /// </summary>
        DateOfBirth = 14,

        /// <summary>
        ///     The decimal
        /// </summary>
        Decimal = 16,

        /// <summary>
        ///     The sub identifier
        /// </summary>
        SubId = 17
    }

    public enum ActivityStatuses : short
    {
        Inactive = 0,
        Active = 1
    }

    public enum PostFormat
    {
        XML = 0,
        Json = 1,
        QueryStringPOST = 2,
        QueryStringGET = 3,
        Email = 4,
        SOAP = 5
    }

    public enum ResponseFormat
    {
        Auto = 0,
        XML = 1,
        Json = 2,
        String = 3,
        Detect = 4
    }

    public enum AffilatePriceMethods : short
    {
        Fixed = 0,
        Revshare = 1
    }

    public enum AffiliateChannelPriceMethods : short
    {
        Fixed = 0,
        Revshare = 1,
        TakeFromAffilate = 2
    }

    public enum BuyerChannelPriceMethods
    {
        Fixed = 0,
        Revshare = 1,
        TakeFromAffilateChannel = 2
    }

    public enum BuyerPriceOptions : short
    {
        Fixed = 0,
        Dynamic = 1,
    }

    public enum AffiliateInvitationStatuses : short
    {
        None = 0,
        Pending = 1,
        Accepted = 2
    }

    public enum BuyerInvitationStatuses : short
    {
        None = 0,
        Pending = 1,
        Accepted = 2
    }

    public enum BuyerInvitationRoles : short
    {
        Manager = 0,
        Employee = 1
    }


    public enum AffiliatePaymentMethods : short
    {
        Transfer = 0,
        Paypel = 1
    }
    public enum AffiliateInvitationRole
    {
        Manager = 0,
    }

    public enum EntityStatus : short
    {
        Active = 1,
        Inactive = 0
    }

    public enum EntityFilterByStatus : short
    {
        All = -1,
        Inactive = 0,
        Active = 1,
        Pending = 2,
        Blocked = 3,
        Rejected = 4,
        Deleted = 5
    }

    public enum DeletedStatus : short
    {
        Deleted = 1,
        NotDeleted = 0
    }

    public enum EntityFilterByDeleted : short
    {
        Deleted = 1,
        NotDeleted = 0,
        All = -1
    }

    public enum CampaignTypes : short
    {
        LeadCampaign = 0,
        ClickCampaign = 1
    }

    public enum BuyerType
    {
        Storefront = 0,
        Online = 1
    }

    public enum BuyerChannelType
    {
        Storefront = 0,
        Online = 1
    }

    public enum BillFrequency:short
    {
        [Description("Monthly")]
        m = 0,
        [Description("Weekly")]
        b = 1,
        [Description("Bi-Weekly")]
        bw = 2
    }

    public enum PriceType : byte
    {
        Fixed = 0,
        Revenue = 1,
        InheritFromAffiliate = 2
    }

    public enum ChannelType : byte
    {
        AffiliateChannel = 1,
        BuyerChannel = 2
    }

    public enum UserTypes : short
    {
        Super = 1,
        Network = 2,
        Affiliate = 3,
        Buyer = 4
    }

    public enum DataFormat : byte
    {
        XML = 0,
        JSON = 1,
        Other
    }

    public enum LeadResponseStatus : short
    {
        Test = 0,
        Sold = 1,
        Error = 2,
        Reject = 3,
        Processing = 4,
        FilterError = 5,
        MinPriceError = 6,
        ScheduleError = 7
    }

    public enum LeadActionType : short
    {
        Received = 0,
        Posted = 1,
        Responsed = 2
    }

    public enum ErrorType : short
    {
        Unknown = 0,
        NoData = 1,
        InvalidData = 2,
        UnknownDbField = 3,
        MissingValue = 4,
        MissingField = 5,
        NotExistingDbRecord = 6,
        Dropped = 7,
        DailyCapReached = 8,
        IntegrationError = 9,
        FilterError = 10,
        NotEnoughBalance = 11,
        ScheduleCapLimit = 12,
        MinPriceError = 13,
        Timeout = 14
    }

    public enum ReportType : short
    {
        Buyer=1,
        Affiliate = 2
    }

    public enum DataFormatTypes
    {
        None = 0,
        EditBox = 1,
        DropDown = 2
    }

    public enum ReportEntityType : byte
    { 
        Campaign,
        Affiliate,
        Buyer,
        BuyerChannel,
        AffiliateChannel,
        None
    }
    public enum CustomReportType : short
    {

    }

    public enum ReportPeriodType : byte
    {
        Custom=1,
        Recurring=2
        //ThisWeek,
        //LastWeek,
        //PreviosMonth,
        //PreviousYear,
        //TwoYearsLater,
        //ThreeYearsLater,
        //TwoYearsLaterTillNow,
    }

    public enum NotificationType : short
    {
        Greeting=1,
        NewRegistration,
        PasswordChange,
        Message
    }

    public enum FormTemplateType:short
    {
        Wizard=1,
        SingleForm
    }

    public enum IntegrationType : short
    {
        EmbeddedForm=1,
        StadAlonePage,
        PopUpForm
    }

    public enum FormTemplateElementType : short
    {
        TextField = 1,
        RadioButtons,
    }

    public enum TextStyle : short
    {
        Bold = 1,
        Italic,
        UnderLine,
    }
    public enum EntityTypes:int
    {
        EntityChangeHistory = 1,
        AffiliateResponse=2,
        LeadContent =3,
        LeadContentDublicate =4,
        LeadMainReportDay =5,
        LeadMainReportDayAffiliate =6,
        LeadMainReportDayReceived =7,
        LeadMainReportDayRedirected =8,
        LeadMainReportDayPrices =9,
        LeadMainReportDayHour =10,
        LeadMainResponse =11,
        BuyerResponse =12,
        PostedData =13,
        LeadMainReport =14,
        LeadFieldsContent =15,
        LeadMainReportDaySubIds =16,
        LeadSensitiveData =17,
        LeadMain =18,
        Log =19,
        AttachedChannel =20,
        AffiliateChannelNote =21,
        AffiliateChannelFilterCondition =22,
        FormTemplateItem =23,
        FormTemplate =24,
        AffiliateChannelTemplate =25,
        AffiliateChannel =26,
        AffiliateNote =27,
        AffiliateInvitation =28,
        Affiliate =29,
        BuyerChannelNote =30,
        BuyerChannelSchedule =31,
        BuyerChannelTemplate =32,
        BuyerChannelFilterCondition =33,
        BuyerChannelTemplateMatching =34,
        BuyerChannel =35,
        BuyerInvitation =36,
        Buyer =37,
        History =38,
        EmailQueue =39,
        LeadNote =40,
        PaymentMethod = 42,
        SupportTicketsUser = 43,
        SupportTicketsMessages = 44,
        VerifyAccount = 46,
        Profile = 48,
        UserReport = 49,
        AffiliateInvoice = 51,
        AffiliateInvoiceAdjustment = 52,
        AffiliatePayment = 54,
        BlackListValue = 55,
        BuyerBalance = 56,
        BuyerInvoiceAdjustment = 57,
        BuyerInvoice = 58,
        BuyerPayment = 59,
        RedirectUrl = 60,
        RefundedLeads = 61,
        CampaignTemplate = 62,
        Filter = 63,
        Campaign = 64
    }


    public enum EmailTemplateType
    {
        ResponseNull = -1,
        //AccountConfirming = 1,
        //AccountIsActive = 2,
        //ThreeDaysBeforeEndTrial = 4,
        //OneDayBeforeEndTrial = 5,
        //DayTrialExpiration = 6,
        //OneDayAfterExpiration = 7,
        //ThreeDaysAfterExpiration = 8,
        //SevenDaysAfterExpiration = 9,
        //LimitWebsiteCount = 11,
        //LimitMessageCount = 12,
        //PeriodExpiration = 13,
        //LimitSubscriberCount = 14,
        //WeeklyReport = 15,
        //ResetPassword = 16,
        //ContactUsMessageReceivedWebPush = 17,
        //CodeMissingFromWebsite = 18,
        //ConfirmationOrder = 19,
        //SendingUserPassword = 20,
        //ContactUsMessageReceivedAdRackLeads = 21,
        //ContactUsMessageReceivedAdRackCalls = 22,
        //ContactUsMessageReceivedAdRackClicks = 23,
        //ChangePassword = 28,
        //PaymentFailure = 29,
        //RegisterConfirmation = 30,
        //NewCustomerAdminEmail = 31,
        //DeletedCustomerAdminEmail = 32,
        //Promotion30PercentDiscount = 33,
        //PromotionEndingSoon = 34
    }

    public enum EmailOperatorEnums:short
    {
        LeadNative = 1,
        SendGrid
    };

    public enum DataGenerationTypes : short
    {
        Addons=1,
        Permissions,
        Plans,
        UserPlan,
        PermissionAddon,
        UserAddon

    }
}
