namespace ACS.MDB.Library.Common
{
    /// <summary>
    /// This class provides constants which will be used in whole application.
    /// </summary>
    public class Constants
    {
        

        /// <summary>
        /// Exception Error messages
        /// </summary>
        public const string CANNOT_SAVE = "Cannot save '{0}'. Please refer the errors on screen";

        public const string PLEASE_CONTACT_SYSTEM_ADMIN = "Your account has been deactivated. Please contact System Admin";
        public const string INVALID_USERNAME_OR_PASSWORD = "The username or password you entered is incorrect";
        public const string USER_NOT_EXIST = "User does not exists";
        public const string EMAIL_ID_EXIST = "User email id is already exists";
        public const string CURRENCY_ALREADY_EXIST = "Currency already exists";
        public const string CURRENCY_CANNOT_BE_INACTIVE = "Selected Currency cannot be deactivated as it is associated with the Contract";
        public const string CURRENCY_NOT_FOUND = "The selected currency was not found";
        public const string DIVISION_ALREADY_EXIST = "Division already exists";
        public const string DIVISION_CANNOT_BE_INACTIVE = "Selected Division cannot be deactivated as it is associated with the Contract";
        public const string DIVISION_ALREADY_ACTIVE_IN_COMPANY = "This Division '{0}' is already active in Company '{1}'";
        public const string PRODUCT_ALREADY_EXIST = "Product already exists";

        public const string SUBPRODUCT_ALREADY_EXIST = "Sub Product already exists";
        public const string COST_CENTRE_ALREADY_MAPPED_WITH_COMPANY = "This Cost centre '{0}' is already mapped with Company '{1}'";
        public const string INDEX_ALREADY_EXIST = "Index already exists";
        public const string INDEX_RATE_ALREADY_EXIST = "Index rate with entered month and year is already exists";
        public const string ITEM_NOT_FOUND = "Error : {0} not found";
        public const string AUDIT_REASON_CANNOT_BE_NULL = "Audit Reason cannot be left blank";
        public const string AUDIT_REASON_ALREADY_EXISTS = "Audit Reason already exists";

        //public const string SESSION_TIMEOUT = "Cannot save '{0}'. Session time out. Please log out and log in again";
        public const string SESSION_TIMEOUT = "Session Timeout";

        public const string USER_MUST_BE_ASSOCIATED_WITH_ATLEAST_ONE_COMPANY = "The User must be associated with atleast one company";
        public const string DEFAULT_COMPANY_MUST_BE_FROM_SELECTED_COMPANY_LIST = "The Default Company should be available in selected company list";
        public const string INVALID_DATE = "The date should be in 'dd/mm/yyyy' format";
        public const string ENDUSER_ALREADY_EXIST = "End User name already exists";
        public const string CUSTOMERCOMMENT_ALREADY_EXIST = "Customer Comment already Exist";
        public const string INVOICECUSTOMER_ALREADY_HAVE_COMMENT = "This  Invoice customer '{0}' already has comment for the selected company";

        /// <summary>
        /// Change password and forgot password
        /// </summary>
        public const string PASSWORD_NOT_MATCHED = "Entered Password does not match with the Current password";

        public const string EMAIL_ID_NOT_EXIST = "The email address entered does not exist";
        public const string TEMPORARY_PASSWORD = "temporary";
        public const string FIELD_EXEEDS_LENGTH_FOR_MULTILINE_TEXBOX = "{0} must be with a maximum length of {1}";

        /// <summary>
        /// Delete messages
        /// </summary>
        public const string DELETE_MESSAGE_FOR_INDEX = "Are you sure, you want to delete the selected Index(es) ? Index associated with Billing Details will not be deleted";
        public const string DELETE_MESSAGE_FOR_PRODUCT = "Are you sure, you want to delete the selected Product(s) ? Product associated with the Billing Details will not be deleted";
        public const string DELETE_MESSAGE_FOR_SUBPRODUCT = "Are you sure, you want to delete the selected Sub Product(s) ? Sub Product associated with Billing Details will not be deleted";
        public const string DELETE_MESSAGE_FOR_ENDUSER = "Are you sure you want to delete the End User(s) ? End User(s) associated with contract will not be deleted";
        public const string DELETE_MESSAGE_FOR_CUSTOMER_COMMENT = "Are you sure you want to delete the customer comments? Customer comments associated with contract will not be deleted";
        public const string DELETE_MESSAGE_FOR_CONTRACT = "Are you sure you want to delete the contract? Deleting the contract will also delete the associated Coding line(s), Billing Line(s) and Milestone(s)";
        public const string DELETE_MESSAGE_FOR_CONTRACT_LINE = "Are you sure, you want to delete the Coding line(s)? Coding line(s) associated with Billing line(s) and Milestone(s) will not be deleted";
        public const string DELETE_MESSAGE_FOR_MAINTENANCE_LINE = "Are you sure, you want to delete the Billing detail(s)? Billing detail(s) associated with Milestone(s) will not be deleted";
        public const string DELETE_MESSAGE_FOR_MILESTONE = "Are you sure you want to delete the Milestone(s)? Milestone(s) with only status ‘Ready for calculating’ will get deleted";
        public const string DELETE_MESSAGE_FOR_AUDIT_REASON = "Are you sure, you want to delete the Audit reason(s)? Audit Reason(s) associated with Contract Maintenance(s) will not be deleted";

