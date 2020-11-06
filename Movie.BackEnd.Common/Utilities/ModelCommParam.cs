namespace Movie.BackEnd.Common.Utilities
{
    public class ModelCommParam
    {
        
        public ModelCommParam()
        {
            
        }
        public int? recordNumber { get; set; }
        public int? recordSize { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }
        public int? IsTotal { get; set; }
        public bool? isPaging { get; set; }
        public long? companyCode { get; set; }

        #region Common Ids
        public string searchValue { get; set; }
        //public string language { get; set; }
        //public string location { get; set; }
        #endregion

        #region Entity related
        public int? userCode { get; set; }
        public int? imdbCode { get; set; }
        // public string title { get; set; }

        #endregion


    }
}
