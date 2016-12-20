using System;
using System.Collections.Generic;
using System.Linq;
using ACS.MDB.Library.DataAccess.LINQ;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Library.DataAccess
{
    public class ContractLineDAL : BaseDAL
    {
        /// <summary>
        /// Gets the list of ContractLines based on Contract Id
        /// </summary>
        /// <param name="contractId">ContractId</param>
        /// <returns>List of Contract Lines</returns>
        public List<ContractLineVO> GetContractLineByContractId(int contractId)
        {
            List<ContractLine> contractLine = mdbDataContext.ContractLines.Where(c => c.ContractID == contractId && c.IsDeleted == false).ToList();
            List<ContractLineVO> contractLineVOList = new List<ContractLineVO>();
            foreach (var item in contractLine)
            {
                contractLineVOList.Add(new ContractLineVO(item));
            }
            return contractLineVOList;
        }

        /// <summary>
        /// Save the Contract Line
        /// </summary>
        /// <param name="contractLineVO">Value object of Contract Line</param>
        public void ContractLineSave(ContractLineVO contractLineVO)
        {
            if (contractLineVO.ContractLineID == 0)
            {
                //Insert New Record
                ContractLine newContractLine = new ContractLine();
                newContractLine.ContractID = contractLineVO.ContractID;
                newContractLine.ActivityCategoryID = contractLineVO.ActivityCategoryId;
                newContractLine.ActivityCodeID = contractLineVO.ActivityCodeId;
                newContractLine.AccountCodeID = contractLineVO.AccountId;
                newContractLine.JobCodeID = contractLineVO.JobCodeId;
                newContractLine.CostCentreID = contractLineVO.CostCenterId;
                //newContractLine.QTY = contractLineVO.QTY;
                newContractLine.CreationDate = DateTime.Now;
                newContractLine.CreatedBy = contractLineVO.CreatedByUserId;
                mdbDataContext.ContractLines.InsertOnSubmit(newContractLine);
                mdbDataContext.SubmitChanges();
            }
            else
            {
                //Update Existing Record
                ContractLine selectedContractLine = mdbDataContext.ContractLines.SingleOrDefault(c => c.ID == contractLineVO.ContractLineID);
                selectedContractLine.ActivityCategoryID = contractLineVO.ActivityCategoryId;
                selectedContractLine.ActivityCodeID = contractLineVO.ActivityCodeId;
                selectedContractLine.AccountCodeID = contractLineVO.AccountId;
                selectedContractLine.JobCodeID = contractLineVO.JobCodeId;
                selectedContractLine.CostCentreID = contractLineVO.CostCenterId;
                // selectedContractLine.QTY = contractLineVO.QTY;
                selectedContractLine.LastUpdatedBy = contractLineVO.LastUpdatedByUserId;
                selectedContractLine.LastUpdatedDate = DateTime.Now;
                mdbDataContext.SubmitChanges();
            }
        }

        /// <summary>
        ///  Gets the ContractLine details by contractLine id.
        /// </summary>
        /// <param name="contractLineId">contractLine Id</param>
        /// <returns>ContractLine details</returns>
        public ContractLineVO GetContractLineById(int contractLineId)
        {
            ContractLine contractLine = mdbDataContext.ContractLines.SingleOrDefault(c => c.ID == contractLineId);
            ContractLineVO contractLineVO = new ContractLineVO();

            if (contractLine != null)
            {
                contractLineVO = new ContractLineVO(contractLine);
                contractLineVO.Contract = contractLine.Contract;
            }

            return contractLineVO;
        }

        /// <summary>
        /// Delete contractLines
        /// </summary>
        /// <param name="Ids">Ids of contactLine to be deleted</param>
        /// <param name="userId">The logged in user id</param>
        public void ContractLineDelete(List<int> Ids, int? userId)
        {
            foreach (var id in Ids)
            {
                if (id != 0)
                {

                    ContractLine contractLine = new ContractLine();
                    contractLine = mdbDataContext.ContractLines.SingleOrDefault(c => c.ID == id);

                    //get contract maintenance lines by contract Line id
                    List<ContractMaintenance> contractMaintenanceLines = mdbDataContext.ContractMaintenances.Where(c => c.ContractLineID == id && c.IsDeleted == false).ToList();
                    if (contractMaintenanceLines.Count == 0)
                    {
                        //Delete contract line
                        contractLine.IsDeleted = true;
                        contractLine.LastUpdatedDate = DateTime.Now;
                        contractLine.LastUpdatedBy = userId;
                    }

                    //foreach (var contractMaintenance in contractMaintenanceLines)
                    //{
                    //    contractMaintenance.IsDeleted = true;
                    //    contractMaintenance.LastUpdatedDate = DateTime.Now;
                    //    contractMaintenance.LastUpdatedBy = userId;
                    //}

                    ////Delete milestones lines
                    //List<MilestoneLine> milestoneLines = mdbDataContext.MilestoneLines.Where(c => c.ContractID == id).ToList();
                    //foreach (var milestoneLine in milestoneLines)
                    //{
                    //    milestoneLine.IsDeleted = true;
                    //    milestoneLine.LastUpdatedDate = DateTime.Now;
                    //    milestoneLine.LastUpdatedBy = userId;
                    //}

                    ////Delete milestones
                    //List<Milestone> milestones = mdbDataContext.Milestones.Where(c => c.ContractID == id).ToList();
                    //foreach (var milestone in milestones)
                    //{
                    //    milestone.IsDeleted = true;
                    //    milestone.LastUpdatedDate = DateTime.Now;
                    //    milestone.LastUpdatedBy = userId;
                    //}
                }
            }

            mdbDataContext.SubmitChanges();
        }

        /// <summary>
        /// Save contractLines for copied contract 
        /// </summary>
        /// <param name="contractLineVOList">contractLineVO List</param>
        /// <param name="contractId">contract Id</param>
        /// <param name="userId">login userId</param>
        public void SaveCopyContractLine(List<ContractLineVO> contractLineVOList, int contractId,int? userId)
        {
            foreach (var contractLineVO in contractLineVOList)
            {
                ContractMaintenanceDAL contractMaintenanceDAL = new ContractMaintenanceDAL();
                List<ContractMaintenanceVO> contractMaintenanceVOList = contractMaintenanceDAL.GetContractMaintenanceListbyContractLineId(contractLineVO.ContractLineID);
                ContractLine copyContractLine = new ContractLine();

                copyContractLine.ContractID = contractId;
                copyContractLine.ActivityCategoryID = contractLineVO.ActivityCategoryId;
                copyContractLine.ActivityCodeID = contractLineVO.ActivityCodeId;
                copyContractLine.AccountCodeID = contractLineVO.AccountId;
                copyContractLine.JobCodeID = contractLineVO.JobCodeId;
                copyContractLine.CostCentreID = contractLineVO.CostCenterId;
                copyContractLine.CreationDate = DateTime.Now;
                copyContractLine.CreatedBy = userId;
                mdbDataContext.ContractLines.InsertOnSubmit(copyContractLine);
                mdbDataContext.SubmitChanges();

                //If contractMaintenance List is not null then copy contractMaintenance
                if (contractMaintenanceVOList != null)
                {
                    int contractLineId = copyContractLine.ID;
                    contractMaintenanceDAL.SaveCopyContractMaintenanceForContract(contractMaintenanceVOList, contractId, contractLineId,userId);
                }
            }
        }
    }
}