using System;
using System.Collections.Generic;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;
using ACS.MDB.Net.App.Common;

namespace ACS.MDB.Net.App.Services
{
    public class AuditReasonService
    {
        AuditReasonDAL auditReasonDAL = null;

        /// <summary>
        /// constructor
        /// </summary>
        public AuditReasonService()
        {
            auditReasonDAL = new AuditReasonDAL();
        }

        /// <summary>
        /// Get the audit reason List
        /// </summary>
        /// <returns>Value object of audit reason List</returns>
        public List<AuditReasonVO> GetReasonCodeList()
        {
            return auditReasonDAL.GetReasonCodeList();
        }

        /// <summary>
        /// Get Audit Reason Code List
        /// </summary>
        /// <returns>List of AuditReasonVO's</returns>
        public List<AuditReasonVO> GetAuditReasonCodeVOList()
        {
            return auditReasonDAL.GetAuditReasonCodeVOList();
        }

        /// <summary>
        /// Save the AuditReason (new or edited)
        /// </summary>
        /// <param name="model">The Audit Reason Details (model) and User Id</param>
        /// <returns></returns>
        // POST: /Administration/AuditReasonSave
        public void SaveAuditReason(AuditReasonVO auditReasonVO)
        {
            if (!String.IsNullOrEmpty(auditReasonVO.ReasonDescription))
            {
                AuditReasonVO auditReasonVOExist = auditReasonDAL.GetAuditReasonByName(auditReasonVO.ReasonDescription);

                //Check whether audit reason already exist or not
                if (auditReasonVOExist != null && auditReasonVOExist.ReasonCode != auditReasonVO.ReasonCode)
                {
                    throw new ApplicationException(Constants.AUDIT_REASON_ALREADY_EXISTS);
                }
                auditReasonDAL.SaveAuditReason(auditReasonVO);
            }
            else
            {
                throw new ApplicationException(Constants.AUDIT_REASON_CANNOT_BE_NULL);
            }
        }

        /// <summary>
        /// Delete the audit reasons
        /// </summary>
        /// <param name="Ids">The list of audit reason id's</param>
        /// <returns></returns>
        public void DeleteAuditReason(List<int> Ids, int? userId)
        {
            auditReasonDAL.DeleteAuditReason(Ids, userId);
        }

        /// <summary>
        /// Get audit reason By id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public AuditReasonVO GetAuditReasonById(int id)
        {
            return auditReasonDAL.GetAuditReasonById(id);
        }
    }
}