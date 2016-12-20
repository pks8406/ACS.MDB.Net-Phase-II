using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.Common;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ApproveMaintenanceDAL : BaseDAL
    {
        /// <summary>
        /// Get list of milestone for approve maintenance
        /// </summary>
        /// <param name="companyId">company id</param>
        /// <param name="invoiceCustomerId">customer id</param>
        /// <param name="divisionId">division id</param>
        /// <param name="milestoneStatusId">milestone id</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        public List<MilestoneVO> GetApproveMaintenanceList(int? companyId, int? invoiceCustomerId, int? divisionId,
                                                           int? milestoneStatusId,
                                                            DateTime? startDate, DateTime? endDate, int? userId)
        {
            List<MilestoneVO> milestoneVOList = new List<MilestoneVO>();
            String BillingLinesAll = String.Empty;

            var result = (from milestone in mdbDataContext.Milestones
                          join contract in mdbDataContext.Contracts on milestone.ContractID equals contract.ID
                          join division in mdbDataContext.Divisions on contract.DivisionID equals division.ID
                          join customer in mdbDataContext.OACustomers on contract.InvoiceCustomerID equals customer.ID
                          where contract.CompanyID == companyId
                                &&
                                (customer.ID ==
                                 (invoiceCustomerId == -1 ? contract.InvoiceCustomerID : invoiceCustomerId))
                          && (division.ID == (divisionId == -1 ? contract.DivisionID : divisionId))
                          && (milestone.MilestoneStatusID == milestoneStatusId)
                          && (milestone.EstimatedDate >= startDate && milestone.EstimatedDate <= endDate)
                          && milestone.IsDeleted == false
                          select new
                          {
                              milestone.ID,
                              milestone.ContractID,
                              milestone.ContractLineID,
                              milestone.MaintenanceID,
                              contract.CompanyID,
                              contract.OACompany.CompanyName,
                              contract.InvoiceCustomerID,
                              contract.DivisionID,
                              contract.Division.DivisionName,
                              contract.ContractNumber,
                              milestone.Amount,
                              milestone.EstimatedDate,
                              milestone.MilestoneStatus1.StatusName,
                              milestone.MilestoneStatus1.Description,
                              customer.CustomerName,
                              milestone.RenewalStartDate,
                              milestone.RenewalEndDate,
                              milestone.IsApproved
                              //milestone.MilestoneBillingLines
                          }).ToList();

            //string BillingLines = String.Empty;
            foreach (var item in result)
            {
                ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.SingleOrDefault(cm => cm.ID == item.MaintenanceID);
                string comment = contractMaintenance.Comment;

                milestoneVOList.Add(new MilestoneVO
                {
                    ID = item.ID,
                    CompanyID = item.CompanyID,
                    CompanyName = item.CompanyName,
                    ContractID = item.ContractID,
                    ContractLineID = item.ContractLineID,
                    ContractMaintenanceID = item.MaintenanceID,
                    DivisionName = item.DivisionName,
                    ContractNumber = item.ContractNumber,
                    Amount = item.Amount,
                    InvoiceCustomerName = item.CustomerName,
                    InvoiceDate = item.EstimatedDate,
                    RenewalStartDate = item.RenewalStartDate,
                    RenewalEndDate = item.RenewalEndDate,
                    MilestoneStatusName = item.StatusName,
                    MilestoneStatusDescription = item.Description,
                    IsApproved = item.IsApproved
                });
            }

            return milestoneVOList;
        }

        /// <summary>
        /// Delete Milestones and associated details
        /// </summary>
        /// <param name="Ids">Ids of milestones to be deleted</param>
        /// <param name="userId">The logged in user id</param>
        public void DeleteApproveMaintenance(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    //Delete milestones
                    Milestone milestone = mdbDataContext.Milestones.SingleOrDefault(c => c.ID == id);
                    if (milestone.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                    {
                        milestone.IsDeleted = true;
                        milestone.LastUpdatedDate = DateTime.Now;
                        milestone.LastUpdatedBy = userId;

                        //Delete billing line tags
                        List<MilestoneBillingLine> milestoneBillingLines =
                            mdbDataContext.MilestoneBillingLines.Where(m => m.MilestoneID == id).ToList();

                        foreach (var item in milestoneBillingLines)
                        {
                            item.IsDeleted = true;
                            item.LastUpdatedDate = DateTime.Now;
                            item.LastUpdatedBy = userId;
                        }
                    }
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Approve milestones for payment
        /// </summary>
        /// <param name="Ids">Ids of milestones to be approved</param>
        /// <param name="userId">The logged in user id</param>
        public void ApproveAllMaintenance(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    Milestone milestone = mdbDataContext.Milestones.SingleOrDefault(c => c.ID == id);
                    if (milestone != null)
                    {
                        //If milestone status is "Ready for Calculating"
                        if (milestone.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING))
                        {
                            //Set milestone status as "Approved for Payment"
                            milestone.MilestoneStatusID = Convert.ToInt32(Constants.MilestoneStatus.APPROVED_FOR_PAYMENT);
                            milestone.IsApproved = true;
                            milestone.LastUpdatedDate = DateTime.Now;
                            milestone.LastUpdatedBy = userId;
                        }
                    }
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// UnApprove milestones for payment
        /// </summary>
        /// <param name="Ids">Ids of milestones to be unapproved</param>
        /// <param name="userId">The logged in user id</param>
        public void UnApproveAllMaintenance(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    Milestone milestone = mdbDataContext.Milestones.SingleOrDefault(c => c.ID == id);
                    if (milestone != null)
                    {
                        //If milestone status is "Approved for Payment"
                        if (milestone.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.APPROVED_FOR_PAYMENT))
                        {
                            //Set milestone status as "Ready for Calculating"
                            milestone.MilestoneStatusID = Convert.ToInt32(Constants.MilestoneStatus.READY_FOR_CALCULATING);
                            milestone.IsApproved = false;
                            milestone.LastUpdatedDate = DateTime.Now;
                            milestone.LastUpdatedBy = userId;
                        }
                    }
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="divisionId"></param>
        /// <param name="invoiceCustomerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<InvoiceHeaderVO> GenerateInvoice(int companyId, int divisionId, int invoiceCustomerId,
                                                     DateTime fromDate, DateTime toDate, int? userId)
        {
            var milestones = (from milestone in mdbDataContext.Milestones
                              join contract in mdbDataContext.Contracts on milestone.ContractID equals
                                  contract.ID
                              join division in mdbDataContext.Divisions on contract.DivisionID equals
                                  division.ID
                              join customer in mdbDataContext.OACustomers on contract.InvoiceCustomerID equals
                                  customer.ID
                              join contractMaintenance in mdbDataContext.ContractMaintenances on milestone.MaintenanceID equals
                                  contractMaintenance.ID
                              where contract.CompanyID == companyId
                                    &&
                                    (customer.ID ==
                                     (invoiceCustomerId == -1
                                          ? contract.InvoiceCustomerID
                                          : invoiceCustomerId))
                                    && (division.ID == (divisionId == -1 ? contract.DivisionID : divisionId))
                                    && (milestone.EstimatedDate >= fromDate && milestone.EstimatedDate <= toDate)
                                    && milestone.MilestoneStatusID == 1 && milestone.IsApproved // 1 - Approved for Payment
                                    && milestone.IsDeleted == false
                              select new
                                         {
                                             milestone.ID,
                                             milestone.ContractID,
                                             milestone.ContractLineID,
                                             milestone.MaintenanceID,
                                             //credit = milestone.Amount < 0,
                                             contract.CompanyID,
                                             contract.InvoiceCustomerID,
                                             contractMaintenance.DocumentTypeID
                                         }).GroupBy(g => new { g.ContractID, g.CompanyID, g.InvoiceCustomerID, g.DocumentTypeID }).ToList();

            // Set milestone status as In Progress so other user can not access/generate same invoice 
            foreach (var ml in milestones.SelectMany(item => item))
            {
                // 4 = In Progress
                SetMilestoneStatus(ml.ID, userId, Convert.ToInt32(Constants.MilestoneStatus.IN_PROGRESS), DateTime.Now);
            }

            return milestones.Select(milestone => new InvoiceHeaderVO
                                                      {
                                                          ContractId = milestone.Key.ContractID,
                                                          CompanyId = milestone.Key.CompanyID,
                                                          InvoiceCustomerId = milestone.Key.InvoiceCustomerID,
                                                          DocumentTypeID = milestone.Key.DocumentTypeID                                                          
                                                      }).ToList();
        }

        /// <summary>
        /// Set milestone status of milestone to In Progress so no other user can access/generate invoice 
        /// </summary>
        /// <param name="milestoneId">The milestone id</param>
        /// <param name="userId"></param>
        /// <param name="milestoneStatus">Set milestone status</param>
        /// <param name="invoiceDate">Invoice date actual bill date</param>
        private void SetMilestoneStatus(int milestoneId, int? userId, int milestoneStatus, DateTime invoiceDate)
        {
            Milestone milestone = mdbDataContext.Milestones.Single(m => m.ID == milestoneId);
            
            if (milestone == null) return;

            milestone.MilestoneStatus1 = mdbDataContext.MilestoneStatus.Single(m => m.ID == milestoneStatus);

            //Set billing date if milestone status is link loaded
            if (milestoneStatus == Convert.ToInt32(Constants.MilestoneStatus.LINK_LOADED))
            {
                milestone.BillDate = invoiceDate;
            }

            //milestone.MilestoneStatusID = ms.ID;
            milestone.LastUpdatedBy = userId;
            milestone.LastUpdatedDate = DateTime.Now;

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Process invoice 
        /// </summary>
        /// <param name="invoiceHeaderVo"></param>
        public InvoiceHeaderVO GetContarctDetails(InvoiceHeaderVO invoiceHeaderVo, DateTime invoiceDate)
        {
            Contract contract =
                mdbDataContext.Contracts.SingleOrDefault(c => c.ID == invoiceHeaderVo.ContractId && !c.IsDeleted);

            // Get posting period & posting year details from OA Period table
            //List<int> postingData = GetPeriod(contract.CompanyID);

            InvoiceHeaderVO invoiceHeaderVO = new InvoiceHeaderVO(contract, invoiceDate, invoiceHeaderVo.DocumentTypeID);

            return invoiceHeaderVO;
        }

        /// <summary>
        /// Get general ledger details for invoice
        /// </summary>
        /// <param name="invoiceHeaderVO"></param>
        /// <returns></returns>
        public List<InvoiceGLDetailVO> GetInvoiceDetails(InvoiceHeaderVO invoiceHeaderVO, DateTime fromDate, DateTime toDate)
        {

            List<Milestone> milestones = mdbDataContext.Milestones.Where(m => m.ContractID == invoiceHeaderVO.ContractId &&
                                                                   m.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.IN_PROGRESS) // 4 In Progress
                                                                   && m.ContractMaintenance.DocumentTypeID == invoiceHeaderVO.DocumentTypeID
                                                                   && (m.EstimatedDate >= fromDate && m.EstimatedDate <= toDate)
                                                                   && !m.IsDeleted).ToList();

            List<InvoiceGLDetailVO> invoiceGlDetailVos = new List<InvoiceGLDetailVO>();

            foreach (var item in milestones)
            {
                InvoiceGLDetailVO invoiceGlDetailVO = new InvoiceGLDetailVO(item);

                if (!invoiceGlDetailVos.Contains(invoiceGlDetailVO))
                {
                    invoiceGlDetailVos.Add(invoiceGlDetailVO);    
                }
            }

            return invoiceGlDetailVos;

            //List<ContractMaintenance> contractMaintenance = mdbDataContext.ContractMaintenances.Where(cm => cm.ContractID == invoiceHeaderVO.ContractId
            //                                                                                                 && cm.DocumentTypeID == invoiceHeaderVO.DocumentTypeID).ToList();

            //List<ContractLine> contractLines = new  List<ContractLine>();
            //foreach (var item in contractMaintenance)
            //{
            //    contractLines.Add(item.ContractLine);
            //}

            //Get contract line details for contracts
            //List<ContractLine> contractLines = mdbDataContext.ContractLines
            //                                                 .Where(
            //                                                     cl =>
            //                                                     cl.ContractID == invoiceHeaderVO.ContractId &&
            //                                                     !cl.IsDeleted)
            //                                                 .ToList();
            //return contractLines.Select(contractLine => new InvoiceGLDetailVO(contractLine)).ToList();


        }

        /// <summary>
        /// Gets invoice milestone details for invoice file generation
        /// </summary>
        /// <param name="invoiceHeaderVO">Header details</param>
        /// <param name="invoiceGLDetail">GL coding details</param>
        /// <param name="fromDate">from date</param>
        /// <param name="toDate">to date</param>
        /// <returns></returns>
        public MilestoneVO GetMilestoneDetails(InvoiceHeaderVO invoiceHeaderVO, InvoiceGLDetailVO invoiceGLDetails,
                                                     DateTime fromDate, DateTime toDate)
        {
                Milestone milestones =
                    mdbDataContext.Milestones.FirstOrDefault(m => m.ID == invoiceGLDetails.MilestoneId && !m.IsDeleted);
                                                       //Where(m => m.ContractLineID == invoiceGlDetailVO.ContractLineId
                                                       //           && m.ContractID == invoiceHeaderVO.ContractId
                                                       //           && m.ContractMaintenance.ID == invoiceGlDetailVO.ContractMaintenanceId &&
                                                       //           m.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.IN_PROGRESS) // 4 In Progress
                                                       //           && m.ContractMaintenance.IsGrouped == null &&
                                                       //           (m.EstimatedDate >= fromDate &&
                                                       //            m.EstimatedDate <= toDate)
                                                       //           && !m.IsDeleted).ToList();

                //foreach (var milestone in milestones)
                //{
                MilestoneVO milestoneVO = new MilestoneVO(milestones);

                //}

                return milestoneVO;

            //Get milestone details for contract & contract lines
            //List<Milestone> milestones = mdbDataContext.Milestones.
            //                                            Where(m => m.ContractLineID == invoiceGLDetail.ContractLineId
            //                                                       && m.ContractID == invoiceHeaderVO.ContractId 
            //                                                       && m.ContractMaintenance.ID == invoiceGLDetail.ContractMaintenanceId &&
            //                                                       m.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.IN_PROGRESS) // 4 In Progress
            //                                                       && m.ContractMaintenance.IsGrouped == null &&
            //                                                       (m.EstimatedDate >= fromDate &&
            //                                                        m.EstimatedDate <= toDate)
            //                                                       && !m.IsDeleted).ToList();

            //return milestones.Select(milestone => new MilestoneVO(milestone)).ToList();
        }

        /// <summary>
        /// Return default billing 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="groupId"></param>
        /// <param name="periodFrequencyId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public MilestoneVO GetDefaultBillingLine(int contractId, int? groupId, int periodFrequencyId, DateTime fromDate,
                                                 DateTime toDate)
        {
            Milestone milestone =
                mdbDataContext.Milestones.FirstOrDefault(
                    m => m.ContractID == contractId && m.ContractMaintenance.GroupId == groupId
                         && m.ContractMaintenance.ChargeFrequencyID == periodFrequencyId &&
                         m.ContractMaintenance.IsDefaultLineInGroup == true && !m.IsDeleted && !m.ContractMaintenance.IsDeleted
                         && m.EstimatedDate >= fromDate && m.EstimatedDate <= toDate);

            #region Khushboo
            
            MilestoneVO milestoneVO = null;

            if (milestone != null)
            {
                //MilestoneVO milestoneVO = new MilestoneVO(milestone);
                milestoneVO = new MilestoneVO(milestone);
                //return milestoneVO;
            }
            else
            {
                //Billing line tag of that particular(original billing text) billing line
                Milestone milestoneOfParticularBillingLine =
                mdbDataContext.Milestones.FirstOrDefault(
                    m => m.ContractID == contractId && m.ContractMaintenance.GroupId == groupId
                         && m.ContractMaintenance.ChargeFrequencyID == periodFrequencyId &&
                         !m.IsDeleted && !m.ContractMaintenance.IsDeleted
                         && m.EstimatedDate >= fromDate && m.EstimatedDate <= toDate);
                milestoneVO = new MilestoneVO(milestoneOfParticularBillingLine);
                //return null;
            }
            return milestoneVO;
            #endregion
        }

  

        /// <summary>
        /// Return list of milestone of grouped contract maintenance lines
        /// </summary>
        /// <param name="invoiceHeaderVO">The invoice header deatils</param>
        /// <param name="invoiceGLDetail">The coding line deatils</param>
        /// <param name="fromDate">From date</param>
        /// <param name="toDate">To date</param>
        /// <returns></returns>
        public List<MilestoneVO> GetGroupedMilestoneDetails(InvoiceHeaderVO invoiceHeaderVO, List<InvoiceGLDetailVO> invoiceGLDetail,
                                                    DateTime fromDate, DateTime toDate)
        {
            List<MilestoneVO> milestoneVos = new List<MilestoneVO>();

            foreach (var invoiceGlDetailVO in invoiceGLDetail)
            {
             
                //Get milestone details for contract & contract lines
                List<Milestone> milestones = mdbDataContext.Milestones.
                                                            Where(m => m.ContractLineID == invoiceGlDetailVO.ContractLineId
                                                                       && m.ContractID == invoiceHeaderVO.ContractId &&
                                                                       m.MilestoneStatusID == Convert.ToInt32(Constants.MilestoneStatus.IN_PROGRESS) // 4 In Progress
                                                                       && (m.EstimatedDate >= fromDate && m.EstimatedDate <= toDate)
                                                                       && m.ContractMaintenance.IsGrouped == true
                                                                       && m.ContractMaintenance.GroupId == invoiceGlDetailVO.GroupId
                                                                       && m.ContractMaintenance.ChargeFrequencyID == invoiceGlDetailVO.PeriodFrequencyId
                                                                       && !m.IsDeleted).ToList();

                foreach (var milestone in milestones)
                {
                   MilestoneVO milestoneVO = new MilestoneVO(milestone);

                    MilestoneVO mlVO = milestoneVos.Find(m => m.ID == milestoneVO.ID);

                    if(mlVO == null)
                    {
                        milestoneVos.Add(milestoneVO);
                    }
                }
            }

            //var gpMilestones = (from milestone in milestoneVos
            //                    select new
            //                               {
            //                                   milestone.ContractLineID,
            //                                   milestone.GroupId,
            //                                   milestone.PeriodFrequencyId
            //                               }).GroupBy(g => new {g.ContractLineID, g.GroupId, g.PeriodFrequencyId}).ToList();

            //return gpMilestones;
            return milestoneVos;
            //return milestones.Select(milestone => new MilestoneVO(milestone)).ToList();
        }

        /// <summary>
        /// Get Posting Period & Posting Year detais from OA Period table
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<OAPeriodVO> GetPeriod(int companyId)
        {
            List<OAPeriod> oaPeriods = mdbDataContext.OAPeriods.Where(p => p.CompanyID == companyId && !p.IsDeleted).ToList();

            return oaPeriods.Select(oaPeriodVo => new OAPeriodVO(oaPeriodVo)).ToList();
        }


        /// <summary>
        /// Set milestone status to link loaded
        /// </summary>
        /// <param name="companyId">The company id</param>
        /// <param name="divisionId">The division id</param>
        /// <param name="invoiceCustomerId">The customer id</param>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="userId">Logged in user id</param>
        /// <param name="milestoneStatusId"></param>
        public void UpdateMilestoneStatus(int companyId, int divisionId, int invoiceCustomerId,
                                          DateTime startDate, DateTime endDate, int? userId, int milestoneStatusId, DateTime invoiceDate)
        {
            var milestones = (from milestone in mdbDataContext.Milestones
                               join contract in mdbDataContext.Contracts on milestone.ContractID equals
                                   contract.ID
                               join division in mdbDataContext.Divisions on contract.DivisionID equals
                                   division.ID
                               join customer in mdbDataContext.OACustomers on contract.InvoiceCustomerID
                                   equals customer.ID
                               where contract.CompanyID == companyId
                                     &&
                                     (customer.ID ==
                                      (invoiceCustomerId == -1
                                           ? contract.InvoiceCustomerID
                                           : invoiceCustomerId))
                                     &&
                                     (division.ID == (divisionId == -1 ? contract.DivisionID : divisionId))
                                     &&
                                     (milestone.EstimatedDate >= startDate &&
                                      milestone.EstimatedDate <= endDate)
                                     && milestone.MilestoneStatusID == (int) Constants.MilestoneStatus.IN_PROGRESS
                                     && milestone.IsDeleted == false
                               select new
                                          {
                                              milestone.ID
                                          }).ToList();

            foreach (var milestone in milestones)
            {
                SetMilestoneStatus(milestone.ID, userId, milestoneStatusId, invoiceDate);
            }
        }

        /// <summary>
        /// Return contract line deatils based on contract maintenance id
        /// </summary>
        /// <param name="contractMaintenanceId">Contract maintenance id</param>
        /// <returns>Return contract line</returns>
        public InvoiceGLDetailVO GetContractLineBasedOnContractMaintenanceId(int contractMaintenanceId)
        {
            ContractMaintenance contractMaintenance = mdbDataContext.ContractMaintenances.FirstOrDefault(cm => cm.ID == contractMaintenanceId & !cm.IsDeleted);

            InvoiceGLDetailVO contractLineVO = new InvoiceGLDetailVO(contractMaintenance.ContractLine);

            return contractLineVO;

        }
    }
}
