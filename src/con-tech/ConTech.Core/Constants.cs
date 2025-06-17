using Markdig;
using MimeKit;

namespace ConTech.Core;

public class SortDirection
{
    public const string? Ascending = null;
    public const string Descending = "Descending";
}

public class Constants
{
    public static readonly List<MailboxAddress> EmailsToBeBCCed =
    [
        new MailboxAddress("Fares", "fares@silverkeytech.com"),
        new MailboxAddress("Dody", "dodyg@silverkeytech.com"),
        new MailboxAddress("Waleed", "waleed@silverkeytech.com"),
        new MailboxAddress("Amr Ali", "amra@silverkeytech.com")
    ];

    public static class Orleans
    {
        public const string Storage = "cmis-storage";
    }

    public const int PublicPageSize = 10;

    public const int CommonPageSize = 50;

    public const int AllOptionValue = -1;

    public const int NoneOptionValue = -2;

    public const int DefaultReportTabId = 100;

    public const int ActiveOptionValue = -3;

    public const short ShortNoneOptionValue = -1;

    public const int ShortAllOptionValue = -2;

    public const int NumberOfNotifications = 25;

    public const string DefaultSortColumn = "Id";

    public const int MaxWrongLogins = 5;

    public const int MaxPathCount = 7;

    public const int SystemId = 1;

    public const string OtherOption = "other-option";

    public const int DefaultProjectTabId = 100;

    public static List<string> CmsSupportedLanguages => ["en", "ar"];

    public const string ImageCacheFolder = "image-cache";

    public const int MaxRetryCount = 3;

    public const string MaxFileSize = "UploadedFilesMaxSize";

    public const int MaxPageSize = int.MaxValue;

    public const string ArabicRegexPattern = @"^[\u0621-\u064A\u0660-\u0669\s\0-9a-zA-Z"".,\-_/\\'?؟~!@#\$%\^&\*\(\)_\+\[\];:{}،]+$";

    public const string EnglishRegexPattern = @"^[a-zA-Z\s\0-9\-\/\\:'?]+$";

    public const string ArabicRegexPattern2 = @"^[\u0621-\u064A\u0660-\u0669\s\0-9"".,\-_/\\'?؟~!@#\$%\^&\*\(\)_\+\[\];:{}،]+$";

    public const string QuestionBankFormId = "c149ab1f-7d8a-428a-ba4b-dbe082d99836";

    public static class Email
    {
        public const string TextPartType = "plain";
        public const string TextHtmlType = "html";
        public const string DefaultSenderEmail = "email:defaultSenderEmail";
        public const string DefaultSenderName = "email:defaultSenderName";
        public const string HostUrl = "HostUrl";
    }
}

public interface IByUser
{
    int UserId { get; }

    int PersonId { get; }
}

public static class ObjectStatus
{
    public const int Active = 1;

    public const int Disabled = 0;
}

public enum UserTypes
{
    Admin = 1,
    GizAdvisor = 2,
    Ngo = 3
}

public static class UserTypesEx
{
    public static int ToInt(this UserTypes self) => (int)self;

    public static UserTypes FromInt(int id) => id switch
    {
        1 => UserTypes.Admin,
        2 => UserTypes.GizAdvisor,
        3 => UserTypes.Ngo,
        _ => throw new ArgumentException()
    };
}

public enum ApplicationStatuses
{
    Draft = 1,
    Submitted = 2,
    Reviewed = 3,
    Accepted = 4,
    Shortlisted = 5,
    Waitlisted = 6,
    Archived = 7,
    AutoRejected = 8,
    Rejected = 9
}

public enum QuestionTypes
{
    Text = 1,
    DropdownList = 2,
    MultipleChoices = 3,
    Boolean = 4,
    File = 5,
    Numeric = 6,
}

public enum FormQuestionTypes
{
    RadioButton = 100,
    NumericInput = 200,
    MoneyBudget = 300,
    DropDown = 400,
    CheckBox = 500,
    TextInput = 600,
    TextArea = 700,
    UploadInputFile = 800,
    DatePickerInput = 900,
    GovernoratesDropDownList = 1000,
    LandlinePhone = 1100,
    E_mail = 1200,
    RepeatedForm = 1300,
    MobileNumber = 1400,
    Attacment = 1500,
}

public enum StageFormType
{
    FormBuilder = 100,
    UploadTemplate = 200,
}

public enum CycleStartingStatus
{
    NotStartedYet = 100,
    InProgress = 200,
    Completed = 300,
}

public enum TagTypes
{
    Score = 1,
    Flow = 2
}

public enum Sorting
{
    Ascendingly = 1,
    Descendingly = 2
}

public static class LibraryItemCategory
{
    public const int Library = 100;
}

public enum NotificationType
{
    Email = 100,
    Web = 200,
    EmailAndWeb = 300
}

public enum NotificationStatus
{
    Created = 100,
    Send = 200,
    Received = 300,
    Opened = 400,
    FailedToSend = 500,
    Canceled = 600
}

public enum RoleType
{
    Admin = 1,
    GizAdvisor = 2,
    Ngo = 3
}

public enum TicketStatusType
{
    New = 1,
    Resolved = 2,
    NotResolved = 3,
    InProgress = 4,
    Waiting = 5
}

public enum DocumentType
{
    File = 100,
    Presentation = 200,
    Video = 300,
    Audio = 400
}

