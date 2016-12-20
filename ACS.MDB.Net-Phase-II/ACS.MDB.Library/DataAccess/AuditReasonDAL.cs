using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class AuditReasonDAL : BaseDAL
    {
        /// <summary>
        /// Get the audit reason List
        /// </summary>
        /// <returns>Value object of audit reason List</returns>
        public List<AuditReasonVO> GetReasonCodeList()
        {
            List<AuditReasonVO> auditReasonVOList = new List<AuditReasonVO>();
            List<AuditReason> auditReasonList = mdbDataContext.AuditReasons.Where(c => c.IsDeleted == false).OrderBy(c => c.ReasonCode).ToList();

            foreach (AuditReason item in auditReasonList)
            {
                auditReasonVOList.Add(new AuditReasonVO(item));
            }

            return auditReasonVOList;
        }

        /// <summary>
        /// Get the audit reason VO List
        /// </summary>
        /// <returns>Value object of audit reason List</returns>
        public List<AuditReasonVO> GetAuditReasonCodeVOList()
        {
            List<AuditReasonVO> auditReasonVOList = new List<AuditReasonVO>();
            List<AuditReason> auditReasonList = mdbDataContext.AuditReasons.Where(c=>c.IsDeleted==false).OrderBy(c => c.ReasonCode).ToList();

            foreach (LINQ.AuditReason auditReason in auditReasonList)
            {
                AuditReasonVO auditReasonVO = new AuditReasonVO(auditReason);
                auditReasonVOList.Add(auditReasonVO);
            }

            return auditReasonVOList;
        }

        /// <summary>
        /// Save the AuditReason (new or edited)
        /// </summary>
        /// <param name="model">The Audit Reason Details (model)</param>
        /// <returns></returns>
        // POST: /Administration/AuditReasonSave
        public void SaveAuditReason(AuditReasonVO auditReasonVO)
        {
            AuditReason auditReason = null;

            if (auditReasonVO.ReasonCode == 0)
            {
                //create new audit reason
                auditReason = new AuditReason();
                auditReason.CreationDate = DateTime.Now;
                auditReason.CreatedBy = auditReasonVO.CreatedByUserId;
            }
            else
            {
                //get audit reason for update
                auditReason = mdbDataContext.AuditReasons.SingleOrDefault(a => a.ReasonCode == auditReasonVO.ReasonCode);
                auditReason.LastUpdationDate = DateTime.Now;
                auditReason.LastUpdatedBy = auditReasonVO.LastUpdatedByUserId;
            }

            //Create or update audit reason details
            auditReason.ReasonDescription = auditReasonVO.ReasonDescription;

            if (auditReasonVO.ReasonCode == 0)
            {
                //If new audit reason
                mdbDataContext.AuditReasons.InsertOnSubmit(auditReason);
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Delete the audit reasons
        /// </summary>
        /// <param name="Ids">The list of audit reason id's</param>
        /// <returns></returns>
        public void DeleteAuditReason(List<int> Ids, int? userId)
        {
            AuditReason auditReason= new AuditReason();
            
            foreach (var id in Ids)
            {
                if (id != 0)
                {
                    auditReason = mdbDataContext.AuditReasons.Where(a => a.ReasonCode == id).SingleOrDefault();

                    //To check weather audit reason is associated with contratmaintaince or not
                    ContractMaintenance contractMaintaince = mdbDataContext.ContractMaintenances.Where(c => (c.DeleteReason == id || c.ReasonCode == id) && !c.IsDeleted).FirstOrDefault();

                    if (contractMaintaince == null)
                    {
                        auditReason.IsDeleted = true;
                        auditReason.LastUpdatedBy = userId;
                        auditReason.LastUpdationDate = DateTime.Now;
                    }
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Get audit reason by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public AuditReasonVO GetAuditReasonById(int id)
        {
            AuditReason auditReason = mdbDataContext.AuditReasons.SingleOrDefault(a => a.ReasonCode == id);

            AuditReasonVO auditReasonVO = null;

            if (auditReason != null)
            {
                auditReasonVO = new AuditReasonVO(auditReason);
            }
            return auditReasonVO;
        }

        /// <summary>
        /// Get the audit reason by name
        /// </summary>
        /// <returns></returns>
        public AuditReasonVO GetAuditReasonByName(string auditReasonDescription)
        {
            AuditReason auditReason = mdbDataContext.AuditReasons.Where(x => x.ReasonDescription == auditReasonDescription && x.IsDeleted == false).FirstOrDefault();
            AuditReasonVO auditReasonVO = null;

            if (auditReason != null)
            {
                auditReasonVO = new AuditReasonVO(auditReason);
            }
            return auditReasonVO;
        }
    }
}