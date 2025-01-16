namespace HFSolutions.TestDotNet.Application.QueryParams
{
    public class UserTaskQueryParams
    {
        public int? TaskStateId { get; set; }
        public DateTime? ExpirationDateFrom { get; set; }
        public DateTime? ExpirationDateTo { get; set; }
    }
}