        //Contract Maintenance error messages
        public const string INVALID_DATE_FOR = "The '{0}' should be in 'dd/mm/yyyy' format";

        public const string RENEWALDATE_REQUIRED = "Please enter First Renewal date";
        public const string ACTIVATIONDATE_REQUIRED = "Please enter Activation date";
        public const string FIRST_PERIOD_START_DATE_CANNOT_BE_GREATER_THAN_RENEWAL_DATE = "Please enter valid date, the First Period start date should not be greater than First Renewal date";
        public const string FINAL_BILLIING_INFO_REQUIRED = "Please enter Final billing period information";
        public const string FINAL_BILLIING_INFO_REQUIRED_FOR_CREDIT_OR_ADHOC = "Please enter Final Billing Period Start Date, End Date and Amount";
        public const string FINAL_BILLIING_RENEWAL_END_DATE_REQUIRED = "Please enter Final Billing Period end date";
        public const string FINAL_BILLIING_RENEWAL_START_DATE_REQUIRED = "Please enter Final Billing Period start date";
        public const string FINAL_RENEWAL_START_DATE_CANNOT_BE_GREATER_THAN_fINAL_RENEWAL_END_DATE = "The Final billing period start date should not be greater than Final billing period end date";
        public const string FINAL_BILLIING_AMOUNT_REQUIRED = "Please enter Final Billing Period amount";
        public const string FIRST_ANNUAL_UPLIFT_DATE_SHOULD_BE_AN_ANNIVERSARY_OF_FIRST_RENEWAL_DATE = "Please enter a valid 'First Annual uplift date', It should be anniversary of First Renewal date and cannot be more than 5 years";
        public const string FIRST_BILLING_RENEWAL_DATE_SHOULD_BE_AN_ANNIVERSARY_OF_FINAL_BILLING_RENEWAL_sTART_DATE = "Please enter a valid “Final billing period start date”, It should be anniversary of First Renewal date and cannot be more than 5 years";
        public const string FINAL_RENEWAL_START_DATE_END_DATE_AND_AMOUNT_REQUIRE = "Please enter start date, end date and amount for Final Billing Period";
        public const string FINAL_RENEWAL_START_DATE_AND_END_DATE_REQUIRE = "Please enter start date and end date for Final Billing Period";
        public const string NEGATIVE_AMOUNT_REQUIRE = "Amount and Base Annual Amount should be negative for the Credit charge frequency type";
        public const string POSITIVE_AMOUNT_REQUIRE = "Amount and Base Annual Amount should be positive";
        public const string PROVIDE_UPLIFT_REQIRE_DETAILS = "Please enter First Annual uplift date, Please enter Additional fixed, Please select Index";
        public const string UNCHECK_UPLIFT_REQUIRE_MESSAGE = "Are you sure you want to uncheck this? By uncheck this, First Annual uplift date , Additional Fixed and Inflation Index will get clear";
        public const string BACKLOG_WARNING_NO_MILESTONE = "Are you sure you want to change this? By selecting this, the First Period Start Date, First Renewal Date, Activation Date, First Annual uplift date and Final billing period information will get clear";
        public const string BACKLOG_WARNING_WITH_MILESTONE = "Are you sure you want to change this? By selecting this, all milestones will get delete and the First Period Start Date, First Renewal Date, Activation Date, First Annual uplift date and Final billing period information will get clear";
        public const string BACKLOG_WARNING_NOT_ALLOW_TO_CHANGE = "You are not allowed to change this option as the milestones are already processed";
        public const string FIRST_ANNUAL_UPLIFT_DATE_REQUIRE = "Please enter First Annual uplift date";
        public const string ADDITIONAL_FIXED_REQUIRE = "Please enter Additional fixed";
        public const string INFLATION_INDEX_REQUIRE = "Please select Inflation index";
        public const string INVALID_BILLING_TEXT = "Invalid billing line text {0}. Please ensure opening([) and closing(]) brackets are provided correctly";
        public const string DELETE_DATE_AND_TERMINATION_DATE_REQUIRE = "Please enter Termination date and select Termination reason";
        public const string PLEASE_SELECT_DELETE_REASON_AND_ENTER_TERMINATION_REASON = "Please select Delete reason and enter Termination reason";
        public const string PLEASE_ENTER_TERMINATION_REASON_AND_DELETE_DATE = "Please enter Termination reason and Termination date";
        public const string DELETE_REASON_REQUIRE = "Please select Termination reason";
        public const string TERMINATION_DATE_REQUIRE = "Please enter Termination date";
        public const string TERMINATION_DATE_CANNOT_BE_LESS_THAN_START_DATE = "Termination date cannot be less than Final Billing Period start date";
        public const string FINAL_RENEWAL_START_DATE_CANNOT_BE_LESS_THAN_FIRST_BILLING_RENEWAL_DATE = "Final Billing Period Start date cannot be less than First Renewal date";
        public const string NO_INVOICE_TEXT_LINE_TO_CALCULATE = "There are no invoice text lines to calculate. Please go and enter text lines on the Billing Line Tab.";

