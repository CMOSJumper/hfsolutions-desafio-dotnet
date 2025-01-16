using HFSolutions.TestDotNet.Application.QueryParams;

namespace HFSolutions.TestDotNet.Application.Responses
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPagedRecords { get; set; }
        public int Pages { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(IEnumerable<T> data, PaginationQueryParams paginationQueryParams, int totalPagedRecords, int totalRecords)
        {
            Data = data;
            PageNumber = paginationQueryParams.PageNumber;
            PageSize = paginationQueryParams.PageSize;
            TotalPagedRecords = totalPagedRecords;
            TotalRecords = totalRecords;

            var totalPages = ((double)TotalRecords / PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            Pages = roundedTotalPages;
        }
    }
}
