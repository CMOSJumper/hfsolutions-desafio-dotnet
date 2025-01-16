namespace HFSolutions.TestDotNet.Application.QueryParams
{
    public class PaginationQueryParams
    {
        private const int MaxPageSize = 30;

        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? _pageNumber : value;
        }

        private int _pageSize = 3;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