        /// <summary>
        /// Entity name constants
        /// </summary>
        public const string PRODUCT = "Product";

        public const string SUBPRODUCT = "Sub product";
        public const string DIVISION = "Division";
        public const string CURRENCY = "Currency";
        public const string AUDIT_REASON = "Audit Reason";
        public const string INDEX = "Index";
        public const string INDEXRATE = "Index Rate";
        public const string PROFITANDLOSS = "P&L";
        public const string USER = "User";
        public const string CONTRACT = "Contract";
        public const string CONTRACT_LINE = "Coding Details";
        public const string ENDUSER = "End User";
        public const string CUSTOMERCOMMENT = "Customer Comment";
        public const string CONTRACT_MAINTENANCE_LINE = "Billing Details";
        public const string MILESTONE = "Milestone";

        /// <summary>
        /// User Types
        /// </summary>
        public const string VIEWER_USER = "Viewer";

        public const string SUPER_USER = "SuperUser";

        /// <summary>
        /// Report constants
        /// </summary>
        public const string DUE_YEAR_DETAIL = "Due Year Detail";

        public const string MONTH_FORECAST = "Month Forcast";

        /// <summary>
        /// Report error messages
        /// </summary>
        public const string START_DATE_AND_END_DATE_ARE_MANDATORY = "Start Date and End Date are mandatory";

        public const string START_DATE_SHOULD_NOT_BE_GREATER_THAN_END_DATE = "Start Date should not be greater than End Date";

        /// <summary>
        /// String format for Decimal values (e.g. If value = 1254.15 then it display as 1,254.15 in datagrid)
        /// </summary>
        public const string STRING_FORMAT_FOR_NUMERIC_VALUE = "{0:#,0.00}";

        /// <summary>
        /// String format for Decimal values upto four digits (e.g. If value = 1254.15 then it display as 1,254.1500 in datagrid)
        /// </summary>
        public const string STRING_FORMAT_FOR_NUMERIC_VALUE_UPTO_FOUR_DECIMAL = "{0:#,0.0000}";

        /// <summary>
        /// String format for Date
        /// </summary>
        public const string DATE_FORMAT = "dd'/'MM'/'yyyy";

        /// <summary>
        /// String format to show Date and Time
        /// </summary>
        public const string DATE_TIME_FORMAT = "dd'/'MM'/'yyyy HH:mm:ss";

        /// <summary>
        /// String format for Date on view page
        /// </summary>
        public const string DATE_FORMAT_FOR_VIEWPAGE = "{0:dd'/'MM'/'yyyy}";

        /// <summary>
        /// string format for Dates
        /// </summary>
        public const string STRING_FORMAT_FOR_DATE = "dd'/'MM'/'yyyy";

        /// <summary>
        /// string format for Not applicable fileds for billing line information
        /// </summary>
        public const string NOT_APPLICABLE = "N/A";

        /// <summary>
        /// Error message if no milestone found with status approve for payment 
        /// </summary>
        public const string NO_MILESTONE_FOUND_FOR_AP = "No milestone found with status Approved & Approve for Payment.";

        public const string CURRENCY_NOT_AVAILABLE_FOR_CUSTOMER = "Associated currency with invoice customer is '{0}' and this is not available. Please contact administrator";

        public const string MISSING_BILLING_LINES = "The billing line is missing for Contract Number -";

