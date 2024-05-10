namespace WebApi___Sec3.Pagination
{
    public abstract class QueryStringParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pagesize = maxPageSize;
        public int PageSize
        {
            get
            {
                return _pagesize;
            }
            set
            {
                _pagesize = (value > maxPageSize) ? maxPageSize : value;
            }
        }


    }
}