public enum AnnouncementVisiblity
{
    Global = 100,
    CycleSpecific = 200,
    NgoSpecific = 300,
}

public enum CycleStatus
{
    Announced = 1,
    Filtration = 2,
    ConceptNotes = 3,
    CapacityDevelopmentAndProposals = 4,
    NegotiationAndAcceptance = 5,
    Contracts = 6,
    Implementation = 7,
    Completed = 8,
}

public enum CycleStageStatus
{
    Pending = 1,
}

public enum CycleOpenType
{
    OpenCycle = 100,
    CloseCycle = 200
}

public enum NgoProfileCategory
{
    Lsa = 1,
    Umbrella = 2,
    DirectProject = 3,
    SubGrantee = 4,
}

public enum ProjectReportStatus
{
    Pending = 1,
    InProgress = 2,
    Submitted = 3,
    SubmittedLate = 4,
    PendingUpdates = 5,
    Approved = 6,
    Rejected = 7,
    Draft = 8
}

public enum LogFrameElementType
{
    Objective = 1,
    Outcome = 2,
    Output = 3,
    Indicator = 4,
    Activity = 5,
    KeyActivity = 6
}

public enum ThematicAreasCategories
{
    AccessibilityAndInclusion = 1,
    Outreach = 2,
    ParticipatoryApproach = 3,
    NetworkingAndPartnerships = 4,
    ChallengesLessonsLearntAndSuccessStories = 5,
    Challenges = 6,
    LessonsLearnt = 7,
    SuccessStories = 8
}

public enum ProjectBenefeciaryTypes
{
    MaleAbove29 = 1,
    MaleWithDisabilityAbove29 = 2,
    FemaleAbove29 = 3,
    FemaleWithDisabilityAbove29 = 4,
    MaleBetween16and29 = 5,
    MaleWithDisabilityBetween16and29 = 6,
    FemaleBetween16and29 = 7,
    FemaleWithDisabilityBetween16and29 = 8,
    MaleLessThan16 = 9,
    MaleWithDisabilityLessThan16 = 10,
    FemaleLessThan16 = 11,
    FemaleWithDisabilityLessThan16 = 12,
}

public enum PeriodicReportSections
{
    Beneficiaries = 1,
    PlannedActivites = 2,
    TechnicalQuestions = 3
}

public enum StageType
{
    Profile = 100,
    ConceptNotes = 200,
    Proposals = 300,
    CapacityDevelopment = 400,
    ContractImplementation = 500,
}

public enum ProposalStageSubType
{
    Technical = 100,
    Financial = 200,
}

public enum GovernoratesFundingScope
{
    InFundingScope = 1,
    NotInFundingScope = 2,
    Both = 3
}

public enum ActivityStatus
{
    InProgress = 1,
    Planned = 2,
    Completed = 3,
    Delayed = 4
}

public enum ImplementationPlanStatus
{
    Draft = 1,
    Designed = 2,
    AvailableForEdit = 3,
    EditRequested = 4,
    Approved = 5,
    Submitted = 6
}

public static class YesOrNo
{
    public const int Yes = 1;

    public const int No = 0;
}

public static class AcceptedOrRejected
{
    public const int Accepted = 1;

    public const int Rejected = 0;

    public const int Pending = 2;
}

public static class AllowEdit
{
    public static List<string> Codes { get; set; } =
    [
        "UE3Boo", "DKV7Qq", "DSNZbs", "FMeVyr", "p62eKr",
        "8Gf8z9", "6eIsn5", "KzP3Hs", "TxVmDi", "XwDQL5",
        "BTIepR", "sJYyOI", "nkMJdN", "3t1cVd", "Aj4o6h",
        "PaKjSu", "ILZ7wv", "qmUzPM", "anb9ia", "wPiLyu",
    ];
}

public static class BeneficiaryTypeId
{
    public const int MoreThanTwentyNineMaleWithoutDisability = 1;

    public const int MoreThanTwentyNineMaleWithDisability = 2;

    public const int MoreThanTwentyNineFemaleWithoutDisability = 3;

    public const int MoreThanTwentyNineFemaleDisability = 4;

    public const int MoreThanSixteenMaleWithoutDisability = 5;

    public const int MoreThanSixteenMaleWithDisability = 6;

    public const int MoreThanSixteenFemaleWithoutDisability = 7;

    public const int MoreThanSixteenFemaleDisability = 8;

    public const int LessThanSixteenMaleWithoutDisability = 9;

    public const int LessThanSixteenMaleWithDisability = 10;

    public const int LessThanSixteenFemaleWithoutDisability = 11;

    public const int LessThanSixteenFemaleDisability = 12;
}

public static class GovernorateFilterType
{
    public const int HeadQuarter = 1;

    public const int GeographicalScope = 2;

    public const int ProjectScope = 3;
}

public enum CycleSteps
{
    CycleDetails = 100,
    CycleStages = 200,
    ProfileForm = 300,
    ProfileScoring = 400,
    ProfileCategorization = 500,
    InvitationList = 600,
    All = 700,
}

public static class MarkdownUtils
{
    public static MarkdownPipeline MarkdownPipelines
    {
        get
        {
            var pipeline = new Markdig.MarkdownPipelineBuilder();
            pipeline.UseAdvancedExtensions().UseSoftlineBreakAsHardlineBreak();
            return pipeline.Build();
        }
    }
}