        public const string MAINTENANCE_CREDIT_NOTE = "Maintenance Credit note";

        public const string MAINTENANCE = "Maintenance";

        public const string VALIDATION_RECALCULATION =
            "There are invalid Ad-Hoc or Credit lines, The recalculation can't run until they are corrected!";

        /// <summary>
        /// Message when Recalculation for inflation index done succefully
        /// </summary>
        public const string RECALCULATION_DONE_SUCCESSFULLY = "Recalculation of Inflation Index(s) has been done successfully, please check the log file. You may be redirected to the Login page due to session time out.";
        public const string RECALCULATION_DONE_SUCCESSFULLY_FOR_UPLIFT_REQUIRED = "Recalculation For Uplift Required Done Successfully";
        public const string RECALCULATION_DONE_SUCCESSFULLY_FOR_UPLIFT_NOT_REQUIRED = "Recalculation For Uplift Not Required Done Successfully";

        /// <summary>
        /// Enum for milestone status
        /// </summary>
        public enum MilestoneStatus
        {
            APPROVED_FOR_PAYMENT = 1,
            IN_PROGRESS = 4,
            LINK_LOADED = 6,
            COMPLETED_BY_USER = 7,
            PENDING = 8,
            READY_FOR_CALCULATING = 9,
            READY_FOR_INVOICING = 12,
            CANCELLED = 13
        }

        /// <summary>
        /// Enum for Charge Frequency
        /// </summary>
        public enum ChargeFrequency
        {
            MONTHLY = 1,
            QUARTERLY = 2,
            HALF_YEARLY = 3,
            YEARLY = 4,
            AD_HOC = 5,
            CREDIT = 6,
            BI_MONTHLY = 7
        }

        /// <summary>
        /// Bill to oa file generated successfully 
        /// </summary>
        public const string OPEN_ACCOUNT_FILE_GENERATE = "The Open Account output file generated successfully.";

        /// <summary>
        /// Error message if unc path username has not provided
        /// </summary>
        public const string PROVIDE_USERNAME_FOR_UNCPATH = "Please provide username to access - ";

        /// <summary>
        /// Error message if password is not provided for unc path
        /// </summary>
        public const string PROVIDE_PASSWORD_FOR_UNCPATH = "Please provide password to access - ";

        /// <summary>
        /// Error message while generating open account flat file
        /// </summary>
        public const string ERROR_GENERATING_OPENACCOUNT_FILE =
            "Error while generating Open Account flat file. LastError = ";

        /// <summary>
        /// if generating open account file for company 102 then file name should contain USMAI
        /// </summary>
        public const string USMAINT = "\\USMAI";

        /// <summary>
        /// if comapny is not 102 then filename should contain MAINT
        /// </summary>
        public const string MAINT = "\\MAINT";

        /// <summary>
        /// Header record type for bill to OA
        /// </summary>
        public const string HEADER = "H";

        /// <summary>
        /// Extra line record type X for footer line in BILL TO OA
        /// </summary>
        public const string RECORD_TYPE_X = "X";

        /// <summary>
        /// Reference type while bill to OA
        /// </summary>
        public const string ADVANCE_MAINT = "Advanced Maint";

        /// <summary>
        /// Record type T for footer row for company 102 only
        /// </summary>
        public const string RECORD_TYPE_T = "T";

        /// <summary>
        /// Document type 
        /// </summary>
        public const string DOCUMENT_TYPE_MASE = "MASE";

        /// <summary>
        /// Enum for Recalculation Status
        /// </summary>
        public enum RecalculationStatus
        {
            PENDING = 1,
            IN_PROGRESS = 2,
            COMPLETED = 3,
            COMPLETED_WITH_ERRORS = 4
        }

        /// <summary>
        /// Set The Recalculation Status
        /// </summary>
        public const string PENDING = "Pending";
        public const string IN_PROGRESS = "In Progress";
        public const string COMPLETED = "Completed successfully";
        public const string COMPLETED_WITH_ERRORS = "Completed  with error";


        public const string OAPERIOD_DATA_NOT_AVAILABLE = "OA Period Data for company '{0}' does not exist, on account of which the OA link file will not be generated.";

        /// <summary>
        /// Enum for Charge Frequency
        /// </summary>
        public enum Auditaction
        {
            UPADTE = 1,
            DELETE = 2
        }

        //SP name of theoretical revenue report
        public const string TheoreticalRevenueReport = "usp_Theoretical_Revenue_Forecast";

        //SP name of actual revenue report
        public const string ActualRevenueReport = "usp_Actual_Revenue_Forecast";

    }
}