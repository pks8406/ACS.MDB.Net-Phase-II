using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ACS.MDB.Net.App.Common;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class GenerateInvoiceService
    {
        /// <summary>
        /// path of the log file
        /// </summary>
        private string logFilePath = string.Empty;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GenerateInvoiceService()
        {
            logFilePath = ApplicationConfiguration.GetOAFlatFileLocation();
        }

        /// <summary>
        /// Returns the current date and time
        /// with proper format
        /// </summary>
        /// <returns>Current date and time</returns>
        private string GetCurrentDateTime()
        {
            return String.Format("{0:yyMMdd-HHmmss}", DateTime.Now) + ".txt";
        }

        /// <summary>
        /// Gets fully qualified path of the log file
        /// </summary>
        private string GetLogPath()
        {
            if (!String.IsNullOrWhiteSpace(logFilePath))
            {
                bool isExists = Directory.Exists((logFilePath));

                if (!isExists)
                    Directory.CreateDirectory(logFilePath);
            }
            else
            {
                //logFilePath = "C:\\MDB\\MDBOASyncLog";
                Directory.CreateDirectory(logFilePath);
            }

            return logFilePath;
        }

       /// <summary>
       /// List if invoices write to flat file
       /// </summary>
       /// <param name="InvoiceHeaderVos"></param>
        public void Write(List<InvoiceHeaderVO> InvoiceHeaderVos)
        {
           try
           {

         
            if (ApplicationConfiguration.UseUNCPath())
            {
                string userName = ApplicationConfiguration.GetUNCUserName();
                string password = ApplicationConfiguration.GetUNCPassword();
                string domainName = ApplicationConfiguration.GetUNCDomainName();

                // Connect to remote machine to access remote location
                using (UNCAccessWithCredentialService unc = new UNCAccessWithCredentialService())
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw new ApplicationException(Constants.PROVIDE_USERNAME_FOR_UNCPATH + logFilePath);
                    }

                    if (string.IsNullOrEmpty(password))
                    {
                        throw new ApplicationException(Constants.PROVIDE_PASSWORD_FOR_UNCPATH + logFilePath);
                    }

                    if (unc.NetUseWithCredentials(logFilePath,
                                                  userName,
                                                  domainName,
                                                  password))
                    {
                        // Write file on remote location
                        WriteToFile(InvoiceHeaderVos);
                    }
                    else
                    {
                        throw new ApplicationException(Constants.ERROR_GENERATING_OPENACCOUNT_FILE + unc.LastError.ToString());
                    }
                }
            }
            else
            {
                WriteToFile(InvoiceHeaderVos);    
            }
           }
           catch (Exception e)
           {

               throw new ApplicationException(e.Message);
           }
        }

        /// <summary>
        /// Write invoice details to specified location
        /// </summary>
        /// <param name="InvoiceHeaderVos"></param>
        private void WriteToFile(List<InvoiceHeaderVO> InvoiceHeaderVos)
        {
            StreamWriter writer = null;
            
            try
            {
                ValidateLocation(InvoiceHeaderVos[0].CompanyId);

                writer = new StreamWriter(logFilePath, false,Encoding.GetEncoding("Windows-1252"));

                foreach (var invoiceHeaderVo in InvoiceHeaderVos)
                {
                    string documentType = invoiceHeaderVo.TotalAmount > 0
                                              ? invoiceHeaderVo.DocumentTypeIN
                                              : invoiceHeaderVo.DocumentTypeCR;

                    string header = invoiceHeaderVo.RecordType + "|" + invoiceHeaderVo.CompanyId + "|" + documentType +
                                    "|" + invoiceHeaderVo.Reference + "|" + invoiceHeaderVo.Currency + "|" +
                                    invoiceHeaderVo.Description + "|"
                                    + invoiceHeaderVo.TotalAmount + "|" + invoiceHeaderVo.CustomerCode + "|" +
                                    invoiceHeaderVo.PostingPeriod + "|"
                                    + invoiceHeaderVo.PostingYear + "|" + invoiceHeaderVo.DocumentDate + "|" +
                                    invoiceHeaderVo.Status + "|"
                                    + invoiceHeaderVo.Field;

                    //Write invoice header
                    writer.WriteLine(header);

                    foreach (var invoiceDetail in invoiceHeaderVo.InvoiceDetailVos)
                    {
                        //Write N line in the file
                        foreach (var nominalLine in invoiceDetail.NominalLinesList)
                        {
                            string glDetail = nominalLine.RecordType + "|" +
                                       nominalLine.CostCentre + "|" +
                                       nominalLine.AccountCode + "|" +
                                       nominalLine.JobCode + "|" +
                                       nominalLine.ActivityCode + "|" +
                                       nominalLine.Value + "|" +
                                       nominalLine.TaxCode + "|" +
                                       nominalLine.ContractDetails + "|" +
                                       nominalLine.Field;
                            writer.WriteLine(glDetail);
                        }

                        //string glDetail = invoiceDetail.InvoiceGlDetails.RecordType + "|" +
                        //                  invoiceDetail.InvoiceGlDetails.CostCentre + "|" +
                        //                  invoiceDetail.InvoiceGlDetails.AccountCode + "|" +
                        //                  invoiceDetail.InvoiceGlDetails.JobCode + "|" +
                        //                  invoiceDetail.InvoiceGlDetails.ActivityCode + "|" +
                        //                  invoiceDetail.InvoiceGlDetails.Value + "|" +
                        //                  invoiceDetail.InvoiceGlDetails.TaxCode + "|" +
                        //                  invoiceDetail.InvoiceGlDetails.ContractDetails + "|" +
                        //                  invoiceDetail.InvoiceGlDetails.Field;

                        // Billing lines
                        string line = String.Join("|", invoiceDetail.InvoiceBillingLines.BillingLines);

                        string billingLines = invoiceDetail.InvoiceBillingLines.RecordType + "|" +
                                              invoiceDetail.InvoiceBillingLines.DocumentType + "|" +
                                              invoiceDetail.InvoiceBillingLines.Field3 + "|" +
                                              invoiceDetail.InvoiceBillingLines.VatCode + "|" +
                                              line + "|" + invoiceDetail.InvoiceBillingLines.UnitPrice + "|" +
                                              invoiceDetail.InvoiceBillingLines.Qty +
                                              "|" + invoiceDetail.InvoiceBillingLines.Amount +
                                              invoiceDetail.InvoiceBillingLines.Fields;

                        //Write invoice GL coding details
                       
                        writer.WriteLine(billingLines);
                    }

                    string footerLine = string.Empty;

                    //Footer Line for all companies
                    footerLine = invoiceHeaderVo.FooterBillingLine.RecordType + "|" +
                                 invoiceHeaderVo.FooterBillingLine.DocumentType + "|" +
                                 String.Join("|", new string[21]) + invoiceHeaderVo.FooterBillingLine.Fields;

                    writer.WriteLine(footerLine);

                    //Added one Extra footer Line 'T' for ComapnyId = 102
                    if (invoiceHeaderVo.CompanyId == 102)
                    {
                        footerLine = invoiceHeaderVo.FooterBillingLine.RecordTypeForT + "|" +
                                     invoiceHeaderVo.FooterBillingLine.DocumentTypeForT + "|" +
                                     invoiceHeaderVo.TotalAmount + "|" + invoiceHeaderVo.FooterBillingLine.VatCode +
                                     String.Join("|", new string[21]); //+ invoiceHeaderVo.FooterBillingLine.Fields;

                        writer.WriteLine(footerLine);
                    }
                    //else
                    //{
                    //    footerLine = invoiceHeaderVo.FooterBillingLine.RecordType + "|" +
                    //                 invoiceHeaderVo.FooterBillingLine.DocumentType + "|" +
                    //                 String.Join("|", new string[21]) + invoiceHeaderVo.FooterBillingLine.Fields;
                    //}
                }
            }
            // If file failed to generate 
            catch (IOException io)
            {
                
                if (io.Message.Contains("The network path was not found."))
                {
                    throw new ApplicationException(Constants.OPEN_ACCOUNT_LOCATION_NOT_ACCESSIBLE);
                }

                 throw new ApplicationException(Constants.FAILED_TO_GENERATE_OPEN_ACCOUNT_FILE);
            }

            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            finally
            {
                //Cleanup buffers and
                //close the writer
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Validate location create directory if not exists
        /// </summary>
        /// <param name="InvoiceHeaderVos"></param>
        private void ValidateLocation(int companyId)
        {
            if (!String.IsNullOrWhiteSpace(logFilePath))
            {
                bool isExists = Directory.Exists((logFilePath));

                if (!isExists)
                    Directory.CreateDirectory(logFilePath);
            }
            else
            {
                //logFilePath = "C:\\MDB\\MDBOASyncLog";
                Directory.CreateDirectory(logFilePath);
            }

            string name = companyId == 102 ? Constants.USMAINT : Constants.MAINT;
            string dateTime = GetCurrentDateTime();

            // Fix of ARBS-42 
            //When multiple users simultaneously click on Bill To OA button application not generating text file 
            //for user where date & time of click on Bill To OA button is same. 

            while (File.Exists(logFilePath + name + dateTime))
            {
                // if file exist with same name and datetime
                dateTime = GetCurrentDateTime();
            }

            logFilePath += (name + dateTime);
        }
    }
}