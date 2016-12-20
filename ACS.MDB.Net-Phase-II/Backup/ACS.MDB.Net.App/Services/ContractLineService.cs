using System.Collections.Generic;
using ACS.MDB.Library.DataAccess;
using ACS.MDB.Library.ValueObjects;

namespace ACS.MDB.Net.App.Services
{
    public class ContractLineService : BaseService
    {
        ContractLineDAL contractLineDAL = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContractLineService()
        {
            contractLineDAL = new ContractLineDAL();
        }

        /// <summary>
        /// Gets the list of ContractLines based on Contract Id
        /// </summary>
        /// <param name="contractId">ContractId</param>
        /// <returns>List of Contract Lines</returns>
        public List<ContractLineVO> GetContractLineByContractId(int contractId)
        {
            return contractLineDAL.GetContractLineByContractId(contractId);
        }

        /// <summary>
        ///  Gets the ContractLine details by contractLine id.
        /// </summary>
        /// <param name="contractLineId">contractLine Id</param>
        /// <returns>ContractLine details</returns>
        public ContractLineVO GetContractLineById(int contractLineId)
        {
            return contractLineDAL.GetContractLineById(contractLineId);
        }

        /// <summary>
        /// Save the Contract Line
        /// </summary>
        /// <param name="contractLineVO">Value object of Contract Line</param>
        public void ContractLineSave(ContractLineVO contractLineVO)
        {
            if (contractLineDAL != null)
            {
                contractLineDAL.ContractLineSave(contractLineVO);
            }        
        }

        /// <summary>
        /// Delete contractLines
        /// </summary>
        /// <param name="Ids">Ids of contactLine to be deleted</param>
        /// <param name="userId">The logged in user id</param>
        public void ContractLineDelete(List<int> Ids, int? userId)
        {
            contractLineDAL.ContractLineDelete(Ids, userId);
        }
    }
}