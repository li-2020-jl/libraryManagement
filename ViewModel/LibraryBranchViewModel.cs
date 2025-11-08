namespace LibraryManagement.ViewModels
{
    public class LibraryBranchViewModel
    {
        public int LibraryBranchId { get; set; }
        public string? BranchName { get; set; }
        public int TotalBooks { get; set; }  //total book stored in the branch
    }
}