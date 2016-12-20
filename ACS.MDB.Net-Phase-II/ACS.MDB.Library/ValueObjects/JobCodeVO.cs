using ACS.MDB.Library.DataAccess.LINQ;

namespace ACS.MDB.Library.ValueObjects
{
    public class JobCodeVO
    {
        public int Id { get; set; }
        public string OAJobCodeId { get; set; }
        public string Name { get; set; }
        public string JobCodeName { get; set; }
        public int? CompanyId { get; set; }
        public int? CustomerId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public JobCodeVO()
        {
        }

        /// <summary>
        /// Transpose LINQ object to Value object
        /// </summary>
        /// <param name="jobCode">LINQ object</param>
        public JobCodeVO(OAJobCode jobCode)
        {
            Id = jobCode.ID;
            OAJobCodeId = jobCode.JobCodeID;
            Name = jobCode.JobCodeName;
            JobCodeName = jobCode.JobCodeID + '-' + jobCode.JobCodeName;
            CompanyId = jobCode.CompanyID;
            CustomerId = jobCode.CustomerID;
        }

        /// <summary>
        /// Transpose model object to JobCode value object
        /// </summary>
        /// <param name="jobCode"></param>
        //public JobCodeVO(JobCode jobCode)
        //{
        //    Id = jobCode.ID;
        //    OAJobCodeId = jobCode.OAJobCodeId;
        //    Name = jobCode.Name;
        //    JobCodeName = jobCode.OAJobCodeId + '-' + jobCode.Name;
        //    CompanyId = jobCode.CompanyId;
        //    CustomerId = jobCode.CustomerId;
        //}
    }
}